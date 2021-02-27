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
    class Unit
    {
        public Spot spot;
        public int x
        {
            get { return spot.x; }
        }
        public int y
        {
            get { return spot.y; }
        }
        public string Name;
        public string Class;
        public Dictionary<string, int> Stats;
        public Dictionary<string, ItemData> Items;
        public Dictionary<string, Animation> Sprites;
        public AnimationManager manager;

        public Unit(UnitData ud,ContentManager content)
        {
            this.Name = ud.Name;
            this.Class = ud.Class;
            this.Stats = new Dictionary<string, int>(ud.Stats);
            this.Items = new Dictionary<string, ItemData>(ud.Items);
            this.Sprites = new Dictionary<string, Animation>();
            foreach (KeyValuePair<string,string[]> anim in ud.Sprites)
            {
                this.Sprites.Add(anim.Key, new Animation(anim.Value[0], anim.Value[1], anim.Value[2], anim.Value[3],content));
            }
            
        }
        public string ToString()
        {
            return $"Name: {this.Name} \n Class: {this.Class} \n Stats " + "{ " + string.Join(",", this.Stats.Select(kv => kv.Key + " = " + kv.Value).ToArray()) + "}" + "\n Items: { " + string.Join(",", this.Items.Select(kv => kv.Key + " = " + kv.Value).ToArray()) + "}" + "\n Sprites: { " + string.Join(",", this.Sprites.Select(kv => kv.Key + " = " + kv.Value).ToArray()) + "}";
        }
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
    }
}
