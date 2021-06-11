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
        public Attack Attack = null;
        public Heal Heal = null;
        public Move(Unit Source, Tile Destination, bool Undo) : base(Source, null, Undo)
        {
            this.Destination = Destination;
            this.StartingSpot = Source.Tile;
        }
        public void HealAttackExecute()
        {
            if (!Undo)
            {
                if (this.Attack != null)
                {
                    this.Attack.Undo = false;
                    this.Attack.Execute();
                }
                if (this.Heal != null)
                {
                    this.Heal.Undo = false;
                    this.Heal.Execute();
                }
            }
            else
            {
                if (this.Heal != null)
                {
                    this.Heal.Undo = true;
                    this.Heal.Execute();
                    this.Heal.Undo = false;
                }
                if (this.Attack != null)
                {
                    this.Attack.Undo = true;
                    this.Attack.Execute();
                    this.Attack.Undo = false;
                }
            }
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
            return $"Undo?: {Undo} Source: {Source.ToString()} Player: {Source.Player}" +
                $" Target: {Destination.ToString()}" + (Attack == null ? "Null" : Attack.ToString());
        }
    }
}
