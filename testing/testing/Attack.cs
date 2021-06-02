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
        private int Damage;
        public Attack(Unit Source, Unit Target, bool Undo) : base(Source, Target, Undo)
        {
            this.Damage = Source.Stats["STR"] + Source.Weapon["MT"] - Target.Stats["DEF"];
        }
        public override string ToString(){
            return $"Source: {Source.ToString()} Target: {Target.ToString()}";
        }
        public int Execute()
        {
            Target.TakeDamage(Undo ? 0 - Damage : Damage);
            return Damage;
        }
        public bool Equals(Attack other)
        {
            return base.Equals(other as Action);
        }
    }
}
