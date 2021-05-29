using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public class Heal : Action
    {
        private int HealFactor = 3;
        public Heal(Unit Source, Unit Target, bool Undo) : base(Source, Target, Undo)
        {

        }
        public override string ToString()
        {
            return $"SourceHeal: {Source.ToString()} Target: {Target.ToString()}";
        }
        public int Execute()
        {
            int healing =
            Target.Stats["HP"] + Source.Stats["STR"] / HealFactor + Source.Weapon["MT"]
            > Target.Stats["MaxHp"] ?
            Target.Stats["MaxHp"] - Target.Stats["HP"] :
            Source.Stats["STR"] / 3 + Source.Weapon["MT"];
            if (Undo)
            {
                healing *= -1;
            }
            Target.TakeDamage(0-healing);
            return healing;
        }
        public bool Equals(Attack other)
        {
            return base.Equals(other as Action);
        }
    }
}
