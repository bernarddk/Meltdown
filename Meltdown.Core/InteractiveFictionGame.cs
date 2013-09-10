using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleX;

namespace Meltdown.Core
{
    class InteractiveFictionGame
    {
        MainConsole console = MainConsole.Instance;

        public void Start()
        {            
            string input = this.ShowPrompt();
            while (input.ToUpper() != "QUIT")
            {
                this.ProcessInput(input);
                input = this.ShowPrompt();
            }
        }

        private void ProcessInput(string input)
        {
            console.Color = Color.DarkBlue;
            console.WriteLine("I don't know how to do that.");
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
    }
}
