using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
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
        GraphicsDeviceManager graphics;
        SpriteBatch SpriteBatch;
        SpriteFont SpriteFont;
        KeyboardState LastKey;
        enum GameStates {SELECT,MOVE,ACTION};
        bool Pvp = false;
        GameStates StateGame = GameStates.SELECT;
        TeamData Data;
        Unit ActiveUnit, LeaderTeam0, LeaderTeam1;
        Attack Attack;
        Heal Heal;
        Move Move;
        ComputerPlayer ComputerPlayer;
        public static DrawText DrawText = new DrawText();
        DrawPortrait PortraitDraw = new DrawPortrait();
        public static TmxMap TmxMap  { get; set; }
        public static Tile[,] Grid;
        public static bool Turn = true;
        Map Map;
        Texture2D TileSet, Highlight, Cursor;
        bool WalkOrAttack = true;
        double Timer = 0;
        public static int TileWidth;
        public static int TilesetTilesWide;
        public static List<Unit> Units = new List<Unit>();
        public static Tile Chosen;

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
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Data = Content.Load<TeamData>("Data");
            Cursor = Content.Load<Texture2D>("Chosen");
            Highlight = Content.Load<Texture2D>("Highlight");
            SpriteFont = Content.Load<SpriteFont>("font");
            TmxMap = new TmxMap("Content/balanced.tmx");
            TileSet = Content.Load<Texture2D>(TmxMap.Tilesets[0].Name.ToString());
            Grid = new Tile[TmxMap.Width, TmxMap.Height];
            TileWidth = TmxMap.Tilesets[0].TileWidth;
            graphics.PreferredBackBufferHeight = TmxMap.Height * TmxMap.TileHeight;
            graphics.PreferredBackBufferWidth = TmxMap.Width * TmxMap.TileWidth;
            graphics.ApplyChanges();
            TilesetTilesWide = TileSet.Width / TileWidth;
            for (int i = 0; i < TmxMap.Layers[0].Tiles.Count; i++)
            {
                Tile Tile = new Tile(TmxMap.Layers[0].Tiles[i].Gid - 1, i);
                Grid[Tile.X, Tile.Y] = Tile;
            }
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    Grid[i, j].AddNeighbors();
                }
            }
            foreach(UnitData unitData in Data.Team)
            {
                Units.Add(new Unit(unitData, Content)
                {
                    Tile = Grid[unitData.X, unitData.Y]
                });
                Unit current = Units.Last();
                Grid[unitData.X, unitData.Y].Unit = current;
                current.Manager.Play(current.Sprite);
            }
            LeaderTeam0 = Units.Find(unit => unit.Name == "Ephraim");
            LeaderTeam1 = Units.Find(unit => unit.Name == "Eirika");
            Chosen = LeaderTeam1.Tile;
            Map = new Map(TmxMap,Grid,Units,TileSet);
            DrawText.Initialize(SpriteBatch, SpriteFont);
            PortraitDraw.Initialize(SpriteBatch, Content);
            ComputerPlayer = new ComputerPlayer();
            
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
                    if (Keyboard.GetState().IsKeyDown(Keys.Right) && LastKey.IsKeyUp(Keys.Right) &&
                        ((int)Chosen.X) < TmxMap.Width - 1)
                    {
                        Chosen = Grid[((int)Chosen.X) + 1, ((int)Chosen.Y)];
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Left) && LastKey.IsKeyUp(Keys.Left) &&
                        ((int)Chosen.X) > 0)
                    {
                        Chosen = Grid[((int)Chosen.X) - 1, ((int)Chosen.Y)];
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) && LastKey.IsKeyUp(Keys.Up) &&
                        ((int)Chosen.Y) > 0)
                    {
                        Chosen = Grid[((int)Chosen.X), ((int)Chosen.Y) - 1];
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) && LastKey.IsKeyUp(Keys.Down) &&
                        ((int)Chosen.Y) < TmxMap.Height - 1)
                    {
                        Chosen = Grid[((int)Chosen.X), ((int)Chosen.Y) + 1];
                    }
                    #endregion
                    if (Keyboard.GetState().IsKeyDown(Keys.X) && LastKey.IsKeyUp(Keys.X))
                    {
                        if (StateGame == GameStates.MOVE)
                        {
                            ActiveUnit.Manager.PauseOrPlay();
                            ActiveUnit = null;
                            StateGame = GameStates.SELECT;
                        }
                    }
            if (Keyboard.GetState().IsKeyDown(Keys.Z) && LastKey.IsKeyUp(Keys.Z))
            {
                switch (StateGame)
                {
                    case GameStates.SELECT:
                        if (Chosen.Unit != null && Chosen.Unit.Player == Turn)
                        {
                            WalkOrAttack = true;
                            StateGame = GameStates.MOVE;
                            ActiveUnit = Chosen.Unit;
                            ActiveUnit.Manager.PauseOrPlay();
                        }
                        break;
                    case GameStates.MOVE:
                        if (Chosen.Unit == null && ActiveUnit.ReachableTiles(Grid, true)
                            .Contains(Chosen))
                        {
                            StateGame = GameStates.ACTION;
                            Move = new Move(ActiveUnit, Chosen, false);
                            Move.Execute();

                            WalkOrAttack = false;
                        }
                        break;
                    case GameStates.ACTION:
                        if (Chosen.Unit != null && ActiveUnit.ReachableTiles(Grid, false)
                            .Contains(Chosen))
                        {
                            StateGame = GameStates.SELECT;
                            Heal = new Heal(ActiveUnit, Chosen.Unit, false);
                            if (ActiveUnit.GetActions(Grid).Any(
                                action => action.Heal != null &&
                                action.Heal.Target.Player &&
                                action.Heal.GetType() == Heal.GetType() &&
                                action.Heal.Equals(Heal)))
                            {
                                Heal.Execute();
                            }
                            Attack = new Attack(ActiveUnit, Chosen.Unit, false);

                            if (ActiveUnit.GetActions(Grid).Any(
                                action => action.Attack != null &&
                                action.Attack.GetType() == Attack.GetType() &&
                                action.Attack.Equals(Attack)))
                            {
                                Attack.Execute();
                            }
                            ActiveUnit.Manager.PauseOrPlay();

                        }
                        else
                        {
                            StateGame = GameStates.SELECT;
                            ActiveUnit.Manager.PauseOrPlay();
                            ActiveUnit = null;
                        }
                        if (Pvp)
                        {
                            Turn = !Turn;

                        }
                        else
                        {
                        ComputerPlayer.MakeTurn(Map, 1);
                        }
                        Chosen = TileNextTurn(Turn);
                        break;
                    default:
                        break;

                }
            }
            this.Window.Title = $"Turn: {Turn}";
            LastKey = Keyboard.GetState();
            base.Update(gameTime);
        }
        /// <summary>
        /// Where the Chosen tile should go
        /// </summary>
        /// <param name="Player">The current player's turn</param>
        /// <returns>A tile for the Chosen</returns>
       private Tile TileNextTurn(bool Player)
        {
            if (Player)
            {
                if (LeaderTeam1.Stats["HP"] > 0)
                    return LeaderTeam1.Tile;
                else
                   return Units.Find(unit => unit.Player == true).Tile;
            }
            else
            {
                if (LeaderTeam0.Stats["HP"] > 0)
                    return LeaderTeam0.Tile;
            }
            return Units.Find(unit => unit.Player == false).Tile;//Always have one return outside conditions
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); 
            SpriteBatch.Begin();
            SpriteBatch.Draw(Content.Load<Texture2D>("Background"), new Vector2(0, 0), Color.White);
            Map.Draw(SpriteBatch);
            Timer += 0.1;
            if (ActiveUnit != null)
            {
                if (Chosen.Unit != ActiveUnit && Chosen.Unit != null)
                {
                    PortraitDraw.Draw(Chosen.Unit, false);
                }
                PortraitDraw.Draw(ActiveUnit, true);
                Rectangle source = new Rectangle((int)Timer
                                            % 16 * TileWidth,
                                            0, TileWidth, TileWidth);
                List<Tile> ls = ActiveUnit.ReachableTiles(Grid, WalkOrAttack);
                foreach (Tile tile in ls) { 
                if (StateGame == GameStates.MOVE ||
                    StateGame == GameStates.ACTION)
                {
                    if (StateGame == GameStates.ACTION)
                    {
                        source.Y = TileWidth;
                    }
                    SpriteBatch.Draw(Highlight,
                        new Rectangle((int)tile.X * TmxMap.TileWidth,
                        (int)tile.Y * TmxMap.TileHeight,
                        TileWidth, TileWidth),
                       source,
                        Color.White * 0.75f);
                }
            }
            }
                
            SpriteBatch.Draw(Cursor, new Vector2((int)Chosen.X*TmxMap.TileWidth,
                Chosen.Y * TmxMap.TileHeight), Color.White * 0.75f);
            foreach (Unit unit in Units)
            {
                unit.Manager.Draw(gameTime, SpriteBatch,
                    new Vector2(unit.X * TmxMap.TileWidth, unit.Y * TmxMap.TileHeight));
            }
            SpriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }