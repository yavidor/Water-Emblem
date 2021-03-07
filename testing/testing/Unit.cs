using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData;
using TiledSharp;
using Microsoft.Xna.Framework.Content;
namespace testing
{
    public class Unit
    {
        /// <summary>
        /// The spot where this unit is
        /// </summary>
        public Spot Spot;
        /// <summary>
        /// X value in the grid of spots
        /// </summary>
        public int x
        {
            get { return Spot.x; }
        }
        /// <summary>
        /// Y value in the grid of spots
        /// </summary>
        public int y
        {
            get { return Spot.y; }
        }
        /// <summary>
        /// Name of the unit
        /// </summary>
        public string Name;
        /// <summary>
        /// Class of the unit
        /// </summary>
        public string Class;
        /// <summary>
        /// Dictionary containing the stats of the unit
        /// HP: Health Points - units dies when reaches 0
        /// STR: Strength - Affects the amount of damage the unit inflicts with a non-magic attack
        /// MAG: Magic - Affects the amount of damage the unit inflicts with a magic attack
        /// SKL: TBM (To Be Removed)
        /// SPD: TBM
        /// LCK: TBM
        /// DEF: Defense - Affects the amount of damage the unit takes from a non-magic attack
        /// RES: Resistacne - Affects the amount of damage the unit takes from a magic attack
        /// CON: TBM
        /// WT: Weight - How heavy the unit is. A unit cannot pick up units who are heavier than them
        /// MOV: Movement - Determines the number of adjacent tiles the unit can move in one turn
        /// </summary>
        public Dictionary<string, int> Stats;
        /// <summary>
        /// The weapon the unit is carrying
        /// </summary>
        public Weapon Weapon;
        /// <summary>
        /// The items the unit is carrying. Maybe will be deleted
        /// </summary>
        public Dictionary<string, ItemData> Items;
        /// <summary>
        /// The animations and sprites of the unit
        /// </summary>
        public Dictionary<string, Animation> Sprites;
        /// <summary>
        /// The animation manager for this unit
        /// </summary>
        public AnimationManager manager;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ud">The data for the unit</param>
        /// <param name="content">The content manager of the game, needed to create new animations</param>
        public Unit(UnitData ud,ContentManager content)
        {
            this.Name = ud.Name;
            this.Class = ud.Class;
            this.Weapon = new Weapon(ud.Weapon);
            this.Stats = new Dictionary<string, int>(ud.Stats);
            this.Items = new Dictionary<string, ItemData>(ud.Items);
            this.Sprites = new Dictionary<string, Animation>();
            foreach (KeyValuePair<string,string[]> anim in ud.Sprites)
            {
                this.Sprites.Add(anim.Key, new Animation(anim.Value[0], anim.Value[1], anim.Value[2], anim.Value[3],content));
            }
            
        }
        /// <summary>
        /// Standart ToString function
        /// </summary>
        /// <returns>A string representing the unit</returns>
        public string ToString()
        {
            return $"Name: {this.Name} \n Class: {this.Class} \n Stats " +
                "{ " + string.Join(",", this.Stats.Select(kv => kv.Key + " = " + kv.Value).ToArray()) + "}"
                + "\n Items: { " + string.Join(",", this.Items.Select(kv => kv.Key + " = " + kv.Value).ToArray()) + "}"
                + "\n Sprites: { " + string.Join(",", this.Sprites.Select(kv => kv.Key + " = " + kv.Value).ToArray()) + "}";
        }
        /// <summary>
        /// Finds every spot the unit can reach
        /// </summary>
        /// <param name="grid">The grid of spots the game is set in</param>
        /// <returns>A list of the valid spots</returns>
        public List<Spot> ReachableSpots(Spot[,] grid)
        {
            Spot temp;
            Queue<Spot> queue = new Queue<Spot>();
            List<Spot> valid = new List<Spot>();
            int[,] dist = new int[grid.GetLength(0) , grid.GetLength(1)];
            for (int i = 0; i < dist.GetLength(0); i++)
            {
                for (int j = 0; j < dist.GetLength(1); j++)
                {
                    dist[i , j] = int.MaxValue;
                }
            }
            dist[this.x , this.y] = 0;
            queue.Enqueue(grid[this.x , this.y]);
            while (queue.Count != 0)
            {
                temp = queue.Dequeue();
                foreach (Spot neighbor in temp.neighbors)
                {
                    if (dist[neighbor.x , neighbor.y] > this.Stats["MOV"])
                    {
                        dist[neighbor.x , neighbor.y] = 1 + dist[temp.x , temp.y];
                        if (dist[neighbor.x , neighbor.y] <= this.Stats["MOV"] && grid[neighbor.x,neighbor.y].walkable)
                        {
                            queue.Enqueue(neighbor);
                        }
                    }
                }
                if (dist[temp.x , temp.y] > 0 && dist[temp.x , temp.y] <= this.Stats["MOV"])
                {
                    valid.Add(temp);
                }
            }
            return valid;
        }
        /// <summary>
        /// Reduces the HP of the unit (can heal by inputing a negative numeber)
        /// </summary>
        /// <param name="damage">Amount of damage taken</param>
        public void TakeDamage(int damage)
        {
            this.Stats["HP"] -= damage;
        }
    }
}
