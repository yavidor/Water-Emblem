using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace testing
{
    public class Map
    {
        #region Data
        /// <summary>
        /// The board in which the game takes place
        /// </summary>
        public Tile[,] Grid;
        /// <summary>
        /// Every unit in the game
        /// </summary>
        public List<Unit> Units;
        /// <summary>
        /// The map as a TMX file
        /// </summary>
        public TmxMap TmxMap;
        /// <summary>
        /// The texture of the Tiles
        /// </summary>
        public Texture2D TileSet;
        /// <summary>
        /// The width and the height of the map in tiles
        /// </summary>
        public int Width, Height;
        /// <summary>
        /// The game itself
        /// </summary>
        public Game1 Game1;
#endregion
        public Map() { }
        public void Initialize(Game1 game1, TmxMap tmxMap, Tile[,] Grid, List<Unit> units)
        {
            this.Game1 = game1;
            this.TmxMap = tmxMap;
            this.Units = new List<Unit>(units);
            Width = TmxMap.Width;
            Height = TmxMap.Height;
            TileSet = Game1.Content.Load<Texture2D>(TmxMap.Tilesets[0].Name);
            this.Grid = Grid; 

        }
        public bool WithinBounds(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < Width && y < Height);
        }

        public double DistanceBetween(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public double DistanceBetween(Tile tile1, Tile tile2)
        {
            return DistanceBetween(tile1.x, tile1.y, tile2.x, tile2.y);
        }
        public void Draw(SpriteBatch SpriteBatch)
        {
            for(int i = 0; i < Grid.GetLength(0); i++)
            {
                for(int j = 0; j < Grid.GetLength(1); j++)
                {
                    SpriteBatch.Draw(TileSet,
                        new Rectangle(TmxMap.TileWidth * i, TmxMap.TileHeight * j, TmxMap.Tilesets[0].TileWidth,
                        TmxMap.Tilesets[0].TileHeight),Grid[i,j].Rec,Color.White);
                }
            }
        }
    }
}
