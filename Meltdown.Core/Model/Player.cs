using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Core;

namespace Meltdown.Core.Model
{
    public class Player
    {
        public List<dynamic> Inventory { get; private set; }
        public static Player Instance { get; private set; }

        static Player()
        {
            Instance = new Player();
        }

        private Player()
        {
            this.Inventory = new List<dynamic>();            
        }

        public void GetObject(dynamic o)
        {
            this.Inventory.Add(o);
        }

        public void LoseObject(dynamic o)
        {
            this.Inventory.Remove(o);
        }
    }
}
