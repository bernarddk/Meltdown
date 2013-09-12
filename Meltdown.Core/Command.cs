using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Core
{
    class Command
    {
        public string Name { get; set; }
        public IEnumerable<string> Verbs { get; private set; }

        internal delegate string CommandAction(string target, string instrument, string preposition);
        private CommandAction action;

        public Command(string name, IEnumerable<string> verbs, CommandAction action)
        {
            this.Name = name;
            this.Verbs = verbs;
            this.action = action;
        }

        // Lock.invoke(door)
        // Unlock.invoke(door, key, with)
        public string Invoke(string target = "", string instrument = "", string preposition = "")
        {
            return this.action.Invoke(target, instrument, preposition);
        }
    }
}
