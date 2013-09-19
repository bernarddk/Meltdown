using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Core.Model;

namespace Meltdown.Core
{
    public class Command
    {
        public string Name { get; set; }
        public IEnumerable<string> Verbs { get; private set; }

        public delegate string CommandAction(InteractiveObject target, string instrument, string preposition);
        private CommandAction action;

        // For ClearScript
        public Command(string name, dynamic verbs, CommandAction action)
        {
            this.Name = name;
            this.action = action;
            if (verbs.constructor.name == "Array")
            {
                var found = new List<string>();

                for (int i = 0; i < verbs.length; i++)
                {
                    found.Add(verbs[i]);
                }

                this.Verbs = found;
            }
        }

        public Command(string name, IEnumerable<string> verbs, CommandAction action)
        {
            this.Name = name;
            this.Verbs = verbs;
            this.action = action;
        }

        // Lock.invoke(door)
        // Unlock.invoke(door, key, with)
        public string Invoke(InteractiveObject target = null, string instrument = "", string preposition = "")
        {
            return this.action.Invoke(target, instrument, preposition);
        }
    }
}
