using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public abstract class Action
    {
        #region Data
        public Unit Source { get; set; } //The unit making the action
        public Unit Target { get; set; } //The unit the action is done on
        public bool Undo; //Making the turn or canceling it
        #endregion
        #region CTOR
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="Source">To initialize the variable with the same name</param>
        /// <param name="Target">To initialize the variable with the same name</param>
        /// <param name="Undo">To initialize the variable with the same name</param>
        public Action(Unit Source, Unit Target, bool Undo)
        {
            this.Source = Source;
            this.Target = Target;
            this.Undo = Undo;
        }
        #endregion
        #region Functions
        /// <summary>
        /// Executing the move
        /// </summary>
        /// <returns></returns>
        public int Execute()
        {
            return 0;
        }
        /// <summary>
        /// Are the two actions the same
        /// </summary>
        /// <param name="other">Another action</param>
        /// <returns>True - the same action, False - not the same action</returns>
        public bool Equals(Action other)
        {
            return (this.Source.Equals(other.Source) && this.Target.Equals(other.Target) &&
                this.Undo == other.Undo);
        }
        #endregion
    }
}
