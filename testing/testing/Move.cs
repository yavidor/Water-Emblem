using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public class Move : Action
    {
        public Tile Destination;
        public Tile StartingSpot;
        public Attack Attack;
        public Heal Heal;
        public Move(Unit Source, Tile Destination, bool Undo) : base(Source, null, Undo)
        {
            this.Destination = Destination;
            StartingSpot = Source.Tile;
        }
        public void Execute()
        {
            if (!Undo)
            {
                StartingSpot.Unit = null;
                Source.Tile = Destination;
                Destination.Unit = Source;
            }
            else
            {
                Source.Tile.Unit = null;
                Source.Tile = StartingSpot;
                StartingSpot.Unit = Source;
            }
        }
        public override string ToString()
        {
            return $"Source: {Source.ToString()} Target: {Destination.ToString()}";
        }
    }
}
