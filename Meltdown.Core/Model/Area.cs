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

        public void AddTwoWayExit(Direction direction, Area exit)
        {
            this.Exits[direction] = exit;
            exit.Exits[this.GetOppositeDirection(direction)] = this;
        }

        // For convenience
        public void AddObject(InteractiveObject obj)
        {
            this.Objects.Add(obj);
        }

        private Direction GetOppositeDirection(Direction source)
        {
            switch (source)
            {
                case Direction.Down: return Direction.Up;
                case Direction.Up: return Direction.Down;
                case Direction.East: return Direction.West;
                case Direction.West: return Direction.East;
                case Direction.North: return Direction.South;
                case Direction.South: return Direction.North;
                default: throw new ArgumentException("What is the opposite of " + source);
            }
        }
    }

    public enum Direction { North, South, East, West, Up, Down }
}
