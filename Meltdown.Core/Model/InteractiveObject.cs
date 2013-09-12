using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Core.Model
{
    class InteractiveObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<string> Affordances { get; private set; }

        public InteractiveObject(string name, string description, IEnumerable<string> affordances)
        {
            this.Name = name;
            this.Description = description;

            this.Affordances = new List<string>();
            if (affordances != null)
            {
                this.Affordances.AddRange(affordances);
            }
        }
    }
}
