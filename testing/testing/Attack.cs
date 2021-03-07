using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public class Attack : Action
    {
        public Unit Source;
        public Unit Target;
        public Attack(Unit Source, Unit Target) : base(Source, Target)
        {
            Target.TakeDamage(Source.Stats["STR"] + Source.Weapon.Stats["MT"] - Target.Stats["DEF"]);
        }
    }
}
