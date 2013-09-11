using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleX;
using Meltdown.Core.Core;

namespace Meltdown.Core
{
    class InteractiveFictionGame
    {
        private MainConsole console = MainConsole.Instance;
        private Area currentArea = new Area("Meadow", "You stand in a grassy meadow under azure-blue skies. Boring, I know. Now what?");

        private IList<Command> knownCommands = new List<Command>()
        {
            new Command("Unknown", new string[0], () => {
                return "Not sure how to do that.";
            })            
        };
        private Command unknownCommand;

        private bool isRunning = true;

        public InteractiveFictionGame()
        {
            this.SetupCommands();
            this.unknownCommand = knownCommands.First(c => c.Name.ToLower() == "unknown");
        }

        public void Start()
        {            
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
            console.Color = Color.DarkBlue;
            string[] text = input.Split(new char[] { ' ' });
            string commandText = (text.Length > 0 ? text[0] : "");

            Command command = knownCommands.FirstOrDefault(c => c.Verbs.Select(s => s.ToUpper()).Contains(commandText.ToUpper()));
            if (command == null)
            {
                command = unknownCommand;
            }

            console.WriteLine(command.Invoke());
            
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
            this.knownCommands.Add(new Command("Quit", new string[] { "q", "quit" }, () =>
            {
                isRunning = false;
                return "Bye!";
            }));

            // All system commands so far
            this.knownCommands.Add(new Command("Look", new string[] { "l", "look" }, () =>
            {
                return string.Format("You are standing in {0}.\n\n{1}", currentArea.Name, currentArea.Description);
            }));
        }
    }
}
