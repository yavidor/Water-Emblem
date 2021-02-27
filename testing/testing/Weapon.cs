using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    class Weapon
    {
        /// <summary>
        /// Dictionary containing the stats of the unit
        /// MT: Might - Affects the amount of damage the weapon inflicts
        /// RNG: Range - How far the weapon can attack
        /// WT: Weight - How heavy the weapon is
        /// MOV: Movement - Determines the number of adjacent tiles the unit can move in one turn
        /// </summary>
        public Dictionary<String, int> Stats;
    }
}