using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Core;
using Meltdown.Core.Model;

namespace Meltdown.Game.Model
{
    class Area
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IList<InteractiveObject> Objects { get; private set; }

        public Area(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            this.Objects = new List<InteractiveObject>();
        }

        public void RemoveObject(InteractiveObject target)
        {
            this.Objects.Remove(target);
        }
    }
}
