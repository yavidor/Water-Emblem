using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        Unit typo;
        Texture2D t2;
        public enum GameStates {SELECT,MOVE,ACTION,TARGET};
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static TmxMap map { get; set; }
        Texture2D tileset, highlight, cursor;
        bool flag = false;
        double timer = 0;
        Spot[,] grid;
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
            font = Content.Load<SpriteFont>("font");
            cursor = Content.Load<Texture2D>("chosen");
            highlight = Content.Load<Texture2D>("blue");
            map = new TmxMap("Content/balanced.tmx");
            tileset = Content.Load<Texture2D>(map.Tilesets[0].Name.ToString());
            Console.WriteLine(tileset);
            grid = new Spot[map.Width, map.Height];
            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;
            graphics.PreferredBackBufferHeight = map.Height * map.TileHeight;
            graphics.PreferredBackBufferWidth = map.Width * map.TileWidth;
            graphics.ApplyChanges();
            tilesetTilesWide = tileset.Width / tileWidth;
            tilesetTilesHigh = tileset.Height / tileHeight;
            for (var i = 0; i < map.Layers[0].Tiles.Count; i++)
            {
                Spot spot = new Spot(map.Layers[0].Tiles[i].Gid - 1, i);
                grid[(int)spot.x, (int)spot.y] = spot;
            }
            chosen = grid[0, 0];
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
                flag = !flag;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && lastkey.IsKeyUp(Keys.Right) && ((int)chosen.x) < map.Width-1)
                chosen = grid[((int)chosen.x)+1, ((int)chosen.y)];
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && lastkey.IsKeyUp(Keys.Left) && ((int)chosen.x) >0)
                chosen = grid[((int)chosen.x) - 1, ((int)chosen.y)];
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && lastkey.IsKeyUp(Keys.Up) && ((int)chosen.y) > 0)
                chosen = grid[((int)chosen.x) , ((int)chosen.y) - 1];
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && lastkey.IsKeyUp(Keys.Down) && ((int)chosen.y) < map.Height - 1)
            {
                chosen = grid[((int)chosen.x), ((int)chosen.y) + 1];
                Console.WriteLine(grid[(int)chosen.x, (int)chosen.y].toString());

              
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
           
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                { 
                    if (grid[i, j].frame == 1)
                    {

                        Console.WriteLine("Oh oh");
                    }
                    else
                    {
                        
                        spriteBatch.Draw(tileset, new Rectangle((int)grid[i, j].x*map.TileWidth, (int)grid[i, j].y*map.TileHeight, tileWidth, tileHeight), grid[i, j].rec, Color.White);
                        if (grid[i, j].walkable && flag)
                          // spriteBatch.Draw(high, new Vector2(spot.x, spot.y), Color.White * 0.2f);
                            spriteBatch.Draw(highlight, new Rectangle((int)grid[i, j].x*map.TileWidth, (int)grid[i, j].y*map.TileHeight, tileWidth, tileHeight), new Rectangle(((int)timer % 16) * tileWidth, 0, tileWidth, tileHeight), Color.White * 0.75f);
                    }
                }
            }
            spriteBatch.Draw(t2, new Vector2((int)chosen.x * map.TileWidth, (int)chosen.y * map.TileHeight), Color.White);
            spriteBatch.Draw(cursor, new Vector2((int)chosen.x*map.TileWidth, (int)chosen.y*map.TileHeight), Color.White * 0.75f);
            
            spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }