using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Core.Core
{
    class Area
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Area(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
