using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronRuby;
using Meltdown.Game.Model;
using Meltdown.Core.Model;
using Meltdown.Core;
using System.Text.RegularExpressions;

namespace Meltdown.Game
{
    class InteractiveFictionGame
    {
        public Area CurrentArea { get { return this.currentArea; } }

        private Area currentArea;
        private Player player = Player.Instance;

        private List<Command> knownCommands = new List<Command>()
        {
            new Command("Unknown", new string[0], (t, i, p) => {
                return "Can't do that.";
            })            
        };

        private Command unknownCommand;
        private bool isRunning = true;

        public InteractiveFictionGame(string contentFile)
        {
            this.SetupCommands();

            this.unknownCommand = knownCommands.First(c => c.Name.ToLower() == "unknown");

            string contents = System.IO.File.ReadAllText(contentFile);
            this.currentArea = Newtonsoft.Json.JsonConvert.DeserializeObject<Area>(contents);
        }

        public void Start()
        {
            // Show the intro. Room. Area.
            this.ProcessInput("l");

            string input = this.ShowPrompt();
            while (this.isRunning)
            {
                this.ProcessInput(input);
                if (this.isRunning)
                {
                    input = this.ShowPrompt();
                }
            }
        }

        private void ProcessInput(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string[] text = input.Split(new char[] { ' ' });
            string commandText = (text.Length > 0 ? text[0] : "");

            Command command = knownCommands.FirstOrDefault(c => (c as Command).Verbs.Any(s => s.ToUpper() == commandText.ToUpper()));            

            if (command == null)
            {
                command = unknownCommand;
            }

            string content = "";
            InteractiveObject first = (text.Length <= 1 ? null : this.currentArea.Objects.FirstOrDefault(o => o.Name.ToUpper() == text[1].Trim().ToUpper()));
            if (first != null && !first.Affordances.Any(f => command.Verbs.Any(v => v.ToUpper() == f.ToUpper())))
            {
                Console.WriteLine(string.Format("You can't {0} the {1}", text[0], first.Name));
            }
            else
            {
                if (text.Length == 1)
                {
                    content = command.Invoke();
                }
                else if (text.Length == 2)
                {
                    // <command> <target>
                    content = command.Invoke(first, "", "");
                }
                else if (text.Length == 4)
                {
                    // <command> <target> <instrument> <preposition>
                    content = command.Invoke(first, text[3], text[2]);
                }

                if (content != "")
                {
                    Console.WriteLine(content);
                }

                Console.WriteLine();
            }
        }

        // Show the prompt and get some input. Input terminates with a newline ('\r').
        // Input is interactively drawn on-screen.
        private string ShowPrompt()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> ");
            //console.Refresh();

            StringBuilder toReturn = new StringBuilder();

            ConsoleKeyInfo next = ReadInputAndAppend(toReturn);
            
            while (next.Key != ConsoleKey.Enter)
            {                
                next = this.ReadInputAndAppend(toReturn);
            }

            return toReturn.ToString().Trim();
        }

        // Read the very next letter. But also, draw it to the screen.
        private ConsoleKeyInfo ReadInputAndAppend(StringBuilder toReturn)
        {
            ConsoleKeyInfo next = Console.ReadKey();
            if (next.Key == ConsoleKey.Backspace)
            {
                if (toReturn.Length > 0)
                {
                    toReturn.Remove(toReturn.Length - 1, 1);
                }
            }
            else
            {
                toReturn.Append(next.KeyChar);
            }

            if (next.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
            }
            
            return next;
        }

        private void SetupCommands()
        {
            this.SetupSystemCommands();
            this.LoadRubyCommands();
        }

        private void SetupSystemCommands()
        {
            this.knownCommands.Add(new Command("Quit", new string[] { "q", "quit" }, (t, i, p) =>
            {
                isRunning = false;
                return "Bye!";
            }));

            // All system commands so far
            this.knownCommands.Add(new Command("Look", new string[] { "l", "look" }, (t, i, p) =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(currentArea.Name);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(currentArea.Description);
                Console.WriteLine();

                foreach (InteractiveObject o in currentArea.Objects)
                {
                    Console.WriteLine(string.Format("You see a {0} here.", o.Name.ToLower()));
                }
                
                return "";
            }));

            // It'll always be a system command, even though you may override and not call the base method
            this.knownCommands.Add(new Command("Get", new string[] { "get" }, (t, i, p) =>
            {
                if (t == null) {
                    return "Get what? (Can't find that.)";
                } else {
                    if (t != null)
                    {
                        if (t.Affordances.Any(a => a.ToUpper() == "get".ToUpper()))
                        {
                            player.GetObject(t);
                            this.currentArea.RemoveObject(t);
                            return string.Format("Picked up {0}.", t.Name);
                        }
                        else
                        {
                            return string.Format("Can't pick {0} up.", t.Name);
                        }
                    }
                    else
                    {
                        return string.Format("Can't find {0} here.", t.Name);
                    }
                }
            }));

            this.knownCommands.Add(new Command("Inventory", new string[] { "inv", "i", "inventory" }, (t, i, p) =>
            {
                if (this.player.Inventory.Count == 0)
                {
                    return "You're not carrying anything right now.";
                }
                else
                {
                    StringBuilder toReturn = new StringBuilder();
                    toReturn.Append("Inventory:\n");
                    foreach (InteractiveObject o in this.player.Inventory)
                    {
                        toReturn.Append(string.Format("    {0}: {1}\n", o.Name, o.Description));
                    }
                    return toReturn.ToString();
                }
            }));
        }

        private void LoadRubyCommands()
        {            
            string[] files = System.IO.Directory.GetFiles(@"Content\Commands", "*.rb");
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            if (files.Length > 0)
            {
                var engine = Ruby.CreateEngine();
                var scope = engine.Runtime.CreateScope();
                scope.SetVariable("game", this);
                scope.SetVariable("player", this.player);

                foreach (string script in files)
                {
                    string fullPath = string.Format("{0}{1}", basePath, script);
                    string contents = System.IO.File.ReadAllText(fullPath);
                    var command = engine.Execute<Command>(contents, scope);                    
                    this.knownCommands.Add(command);
                }
            }
        }
    }
}
