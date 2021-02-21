using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    class Spot
    {
        public int x, y;
        public int row, col,frame;
        public Rectangle rec;
        public List<Spot> neighbors = new List<Spot>();
        public bool walkable = false;

        public Spot(int gid, int tileCount)
        {
            frame = gid;
            if (frame == 1005 || frame == 1006 || frame == 942)
                walkable = true;
            col = gid % Game1.tilesetTilesWide;
            this.row = (int)Math.Floor((double)gid / (double)Game1.tilesetTilesWide);

             this.x = (tileCount % Game1.map.Width);
             this.y = (int)Math.Floor(tileCount / (double)Game1.map.Width);

            this.rec = new Rectangle(Game1.tileWidth * col, Game1.tileHeight * row, Game1.tileWidth, Game1.tileHeight);
        }
        public String toString()
        {
            return "col: " + col + " row: " + row + " x:" + x + " y:" + y;
        }
        public void addNeighbors()
        {
            if (this.x < Game1.map.Width - 1)
            {
                this.neighbors.Add(Game1.Grid[this.x + 1 , this.y]);
            }
            if (this.x > 0)
            {
                this.neighbors.Add(Game1.Grid[this.x - 1 , this.y]);
            }
            if (this.y < Game1.map.Height - 1)
            {
                this.neighbors.Add(Game1.Grid[this.x , this.y + 1]);
            }
            if (this.y > 0)
            {
                this.neighbors.Add(Game1.Grid[this.x , this.y - 1]);
            }
        }
    }
}
