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
        public enum GameStates {SELECT,MOVE,ACTION,TARGET};
        UnitData ud;
        Texture2D IkeTexture;
        Unit Ike, ActiveUnit;
        GameStates State = GameStates.SELECT;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static TmxMap map { get; set; }
        internal static Spot[,] Grid { get => grid; set => grid = value; }

        Texture2D tileset, blue, cursor,red;
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ud = Content.Load<UnitData>("Ike");
            font = Content.Load<SpriteFont>("font");
            cursor = Content.Load<Texture2D>("chosen");
            blue = Content.Load<Texture2D>("blue");
            red = Content.Load<Texture2D>("red");
            map = new TmxMap("Content/balanced.tmx");
            tileset = Content.Load<Texture2D>(map.Tilesets[0].Name.ToString());
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
                    Grid[i, j].AddNeighbors();
                }
            }
            chosen = Grid[10, 19];
            Ike = new Unit(ud, Content)
            {
                Spot = Grid[10, 20]
            };
            Grid[10, 20].unit = Ike;
            Ike.manager.Play(Ike.Sprites["Portrait"]);
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
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && lastkey.IsKeyUp(Keys.Left) && ((int)chosen.x) > 0)
            {
                chosen = Grid[((int)chosen.x) - 1, ((int)chosen.y)];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && lastkey.IsKeyUp(Keys.Up) && ((int)chosen.y) > 0)
            {
                chosen = Grid[((int)chosen.x), ((int)chosen.y) - 1];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && lastkey.IsKeyUp(Keys.Down) && ((int)chosen.y) < map.Height - 1)  
            {
                chosen = Grid[((int)chosen.x), ((int)chosen.y) + 1];              
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z) && lastkey.IsKeyUp(Keys.Z))
            {
                switch (State)
                {
                    case GameStates.SELECT:
                        if (chosen.unit != null)
                        {
                            State = GameStates.MOVE;
                            ActiveUnit = chosen.unit;
                            ActiveUnit.manager.Play(ActiveUnit.Sprites["Running"]);
                        }
                        break;
                    case GameStates.MOVE:
                        if (chosen.unit == null && ActiveUnit.ReachableSpots(Grid).Contains(chosen))
                        {
                            State = GameStates.SELECT;
                            ActiveUnit.Spot.unit = null;
                            ActiveUnit.Spot = chosen;
                            chosen.unit = ActiveUnit;
                            ActiveUnit.manager.Play(ActiveUnit.Sprites["Portrait"]);
                        }
                        break;
                    default:
                        break;

                }
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
                        List<Spot> ls = Ike.ReachableSpots(Grid);
                        if (flag && ls.Contains(Grid[i, j]))
                        {
                            if (State == GameStates.SELECT)
                                spriteBatch.Draw(blue, new Rectangle((int)Grid[i, j].x * map.TileWidth, (int)Grid[i, j].y * map.TileHeight,
                                    tileWidth, tileHeight), new Rectangle(((int)timer % 16) * tileWidth, 0, tileWidth, tileHeight),
                                    Color.White * 0.75f);
                            else
                            spriteBatch.Draw(red, new Rectangle((int)Grid[i, j].x * map.TileWidth, (int)Grid[i, j].y * map.TileHeight,
                                tileWidth, tileHeight), new Rectangle(((int)timer % 16) * tileWidth, 0, tileWidth, tileHeight),
                                Color.White * 0.75f);
                        }
                    }
                }
            }
            spriteBatch.Draw(cursor, new Vector2((int)chosen.x*map.TileWidth, (int)chosen.y*map.TileHeight), Color.White * 0.75f);
            Ike.manager.Draw(gameTime, spriteBatch, new Vector2(Ike.x * map.TileWidth, Ike.y * map.TileHeight));
            spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }