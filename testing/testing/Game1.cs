using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using TiledSharp;
using XMLData;

namespace testing
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        UnitData ud;
        Texture2D Ike;
        Unit typo;
        Texture2D t2;
        public enum GameStates {SELECT,MOVE,ACTION,TARGET};
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static TmxMap map { get; set; }
        internal static Spot[,] Grid { get => grid; set => grid = value; }

        Texture2D tileset, highlight, cursor;
        bool flag = false;
        double timer = 0;
        private static Spot[,] grid;
        Spot chosen;
        KeyboardState lastkey;
        public static int tileWidth;
        public static int tileHeight;
        public static int tilesetTilesWide;
        public static int tilesetTilesHigh;
        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            ud = Content.Load<UnitData>("Ike");
            Ike = Content.Load<Texture2D>(ud.Sprites["Protrait"]);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            cursor = Content.Load<Texture2D>("chosen");
            highlight = Content.Load<Texture2D>("blue");
            map = new TmxMap("Content/balanced.tmx");
            tileset = Content.Load<Texture2D>(map.Tilesets[0].Name.ToString());
            Console.WriteLine(tileset);
            Grid = new Spot[map.Width, map.Height];
            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;
            graphics.PreferredBackBufferHeight = map.Height * map.TileHeight;
            graphics.PreferredBackBufferWidth = map.Width * map.TileWidth;
            graphics.ApplyChanges();
            tilesetTilesWide = tileset.Width / tileWidth;
            tilesetTilesHigh = tileset.Height / tileHeight;
            for (int i = 0; i < map.Layers[0].Tiles.Count; i++)
            {
                Spot spot = new Spot(map.Layers[0].Tiles[i].Gid - 1, i);
                Grid[spot.x, spot.y] = spot;
            }
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    Grid[i, j].addNeighbors();
                }
            }
            chosen = Grid[0, 0];
            typo = new Unit(ud);
            typo.spot = chosen;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && lastkey.IsKeyUp(Keys.Space))
            {
                flag = !flag;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && lastkey.IsKeyUp(Keys.Right) && ((int)chosen.x) < map.Width - 1)
            {
                chosen = Grid[((int)chosen.x) + 1, ((int)chosen.y)];
                typo.spot = chosen;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && lastkey.IsKeyUp(Keys.Left) && ((int)chosen.x) > 0)
            {
                chosen = Grid[((int)chosen.x) - 1, ((int)chosen.y)];
                typo.spot = chosen;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && lastkey.IsKeyUp(Keys.Up) && ((int)chosen.y) > 0)
            {
                chosen = Grid[((int)chosen.x), ((int)chosen.y) - 1];
                typo.spot = chosen;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && lastkey.IsKeyUp(Keys.Down) && ((int)chosen.y) < map.Height - 1)  
            {
                chosen = Grid[((int)chosen.x), ((int)chosen.y) + 1];
                typo.spot = chosen;
                Console.WriteLine(typo.ToString());
                Console.WriteLine(tileHeight);
              
            }
            lastkey = Keyboard.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            timer += 0.1;
           
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if (Grid[i, j].frame == 1)
                    {

                        Console.WriteLine("Oh oh");
                    }
                    else
                    {

                        spriteBatch.Draw(tileset, new Rectangle((int)Grid[i, j].x * map.TileWidth, (int)Grid[i, j].y * map.TileHeight, tileWidth, tileHeight), Grid[i, j].rec, Color.White);
                        List<Spot> ls = typo.ReachableSpots(Grid);
                        if (Grid[i, j].walkable && flag && ls.Contains(Grid[i, j]))
                        {
                            // spriteBatch.Draw(high, new Vector2(spot.x, spot.y), Color.White * 0.2f);
                            spriteBatch.Draw(highlight, new Rectangle((int)Grid[i, j].x * map.TileWidth, (int)Grid[i, j].y * map.TileHeight,
                                tileWidth, tileHeight), new Rectangle(((int)timer % 16) * tileWidth, 0, tileWidth, tileHeight),
                                Color.White * 0.75f);
                        }
                    }
                }
            }
            //spriteBatch.Draw(t2, new Vector2((int)chosen.x * map.TileWidth, (int)chosen.y * map.TileHeight), Color.White);
            spriteBatch.Draw(cursor, new Vector2((int)chosen.x*map.TileWidth, (int)chosen.y*map.TileHeight), Color.White * 0.75f);
            spriteBatch.Draw(Ike, new Vector2((int)chosen.x * map.TileWidth, (int)chosen.y * map.TileHeight), Color.White * 0.75f);
            spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }