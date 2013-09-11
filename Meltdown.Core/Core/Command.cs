using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Core.Core
{
    class Command
    {
        public string Name { get; set; }
        public IEnumerable<string> Verbs { get; private set; }
        private Func<string> action;

        public Command(string name, IEnumerable<string> verbs, Func<string> action)
        {
            this.Name = name;
            this.Verbs = verbs;
            this.action = action;
        }

        public string Invoke()
        {
            return this.action.Invoke();
        }
    }
}
