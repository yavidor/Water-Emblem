using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public class Attack : Action
    {
        #region Data
        private int Damage; //The amount of damage taken
        #endregion
        #region CTOR
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="Source">To initialize the variable with the same name</param>
        /// <param name="Target">To initialize the variable with the same name</param>
        /// <param name="Undo">To initialize the variable with the same name</param>
        public Attack(Unit Source, Unit Target, bool Undo) : base(Source, Target, Undo)
        {
            this.Damage = Source.Stats["STR"] + Source.Stats["MT"] - Target.Stats["DEF"];
            //Damage equals the strength of the attacker plus the might of the weapon minus the defence of the defender
        }
        #endregion
        #region Functions
        /// <summary>
        /// Executes the move
        /// </summary>
        /// <returns>The amount of damage taken</returns>
        public int Execute()
        {
            Target.TakeDamage(Undo ? 0 - Damage : Damage);
            return Damage;
        }
        /// <summary>
        /// Are two attack the same
        /// </summary>
        /// <param name="other">Another attack</param>
        /// <returns>True - the two attacks are the same, False - the two attacks are not the same</returns>
        public bool Equals(Attack other)
        {
            return base.Equals(other as Action);
        }
        #endregion
    }
}
