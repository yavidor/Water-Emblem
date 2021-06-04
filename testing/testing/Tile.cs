using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public class Tile
    {
        #region data
        /// <summary>
        /// the X and Y values in the grid of tiles
        /// </summary>
        public int X, Y;
        /// <summary>
        /// row, col = The row and column of the tile in the tileset.
        /// frame = index of the tile in the tilset
        /// </summary>
        public int row, col,frame;
        /// <summary>
        /// The rectangle inside the tileset of this tile
        /// </summary>
        public Rectangle Rec;
        /// <summary>
        /// A list of all neighboring tiles. Not including diagonals
        /// </summary>
        public List<Tile> Neighbors = new List<Tile>();
        /// <summary>
        /// Can units pass through this tile
        /// </summary>
        public bool Walkable = false;
        /// <summary>
        /// The unit in the tile
        /// </summary>
        public Unit Unit;
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gid">index of the tile in the tilset</param>
        /// <param name="tileCount">flat index of the tile in the grid</param>
        /// <param name="Unit">The unit in the tile, sets to null if not provided</param>
        public Tile(int gid, int tileCount, Unit Unit = null)
        { 
            frame = gid;
            this.Unit = Unit;
            if (frame == 1005 || frame == 1006 || frame == 942)
                Walkable = true;
            col = gid % Game1.tilesetTilesWide;
            this.row = (int)Math.Floor((double)gid / (double)Game1.tilesetTilesWide);

             this.X = (tileCount % Game1.map.Width);
             this.Y = (int)Math.Floor(tileCount / (double)Game1.map.Width);

            this.Rec = new Rectangle(Game1.tileWidth * col, Game1.tileWidth * row, 
                Game1.tileWidth, Game1.tileWidth);
        }
        public String ToString()
        {
            return "col: " + col + " row: " + row + " x:" + X + " y:" + Y;
        }
        /// <summary>
        /// Fills the list of neighbors
        /// </summary>
        public void AddNeighbors()
        {
            if (this.X < Game1.map.Width - 1)
            {
                this.Neighbors.Add(Game1.Grid[this.X + 1 , this.Y]);
            }
            if (this.X > 0)
            {
                this.Neighbors.Add(Game1.Grid[this.X - 1 , this.Y]);
            }
            if (this.Y < Game1.map.Height - 1)
            {
                this.Neighbors.Add(Game1.Grid[this.X , this.Y + 1]);
            }
            if (this.Y > 0)
            {
                this.Neighbors.Add(Game1.Grid[this.X , this.Y - 1]);
            }
        }
        public void RemoveNeighbors()
        {
            this.Neighbors.Clear();
        }
    }
}
