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
        public bool Undo;
        public Action()
        {}

        public Action(Unit Source,Unit Target, bool Undo)
        {
            this.Source = Source;
            this.Target = Target;
            this.Undo = Undo;
        }
        public void Execute()
        {

        }
        public String ToString()
        {
            return $"Soure: {this.Source.ToString()} Target: {this.Target.ToString()}";
        }
    }
}
