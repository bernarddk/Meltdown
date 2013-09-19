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
            if (verbs is IEnumerable<string>)
            {
                this.Verbs = verbs;
            } else if (ScriptHelper.IsArray(verbs))
            {
                this.Verbs = ScriptHelper.ToStringList(verbs);
            }
        }

        // Lock.invoke(door)
        // Unlock.invoke(door, key, with)
        public string Invoke(InteractiveObject target = null, string instrument = "", string preposition = "")
        {
            return this.action.Invoke(target, instrument, preposition);
        }
    }
}
