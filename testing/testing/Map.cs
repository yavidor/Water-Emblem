using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public void Initialize(TmxMap tmxMap, Tile[,] Grid, List<Unit> Units, ContentManager Content)
        {
            this.TmxMap = tmxMap;
            TileSet = Content.Load<Texture2D>(TmxMap.Tilesets[0].Name);
            this.Grid = Grid; 

        }
        public void Draw(SpriteBatch SpriteBatch)
        {
            for(int i = 0; i < Grid.GetLength(0); i++)
            {
                for(int j = 0; j < Grid.GetLength(1); j++)
                {
                    SpriteBatch.Draw(TileSet,
                        new Rectangle(TmxMap.TileWidth * i,
                        TmxMap.TileHeight * j, TmxMap.Tilesets[0].TileWidth,
                        TmxMap.Tilesets[0].TileHeight),Grid[i,j].Rec,Color.White);
                }
            }
        }
    }
}
