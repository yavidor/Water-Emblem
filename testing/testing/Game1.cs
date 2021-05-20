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
        Texture2D tileset, blue, cursor,red;
        bool WalkOrAttack = true;
        public List<Unit> units = new List<Unit>();
        double timer = 0;
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
            Data = Content.Load<TeamData>("Data");
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
                            State = GameStates.MOVE;
                            ActiveUnit = chosen.Unit;
                            ActiveUnit.Manager.PauseOrPlay();
                            Console.WriteLine(String.Concat(ActiveUnit.GetActions(Map).Select(o => o.ToString() + "\n")));
                        }
                        break;
                    case GameStates.MOVE:
                        if (chosen.Unit == null && ActiveUnit.ReachableTiles(Map, true).Contains(chosen))
                        {
                            State = GameStates.ACTION;
                            move = new Move(ActiveUnit, chosen, false);
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
                            if (ls.Contains(Grid[i, j]))
                            {
                                if (State == GameStates.MOVE)
                                    spriteBatch.Draw(blue, new Rectangle((int)Grid[i, j].x * map.TileWidth,
                                        (int)Grid[i, j].y * map.TileHeight,
                                        tileWidth, tileHeight), new Rectangle(((int)timer % 16) * tileWidth,
                                        0, tileWidth, tileHeight),
                                        Color.White * 0.75f);
                                if (State == GameStates.ACTION)
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
            spriteBatch.Draw(cursor, new Vector2((int)chosen.x*map.TileWidth,
                (int)chosen.y*map.TileHeight), Color.White * 0.75f);
            foreach(Unit unit in units)
            {
                unit.Manager.Draw(gameTime, spriteBatch, new Vector2(unit.x * map.TileWidth, unit.y * map.TileHeight));
            }
 
            spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }