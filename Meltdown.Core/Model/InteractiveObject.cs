using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Core.Model
{
    public class InteractiveObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<string> Affordances { get; private set; }

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
                    this.Affordances = ScriptHelper.ToStringList(affordances);
                }
            }
        }

        public bool Can(string affordance)
        {
            return this.Affordances.Any(a => a.ToUpper() == affordance.ToUpper());
        }

        public void Destroy()
        {
        }
    }
}
