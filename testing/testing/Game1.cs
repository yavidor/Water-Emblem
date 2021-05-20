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
        TeamData Data;
        Unit ActiveUnit;
        Attack attack;
        Move move;
        GameStates State = GameStates.SELECT;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static TmxMap map { get; set; }
        internal static Tile[,] Grid { get => grid; set => grid = value; }
        Map Map;
        private static Tile[,] grid;
        Texture2D tileset, Highlight, Cursor;
        bool WalkOrAttack = true;
        public List<Unit> units = new List<Unit>();
        double timer = 0;
        Tile Chosen;
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
            Data = Content.Load<TeamData>("Data");
            Map = new Map();
            font = Content.Load<SpriteFont>("Font");
            Cursor = Content.Load<Texture2D>("Chosen");
            Highlight = Content.Load<Texture2D>("Highlight");
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
            Chosen = Grid[10, 19];
            foreach(UnitData unitData in Data.Team)
            {
                units.Add(new Unit(unitData, Content)
                {
                    Tile = grid[unitData.x, unitData.y]
                });
                Unit current = units.Last();
                grid[unitData.x, unitData.y].Unit = current;
                current.Manager.Play(current.Sprite);
            }
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
            #region Directions
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && lastkey.IsKeyUp(Keys.Right) &&
                ((int)Chosen.x) < map.Width - 1)
            {
                Chosen = Grid[((int)Chosen.x) + 1, ((int)Chosen.y)];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && lastkey.IsKeyUp(Keys.Left) &&
                ((int)Chosen.x) > 0)
            {
                Chosen = Grid[((int)Chosen.x) - 1, ((int)Chosen.y)];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && lastkey.IsKeyUp(Keys.Up) &&
                ((int)Chosen.y) > 0)
            {
                Chosen = Grid[((int)Chosen.x), ((int)Chosen.y) - 1];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && lastkey.IsKeyUp(Keys.Down) &&
                ((int)Chosen.y) < map.Height - 1)  
            {
                Chosen = Grid[((int)Chosen.x), ((int)Chosen.y) + 1];              
            }
            #endregion
            if (Keyboard.GetState().IsKeyDown(Keys.X) && lastkey.IsKeyUp(Keys.X))
            {
                if (State == GameStates.MOVE)
                {
                    ActiveUnit = null;
                    ActiveUnit.Manager.PauseOrPlay();
                    State = GameStates.SELECT;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z) && lastkey.IsKeyUp(Keys.Z))
            {
                switch (State)
                {
                    case GameStates.SELECT:
                        if (Chosen.Unit != null)
                        {
                            WalkOrAttack = true;
                            State = GameStates.MOVE;
                            ActiveUnit = Chosen.Unit;
                            ActiveUnit.Manager.PauseOrPlay();
                        }
                        break;
                    case GameStates.MOVE:
                        if (Chosen.Unit == null && ActiveUnit.ReachableTiles(Map, true)
                            .Contains(Chosen))
                        {
                            State = GameStates.ACTION;
                            move = new Move(ActiveUnit, Chosen, false);
                            move.Execute();

                            WalkOrAttack = false;
                        }
                        break;
                    case GameStates.ACTION:
                        if (Chosen.Unit != null && ActiveUnit.ReachableTiles(Map, false)
                            .Contains(Chosen))
                        {
                            State = GameStates.SELECT;
                            Console.WriteLine(Chosen.Unit.Stats["HP"]);
                            attack = new Attack(ActiveUnit, Chosen.Unit, false);
                            attack.Execute();
                            Console.WriteLine(Chosen.Unit.Stats["HP"]);
                            ActiveUnit.Manager.PauseOrPlay();
                        }
                        else { 
                        State = GameStates.SELECT;
                        ActiveUnit.Manager.PauseOrPlay();
                        ActiveUnit = null;
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
            Map.Draw(spriteBatch);
            timer += 0.1;
            if (ActiveUnit != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>
                    ($"Sprites/Portraits/{ActiveUnit.Name}{Convert.ToInt32(ActiveUnit.Player)}")
                    , new Vector2(0, 0), Color.White);
            }
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
                            Rectangle source = new Rectangle((int)timer % 16 * tileWidth,
                                0, tileWidth, tileHeight);
                            List<Tile> ls = ActiveUnit.ReachableTiles(Map, WalkOrAttack);
                            if (ls.Contains(Grid[i, j]) && (State == GameStates.MOVE ||
                                State == GameStates.ACTION))
                            {
                                if (State == GameStates.ACTION)
                                {
                                    source.Y = tileHeight;
                                }
                                    spriteBatch.Draw(Highlight,
                                        new Rectangle((int)Grid[i, j].x * map.TileWidth,
                                        (int)Grid[i, j].y * map.TileHeight,
                                        tileWidth, tileHeight),
                                       source,
                                        Color.White * 0.75f);
                                }
                            }
                        }
                    }
                }
            spriteBatch.Draw(Cursor, new Vector2((int)Chosen.x*map.TileWidth,
                (int)Chosen.y*map.TileHeight), Color.White * 0.75f);
            foreach(Unit unit in units)
            {
                unit.Manager.Draw(gameTime, spriteBatch,
                    new Vector2(unit.x * map.TileWidth, unit.y * map.TileHeight));
            }
 
            spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }