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
        public float x, y;
        public int row, col,frame;
        public Rectangle rec;
        public bool walkable = false;

        public Spot(int gid, int tileCount)
        {
            frame = gid;
            if (frame == 1005 || frame == 1006 || frame == 942)
                walkable = true;
            col = gid % Game1.tilesetTilesWide;
            this.row = (int)Math.Floor((double)gid / (double)Game1.tilesetTilesWide);

             this.x = (tileCount % Game1.map.Width) * Game1.map.TileWidth;
             this.y = (float)Math.Floor(tileCount / (double)Game1.map.Width) * Game1.map.TileHeight;

            this.rec = new Rectangle(Game1.tileWidth * col, Game1.tileHeight * row, Game1.tileWidth, Game1.tileHeight);
        }
        public String toString()
        {
            return frame.ToString();
        }
    }
}
