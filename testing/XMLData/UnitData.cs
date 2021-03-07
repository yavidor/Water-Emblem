using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLData
{
    public class UnitData
    
        {
        public string Name;
        public string Class;
        public WeaponData Weapon;   
        public Dictionary<string,int> Stats;
        public Dictionary<string,ItemData> Items;
        public Dictionary<string, string[]> Sprites;
   
        }
    }
