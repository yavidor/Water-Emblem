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
    class Map
    {
        public Tile[,] Grid;
        public Rectangle[] tilesRect;
        public TmxMap TmxMap;
        public Texture2D TileSet;
        public int Width, Height;
        public Game1 Game1;
        public Map() { }
        public void Initialize(Game1 game1, TmxMap tmxMap, Tile[,] Grid)
        {
            this.Game1 = game1;
            this.TmxMap = tmxMap;
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
                        new Rectangle(TmxMap.TileWidth * i, TmxMap.TileHeight * j, TmxMap.Tilesets[0].TileWidth, TmxMap.Tilesets[0].TileHeight),Grid[i,j].rec,Color.White);
                }
            }
        }
    }
}
