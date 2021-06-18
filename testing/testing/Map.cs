
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
        #endregion
        public Map(TmxMap TmxMap, Tile[,] Grid, List<Unit> Units, Texture2D TileSet) {
            this.Units = Units;
            this.TmxMap = TmxMap;
            this.TileSet = TileSet;
            this.Grid = Grid;
        }
        public void Draw(SpriteBatch SpriteBatch)
        {
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    SpriteBatch.Draw(TileSet,
                        new Rectangle(TmxMap.TileWidth * i,
                        TmxMap.TileHeight * j, TmxMap.Tilesets[0].TileWidth,
                        TmxMap.Tilesets[0].TileHeight), Grid[i, j].Rec, Color.White);
                }
            }
        }
        public List<Move> GetAllActions()
        {
            List<Move> Moves = new List<Move>();
            foreach (Unit unit in Units)
            {
                Moves.AddRange(unit.GetActions(this.Grid));//Adding each action
            }
            return Moves;
        }
    }
}