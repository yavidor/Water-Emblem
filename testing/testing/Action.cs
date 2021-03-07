using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public abstract class Action
    {
        public Unit Source { get; set; }
        public Unit Target { get; set; }
        public Unit SourceAfter;
        public Unit TargetAfter;
        public Action()
        {}

        public Action(Unit Source,Unit Target)
        {
            this.Source = Source;
            this.Target = Target;
        }
        public void Execute(bool redo)
        {

        }
        public String ToString()
        {
            return $"Soure: {this.Source.ToString()} Target: {this.Target.ToString()}";
        }
    }
}
