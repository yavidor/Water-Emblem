using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData
{
    public class Dtyoe
    {

        public String[] num;
        public String str;
        public String boo;
        public String ToString()
        {
            return "num: " + string.Join(",", num) + " str: " + str + " boo: " + boo;
        }

    }
}
