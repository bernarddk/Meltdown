using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Game.Model
{
    class Player
    {
        public List<InteractiveObject> Inventory { get; private set; }

        public Player()
        {
            this.Inventory = new List<InteractiveObject>();
        }

        public void GetObject(InteractiveObject o)
        {
            this.Inventory.Add(o);
        }
    }
}
