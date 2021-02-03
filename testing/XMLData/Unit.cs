using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLData
{
    public class Unit
    
        {
        public string Name;
        public string Class;
        public Dictionary<string,int> Stats;
        public Dictionary<string,Item> Items;
        public Dictionary<string, string> Sprites;
   
        public String ToString()
        {
            return "Name: " + Name;
        }
        }
    }
