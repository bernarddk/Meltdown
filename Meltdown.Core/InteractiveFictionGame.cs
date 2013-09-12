using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleX;
using Meltdown.Core.Model;

namespace Meltdown.Core
{
    class InteractiveFictionGame
    {
        private MainConsole console = MainConsole.Instance;
        private Area currentArea;
        private Player player = new Player();

        private IList<Command> knownCommands = new List<Command>()
        {
            new Command("Unknown", new string[0], (t, i, p) => {
                return "Not sure how to do that.";
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
            console.Color = Color.DarkCyan;
            string[] text = input.Split(new char[] { ' ' });
            string commandText = (text.Length > 0 ? text[0] : "");

            Command command = knownCommands.FirstOrDefault(c => c.Verbs.Select(s => s.ToUpper()).Contains(commandText.ToUpper()));
            if (command == null)
            {
                command = unknownCommand;
            }

            string content = "";
            if (text.Length == 1)
            {
                content = command.Invoke();
            } else if (text.Length == 2)
            {
                // <command> <target>
                content = command.Invoke(text[1]);
            }
            else if (text.Length == 4)
            {
                // <command> <target> <preposition> <instrument>
                content = command.Invoke(text[1], text[3], text[2]);
            }

            if (content != "")
            {
                console.WriteLine(content);
            }
            console.WriteLine();
            
            console.Refresh();
        }

        // Show the prompt and get some input. Input terminates with a newline ('\r').
        // Input is interactively drawn on-screen.
        private string ShowPrompt()
        {
            console.Color = Color.Grey;
            console.Write("> ");
            console.Refresh();

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
                console.Backspace();
            }
            else
            {
                toReturn.Append(next.KeyChar);
                console.Write(next.KeyChar);
            }
            console.Refresh();

            return next;
        }

        private void SetupCommands()
        {
            this.SetupSystemCommands();
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
                console.Color = Color.Cyan;
                console.WriteLine(currentArea.Name);
                console.Color = Color.DarkCyan;
                console.WriteLine(currentArea.Description);
                console.WriteLine();

                foreach (InteractiveObject o in currentArea.Objects)
                {
                    console.WriteLine(string.Format("You see a {0} here.", o.Name.ToLower()));
                }

                return "";
            }));

            // It'll always be a system command, even though you may override and not call the base method
            this.knownCommands.Add(new Command("Get", new string[] { "get" }, (t, i, p) =>
            {
                if (t == "") {
                    return "Get what?";
                } else {
                    // Find the object
                    var found = this.currentArea.Objects.FirstOrDefault(o => o.Name.ToUpper() == t.ToUpper());
                    if (found != null)
                    {
                        if (found.Affordances.Any(a => a.ToUpper() == "get".ToUpper()))
                        {
                            player.GetObject(found);
                            this.currentArea.RemoveObject(found);
                            return string.Format("Picked up {0}.", found.Name);
                        }
                        else
                        {
                            return string.Format("Can't pick {0} up.", found.Name);
                        }
                    }
                    else
                    {
                        return string.Format("Can't find {0} here.", t);
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
    }
}
