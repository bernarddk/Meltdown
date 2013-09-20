using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Core;
using Meltdown.Core.Model;

namespace Meltdown.Core.Model
{
    public class Area
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IList<InteractiveObject> Objects { get; private set; }
        public IDictionary<Direction, Area> Exits { get; set; }

        public Area(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            this.Objects = new List<InteractiveObject>();
            this.Exits = new Dictionary<Direction, Area>(6);
        }        
    }

    public enum Direction { North, South, East, West, Up, Down }
}
