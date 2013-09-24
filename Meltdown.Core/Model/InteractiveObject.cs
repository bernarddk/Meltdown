using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptMediator;

namespace Meltdown.Core.Model
{
    public class InteractiveObject
    {
        public string Name { get; private set; }
        public string Description { get; set; }
        public List<string> Affordances { get; private set; }
        private IDictionary<string, Action> commandListeners = new Dictionary<string, Action>();

        public InteractiveObject(string name, string description)
            : this(name, description, new string[0])
        {
        }

        // Dynamic: .NET uses IEnumerable<string>, ClearScript uses some V8ScriptItem thing.
        public InteractiveObject(string name, string description, dynamic affordances)
        {
            this.Name = name;
            this.Description = description;

            this.Affordances = new List<string>();
            if (affordances != null)
            {
                if (affordances is IEnumerable<string>)
                {
                    this.Affordances.AddRange(affordances);
                } else if (ScriptHelper.IsArray(affordances)) {
                    this.Affordances = ScriptHelper.ToList<string>(affordances);
                }
            }
        }

        public void AfterCommand(string name, Action action) {
            this.commandListeners[name] = action;
        }

        internal bool Can(string affordance)
        {
            return this.Affordances.Any(a => a.ToUpper() == affordance.ToUpper());
        }

        public void Destroy()
        {
        }

        internal bool ListensFor(string commandName)
        {
            return this.commandListeners.Keys.Any(k => k.ToUpper() == commandName.ToUpper());
        }

        internal void ProcessCommand(string commandName)
        {
            IEnumerable<string> keys = this.commandListeners.Keys.Where(k => k.ToUpper() == commandName.ToUpper());
            foreach (string key in keys)
            {
                var listener = this.commandListeners[key];
                if (listener != null)
                {
                    listener.Invoke();
                }
            }
        }
    }
}
