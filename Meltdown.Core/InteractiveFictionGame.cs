using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meltdown.Core.Model;
using Meltdown.Core;
using System.Text.RegularExpressions;
using System.IO;
using ScriptRunner.Core;

namespace Meltdown.Game
{
    internal class InteractiveFictionGame
    {
        public Area CurrentArea { get { return this.currentArea; } }

        private Area currentArea;
        private Player player = Player.Instance;
        private Runner runner = Runner.Instance;

        private List<Command> knownCommands = new List<Command>()
        {
            new Command("Unknown", new string[0], (t, i, p) => {
                return "Can't do that.";
            })            
        };

        private Command unknownCommand;
        private Command lookCommand;
        private bool isRunning = true;

        // For testing
        internal InteractiveFictionGame()
        {
            this.SetupCommands();

            this.unknownCommand = knownCommands.First(c => c.Name.ToLower() == "unknown");
            this.lookCommand = knownCommands.First(c => c.Name.ToLower() == "look");

            this.BindApiParameters();
        }

        public InteractiveFictionGame(string contentFile) : this()
        {            
            this.LoadGame();
        }

        private void BindApiParameters()
        {
            Runner.Instance
                .BindParameter("game", this)
                .BindParameter("player", this.player);
        }

        private void LoadGame()
        {
            IEnumerable<string> gameFiles = Directory.GetFiles(@"Content\Games", "*.*").Where(s => s.ToLower().EndsWith(".rb") || s.ToLower().EndsWith(".js"));
            if (gameFiles.Count() == 0)
            {
                throw new NotImplementedException("Looks like you don't have any games in Content\\Games. Add some.");
            }
            else if (gameFiles.Count() == 1)
            {
                this.currentArea = Runner.Instance.Execute<Area>(gameFiles.First());
            }
            else
            {
                Console.WriteLine("Available games:");
                int next = 1;

                // Pick
                foreach (string file in gameFiles)
                {
                    Console.WriteLine(string.Format("{0}) {1}", next, file));
                    next++;
                }

                Console.WriteLine("Load which game number?");
                string input = Console.ReadLine();
                int number = 0;
                if (int.TryParse(input, out number))
                {
                    if (number > 0 && number - 1 < gameFiles.Count())
                    {
                        string game = gameFiles.ElementAt(number - 1);
                        this.currentArea = Runner.Instance.Execute<Area>(game);
                    }
                    else
                    {
                        Console.WriteLine("That game doesn't exist, dummy!");
                    }
                }
            }
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
                Console.WriteLine(unknownCommand.Invoke());
            }
            else
            {

                string content = "";

                if (text.Length == 1)
                {
                    // Not invoked on any object.
                    content = command.Invoke();
                }
                else if (text.Length > 1)
                {
                    // Invoked on an object. Does that object exist?
                    InteractiveObject first = (text.Length <= 1 ? null : this.currentArea.Objects.FirstOrDefault(o => o.Name.ToUpper() == text[1].Trim().ToUpper()));
                    if (first != null)
                    {
                        // It exists. Check for custom AfterCommand handlers
                        bool processed = false;
                        if (first.ListensFor(command.Name))
                        {
                            first.ProcessCommand(command.Name);
                            processed = true;
                        }

                        // Find out if it has the right affordance (eg. getting a get-able object)
                        bool hasAffordance = first.Affordances.Any(f => command.Verbs.Any(v => v.ToUpper() == f.ToUpper()));
                        if (hasAffordance)
                        {
                            content = command.Invoke(first);
                        }
                        else if (!processed)

                        // No handlers, and no affordances.
                        {
                            Console.WriteLine(string.Format("You can't {0} the {1}", text[0], first.Name));
                        }
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
                    else
                    {
                        content = "You can't do that.";
                    }
                }
                if (content != "")
                {
                    Console.WriteLine(content);
                }
            }
        }

        // Show the prompt and get some input. Input terminates with a newline ('\r').
        // Input is interactively drawn on-screen.
        private string ShowPrompt()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> ");

            StringBuilder toReturn = new StringBuilder();

            ConsoleKeyInfo next = this.ReadInputAndAppend(toReturn);
            
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
                    this.WriteBackspace();
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
            this.LoadScriptedCommands();
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

                if (currentArea.Objects.Any())
                {
                    foreach (InteractiveObject o in currentArea.Objects)
                    {
                        Console.WriteLine(string.Format("You see a {0} here.", o.Name.ToLower()));
                    }
                    Console.WriteLine();
                }

                if (currentArea.Exits.Any())
                {
                    Console.WriteLine("Exits:");
                    foreach (var exit in currentArea.Exits)
                    {
                        Console.WriteLine(string.Format("\t{0} to {1}", exit.Key, exit.Value.Name));
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("There are no exits.");
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
                            this.currentArea.Objects.Remove(t);
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

            // Directions: NSEW. Forget up/down for now.
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                string name = direction.ToString();
                string firstLetter = name.First().ToString();

                this.knownCommands.Add(new Command(name, new string[] { firstLetter, name.ToLower() }, (t, i, p) =>
                {
                    if (this.currentArea.Exits.ContainsKey(direction))
                    {
                        this.currentArea.ExecuteOnExit();
                        this.currentArea = this.currentArea.Exits[direction];
                        Console.WriteLine(string.Format("You travel {0}.\n", name.ToLower()));
                        this.lookCommand.Invoke();
                        this.currentArea.ExecuteOnEnter();
                        return "";
                    }
                    else
                    {
                        return string.Format("You can't travel {0} from here.\n", name.ToLower());
                    }
                }));
            }
        }

        private void LoadScriptedCommands()
        {
            if (Directory.Exists(@"Content\Commands"))
            {
                string[] allFiles = System.IO.Directory.GetFiles(@"Content\Commands", "*.*");
                IEnumerable<string> files = allFiles.Where(f => f.EndsWith(".rb") || f.EndsWith(".js"));

                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                if (files.Any())
                {
                    foreach (string script in files)
                    {
                        string fullPath = string.Format("{0}{1}", basePath, script);
                        var command = runner.Execute<Command>(fullPath);
                        this.knownCommands.Add(command);
                    }
                }
            }
        }

        private void WriteBackspace()
        {
            // Why doesn't writing "\b" work?
            Console.Write(" ");
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
    }
}
