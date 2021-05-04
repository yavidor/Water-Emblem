using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData;
using TiledSharp;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace testing
{
    public class Unit
    {
        /// <summary>
        /// The Tile where this unit is
        /// </summary>
        public Tile Tile;
        /// <summary>
        /// X value in the grid of tiles
        /// </summary>
        public int x
        {
            get { return Tile.x; }
        }
        /// <summary>
        /// Y value in the grid of tiles
        /// </summary>
        public int y
        {
            get { return Tile.y; }
        }
        /// <summary>
        /// Name of the unit
        /// </summary>
        public string Name;
        /// <summary>
        /// Which player controls of the unit.
        /// True = Player 1 (The host)
        /// False = Player 2 (The computer in case of a game against the computer)
        /// </summary>
        public bool Player;
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
        /// The animations and sprites of the unit
        /// </summary>
        public Animation Sprite;
        /// <summary>
        /// The animation manager for this unit
        /// </summary>
        public AnimationManager Manager;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ud">The data for the unit</param>
        /// <param name="content">The content manager of the game, needed to create new animations</param>
        public Unit(UnitData ud,ContentManager Content)
        {
            this.Name = ud.Name;
            this.Player = ud.Player;
            this.Weapon = new Weapon(ud.Weapon);
            this.Stats = new Dictionary<string, int>(ud.Stats);
            this.Sprite = new Animation($"Sprites/{this.Name}{Convert.ToInt32(this.Player)}", ud.Sprite[0], ud.Sprite[1], ud.Sprite[2],Content);
            
            
        }
        /// <summary>
        /// Standart ToString function
        /// </summary>
        /// <returns>A string representing the unit</returns>
        public string ToString()
        {
            return this.Name;
          /*  return $"Name: {this.Name} \n Class: {this.Class} \n Stats " +
                "{ " + string.Join(",", this.Stats.Select(kv => kv.Key + " = " + kv.Value).ToArray()) + "}"
                + "\n Items: { " + string.Join(",", this.Items.Select(kv => kv.Key + " = " + kv.Value).ToArray()) + "}"
                + "\n Sprites: { " + string.Join(",", this.Sprites.Select(kv => kv.Key + " = " + kv.Value).ToArray()) + "}";
    */   
    }
        /// <summary>
        /// Finds every tiles the unit can reach
        /// </summary>
        /// <param name="grid">The grid of tiles the game is set in</param>
        /// <param name="WalkOrAttack">Tells the function
        /// if it should use movement distance of the unit
        /// or the range of the weapon, true is movement and false is weapon range</param>
        /// <returns>A list of the valid tiles</returns>
        public List<Tile> ReachableTiles(Map Map, bool WalkOrAttack)
        {
            int? range = WalkOrAttack ? this.Stats["MOV"] : this.Weapon.Stats["RNG"];
            Tile temp;
            Tile[,] grid = Map.Grid;
            Queue<Tile> queue = new Queue<Tile>();
            List<Tile> valid = new List<Tile>();
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
                foreach (Tile neighbor in temp.Neighbors)
                {
                    if (dist[neighbor.x , neighbor.y] > range)
                    {
                        dist[neighbor.x , neighbor.y] = 1 + dist[temp.x , temp.y];
                        if (dist[neighbor.x , neighbor.y] <= range && grid[neighbor.x,neighbor.y].Walkable)
                        {
                            queue.Enqueue(neighbor);
                        }
                    }
                }
                if (dist[temp.x , temp.y] > 0 && dist[temp.x , temp.y] <= range)
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
        /// <summary>
        /// Generates every possible action this unit can take
        /// </summary>
        /// <param name="Map">The current board</param>
        /// <returns>A list of all possible actions</returns>
        public List<Action> GetActions(Map Map)
        {
            List<Action> actions = new List<Action>();
            List<Tile> Moves = this.ReachableTiles(Map,true);
            foreach (Tile tile in Moves)
            {
                if (tile.Unit == null) {
                    Move move = new Move(this, tile, false);
                    actions.Add(move);
                    move.Execute();
                    foreach (Tile tileAttack in this.ReachableTiles(Map, false))
                    {
                        if (tileAttack.Unit != null)
                        {
                            Attack attack = new Attack(this, tileAttack.Unit, false);
                            actions.Add(attack);
                        }
                    }
                    move.Undo = true;
                    move.Execute();
                }
            }
                return actions;
        }
    }
}
