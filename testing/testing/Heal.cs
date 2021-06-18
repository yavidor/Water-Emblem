using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public class Heal : Action
    {
        private const int HealFactor = 3;
        private int Healing;
        public Heal(Unit Source, Unit Target, bool Undo) : base(Source, Target, Undo)
        {
            if (Target != null)
            {
                Healing =
               Target.Stats["HP"] + Source.Stats["STR"] / HealFactor + Source.Stats["MT"]
               > Target.Stats["MaxHp"] ?
               Target.Stats["MaxHp"] - Target.Stats["HP"] :
               Source.Stats["STR"] / HealFactor + Source.Stats["MT"];
            }
        }
        public override string ToString()
        {
            return $"SourceHeal: {Source.ToString()} Target: {Target.ToString()}";
        }
        public int Execute()
        {
            Target.TakeDamage(Undo ? Healing : 0 - Healing);
            return Healing;
        }
        public bool Equals(Attack other)
        {
            return base.Equals(other as Action);
        }
    }
}
