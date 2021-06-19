using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public class Heal : Action
    {
        private const int HealFactor = 3; //A part of the healing formula
        private int Healing; //Amount of damage healed
        public Heal(Unit Source, Unit Target, bool Undo) : base(Source, Target, Undo)
        {
            if (Target != null)
            {
                Healing =
               Target.Stats["HP"] + Source.Stats["STR"] / HealFactor + Source.Stats["MT"]
               > Target.Stats["MaxHp"] ?
               Target.Stats["MaxHp"] - Target.Stats["HP"] :
               Source.Stats["STR"] / HealFactor + Source.Stats["MT"];
                //Healing equals to the strength of the healer divided by the heal factor plus the might of the healer weapon
                //Unless this number exceedes the healed unit's maximum HP. Then it is just brings the HP to full
            }
        }
        /// <summary>
        /// Executes the move
        /// </summary>
        /// <returns>The amount of damage healed</returns>
        public int Execute()
        {
            Target.TakeDamage(Undo ? Healing : 0 - Healing);
            return Healing;
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
    }
}
