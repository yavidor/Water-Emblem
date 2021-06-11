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
        public bool Undo;
        public abstract string ToString();
        public Action(Unit Source, Unit Target, bool Undo)
        {
            this.Source = Source;
            this.Target = Target;
            this.Undo = Undo;
        }
        public int Execute()
        {
            return 0;
        }
        public bool Equals(Action other)
        {
            return (this.Source.Equals(other.Source) && this.Target.Equals(other.Target) &&
                this.Undo == other.Undo);
        }
    }
}
