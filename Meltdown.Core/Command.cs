using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Core
{
    public class Command
    {
        public string Name { get; set; }
        public string[] Verbs { get; private set; }

        public delegate string CommandAction(string targetName, string instrument, string preposition);
        private CommandAction action;

        public Command(string name, string[] verbs, CommandAction action)
        {
            this.Name = name;
            this.Verbs = verbs;
            this.action = action;
        }

        // Lock.invoke(door)
        // Unlock.invoke(door, key, with)
        public string Invoke(string targetName = "", string instrument = "", string preposition = "")
        {
            return this.action.Invoke(targetName, instrument, preposition);
        }
    }
}
