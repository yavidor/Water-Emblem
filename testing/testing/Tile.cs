using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    [Serializable]
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
        public int Row, Col, Frame;
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
        #region CTOR
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="Index">index of the tile in the tilset</param>
        /// <param name="TileCount">flat index of the tile in the grid</param>
        /// <param name="Unit">The unit in the tile, sets to null if not provided</param>
        public Tile(int Index, int TileCount, Unit Unit = null)
        {
            Frame = Index;
            this.Unit = Unit;
            if (Frame == 1005 || Frame == 1006 || Frame == 942)
                Walkable = true;
            Col = Index % Game1.TilesetTilesWide;
            this.Row = (int)Math.Floor((double)Index / (double)Game1.TilesetTilesWide);

            this.X = (TileCount % Game1.TmxMap.Width);
            this.Y = (int)Math.Floor(TileCount / (double)Game1.TmxMap.Width);

            this.Rec = new Rectangle(Game1.TileWidth * Col, Game1.TileWidth * Row,
                Game1.TileWidth, Game1.TileWidth);
        }
        #endregion
        #region Functions
        /// <summary>
        /// Fills the list of neighbors
        /// </summary>
        public void AddNeighbors()
        {
            if (this.X < Game1.TmxMap.Width - 1)
            {
                this.Neighbors.Add(Game1.Grid[this.X + 1, this.Y]);
            }
            if (this.X > 0)
            {
                this.Neighbors.Add(Game1.Grid[this.X - 1, this.Y]);
            }
            if (this.Y < Game1.TmxMap.Height - 1)
            {
                this.Neighbors.Add(Game1.Grid[this.X, this.Y + 1]);
            }
            if (this.Y > 0)
            {
                this.Neighbors.Add(Game1.Grid[this.X, this.Y - 1]);
            }
        }
        #endregion
    }
}
