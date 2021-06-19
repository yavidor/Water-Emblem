using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public class Move : Action
    {
        #region Data
        public Tile Destination; //The tile the unit starts on
        public Tile StartingSpot; //The tile the unit goes to
        public Attack Attack = null; //The attack the unit is performing
        public Heal Heal = null; //The heal the unit is performing
        #endregion
        #region CTOR
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="Source">To initialize the variable with the same name</param>
        /// <param name="Destination">To initialize the variable with the same name</param>
        /// <param name="Undo">To initialize the variable with the same name</param>
        public Move(Unit Source, Tile Destination, bool Undo) : base(Source, null, Undo)
        {
            this.Destination = Destination;
            this.StartingSpot = Source.Tile;
        }
        #endregion
        #region Functions
        /// <summary>
        /// Executes the heal and the attack if possible
        /// </summary>
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
        /// <summary>
        /// Executes the move
        /// </summary>
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
        #endregion
    }
}
