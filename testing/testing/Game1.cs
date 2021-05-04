using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
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
        UnitData udE;
        Texture2D IkeTexture;
        Unit Ike, ActiveUnit, Eirika;
        Attack attack;
        Move move;
        GameStates State = GameStates.SELECT;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static TmxMap map { get; set; }
        internal static Tile[,] Grid { get => grid; set => grid = value; }
        Map Map;
        Painter painter;
        bool draw = false;
        Texture2D tileset, blue, cursor,red;
        bool flag = false, WalkOrAttack = true;
        public List<Unit> units = new List<Unit>();
        double timer = 0;
        private static Tile[,] grid;
        Tile chosen;
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
            ud = Content.Load<UnitData>("Epharim");

            udE = Content.Load<UnitData>("Eirika");
            Map = new Map();
            font = Content.Load<SpriteFont>("font");
            cursor = Content.Load<Texture2D>("chosen");
            blue = Content.Load<Texture2D>("blue");
            red = Content.Load<Texture2D>("red");
            map = new TmxMap("Content/balanced.tmx");
            tileset = Content.Load<Texture2D>(map.Tilesets[0].Name.ToString());
            Grid = new Tile[map.Width, map.Height];
            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;
            graphics.PreferredBackBufferHeight = map.Height * map.TileHeight;
            graphics.PreferredBackBufferWidth = map.Width * map.TileWidth;
            graphics.ApplyChanges();
            tilesetTilesWide = tileset.Width / tileWidth;
            tilesetTilesHigh = tileset.Height / tileHeight;
            for (int i = 0; i < map.Layers[0].Tiles.Count; i++)
            {
                Tile Tile = new Tile(map.Layers[0].Tiles[i].Gid - 1, i);
                Grid[Tile.x, Tile.y] = Tile;
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
                Tile = Grid[10, 20]
            };
            Eirika = new Unit(udE, Content)
            {
                Tile = Grid[5, 20]
            };
            units.Add(Ike);
            units.Add(Eirika);
            Grid[10, 20].Unit = Ike;
            Ike.Manager.Play(Ike.Sprite);
            Eirika.Manager.Play(Eirika.Sprite);
            Grid[5, 20].Unit = Eirika;
            Console.WriteLine(Grid[10, 20].Unit);
            Map.Initialize(this, map,Grid,units);
            
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && lastkey.IsKeyUp(Keys.Space))
            {
                flag = !flag;
            }
            #region Directions
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && lastkey.IsKeyUp(Keys.Right) &&
                ((int)chosen.x) < map.Width - 1)
            {
                chosen = Grid[((int)chosen.x) + 1, ((int)chosen.y)];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && lastkey.IsKeyUp(Keys.Left) &&
                ((int)chosen.x) > 0)
            {
                chosen = Grid[((int)chosen.x) - 1, ((int)chosen.y)];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && lastkey.IsKeyUp(Keys.Up) &&
                ((int)chosen.y) > 0)
            {
                chosen = Grid[((int)chosen.x), ((int)chosen.y) - 1];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && lastkey.IsKeyUp(Keys.Down) &&
                ((int)chosen.y) < map.Height - 1)  
            {
                chosen = Grid[((int)chosen.x), ((int)chosen.y) + 1];              
            }
            #endregion
            if (Keyboard.GetState().IsKeyDown(Keys.Z) && lastkey.IsKeyUp(Keys.Z))
            {
                switch (State)
                {
                    case GameStates.SELECT:
                        if (chosen.Unit != null)
                        {
                            WalkOrAttack = true;
                            draw = false;
                            State = GameStates.MOVE;
                            ActiveUnit = chosen.Unit;
                            ActiveUnit.Manager.Animation = ActiveUnit.Sprite;
                            Console.WriteLine(String.Concat(ActiveUnit.GetActions(Map).Select(o => o.ToString() + "\n")));
                        }
                        break;
                    case GameStates.MOVE:
                        if (chosen.Unit == null && ActiveUnit.ReachableTiles(Map, true).Contains(chosen))
                        {
                            State = GameStates.ACTION;
                            move = new Move(ActiveUnit, chosen, false);
                            //ActiveUnit.Tile.Unit = null;
                            //ActiveUnit.Tile = chosen;
                            // chosen.Unit = ActiveUnit;
                            move.Execute();

                            WalkOrAttack = false;
                        }
                        break;
                    case GameStates.ACTION:
                        if (chosen.Unit != null && ActiveUnit.ReachableTiles(Map, false).Contains(chosen))
                        {
                            State = GameStates.SELECT;
                            Console.WriteLine(chosen.Unit.Stats["HP"]);
                            attack = new Attack(ActiveUnit, chosen.Unit, false);
                            attack.Execute();
                            Console.WriteLine(chosen.Unit.Stats["HP"]);
                            Console.WriteLine(Ike.Tile.Unit);
                        }
                        else { 
                        State = GameStates.SELECT;
                        ActiveUnit = null;
                        }
                        break;
                    case GameStates.TARGET:
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
            if (Ike.Tile.Unit == null) 
            Console.WriteLine(Ike.Tile.Unit==null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            Map.Draw(spriteBatch);
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
                        if (ActiveUnit != null)
                        {
                            List<Tile> ls = ActiveUnit.ReachableTiles(Map,WalkOrAttack);
                            if (flag && ls.Contains(Grid[i, j]))
                            {
                                if (State == GameStates.SELECT)
                                    spriteBatch.Draw(blue, new Rectangle((int)Grid[i, j].x * map.TileWidth,
                                        (int)Grid[i, j].y * map.TileHeight,
                                        tileWidth, tileHeight), new Rectangle(((int)timer % 16) * tileWidth,
                                        0, tileWidth, tileHeight),
                                        Color.White * 0.75f);
                                else
                                    spriteBatch.Draw(red, new Rectangle((int)Grid[i, j].x * map.TileWidth,
                                        (int)Grid[i, j].y * map.TileHeight,
                                        tileWidth, tileHeight), new Rectangle(((int)timer % 16) * tileWidth,
                                        0, tileWidth, tileHeight),
                                        Color.White * 0.75f);
                            }
                        }
                    }
                }
            }
            if (draw == true) {
                spriteBatch.Draw(Content.Load<Texture2D>("highlite"), new Vector2(Ike.x * Map.TmxMap.TileWidth+32, Ike.y * Map.TmxMap.TileHeight));
            }
            spriteBatch.Draw(cursor, new Vector2((int)chosen.x*map.TileWidth,
                (int)chosen.y*map.TileHeight), Color.White * 0.75f);
            Ike.Manager.Draw(gameTime, spriteBatch, new Vector2(Ike.x * map.TileWidth,
                Ike.y * map.TileHeight));
            Eirika.Manager.Draw(gameTime, spriteBatch, new Vector2(Eirika.x * map.TileWidth, Eirika.y * map.TileHeight));
            spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }