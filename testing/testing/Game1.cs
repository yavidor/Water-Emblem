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
        bool Pvp = true;//True = Player vs Player. False = Player vs Computer
        enum OnlineStates
        {
            AskingRole, //host or join
            Connecting,
            Playing
        }

        OnlineStates StateOnline = OnlineStates.AskingRole;
        OnlineGame onlineGame;
        enum GameStates {SELECT,MOVE,ACTION};
        bool host;
        GameStates StateGame = GameStates.SELECT;
        TeamData Data;
        Unit ActiveUnit, LeaderTeam0, LeaderTeam1;
        Attack Attack;
        Heal Heal;
        Move Move;
        ComputerPlayer ComputerPlayer;
        public static DrawText DrawText = new DrawText();
        PortraitDraw PortraitDraw = new PortraitDraw();
        public static TmxMap TmxMap  { get; set; }
        public static Tile[,] Grid;
        public static bool Turn = true;
        Map Map = new Map();
        Texture2D tileset, Highlight, Cursor;
        bool WalkOrAttack = true;
        public static List<Unit> Units = new List<Unit>();
        double Timer = 0;
        public static Tile Chosen;
        public static int TileWidth;
        public static int TilesetTilesWide;

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
            tileset = Content.Load<Texture2D>(TmxMap.Tilesets[0].Name.ToString());
            Grid = new Tile[TmxMap.Width, TmxMap.Height];
            TileWidth = TmxMap.Tilesets[0].TileWidth;
            graphics.PreferredBackBufferHeight = TmxMap.Height * TmxMap.TileHeight;
            graphics.PreferredBackBufferWidth = TmxMap.Width * TmxMap.TileWidth;
            graphics.ApplyChanges();
            TilesetTilesWide = tileset.Width / TileWidth;
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
                    Tile = Grid[unitData.x, unitData.y]
                });
                Unit current = Units.Last();
                Grid[unitData.x, unitData.y].Unit = current;
                current.Manager.Play(current.Sprite);
            }
            LeaderTeam0 = Units.Find(unit => unit.Name == "Ephraim");
            LeaderTeam1 = Units.Find(unit => unit.Name == "Eirika");
            Chosen = LeaderTeam1.Tile;
            Map.Initialize(TmxMap,Grid,Units,Content);
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
            switch (StateOnline)
            {
                case OnlineStates.AskingRole:
                    if (Keyboard.GetState().IsKeyDown(Keys.H))
                    {
                        onlineGame = new Host(int.Parse(File.ReadAllText("port.txt")));
                        onlineGame.OnConnection += onlineGame_OnConnection;
                        host = true;

                        StateOnline = OnlineStates.Connecting;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.J))
                    {
                        onlineGame = new Join(File.ReadAllText("ip.txt"),
                            int.Parse(File.ReadAllText("port.txt")));
                        onlineGame.OnConnection += onlineGame_OnConnection;
                        host = false;
                        StateOnline = OnlineStates.Connecting;
                    }
                    break;

                case OnlineStates.Connecting:

                    break;

                case OnlineStates.Playing:
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
                    if (Keyboard.GetState().IsKeyDown(Keys.Z) && LastKey.IsKeyUp(Keys.Z) && host==Turn)
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
                                        Console.WriteLine(Heal.GetType());
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
                                    if (Turn)
                                    {
                                        Chosen = LeaderTeam1.Tile;
                                    }
                                    else
                                    {
                                        Chosen = LeaderTeam0.Tile;
                                    }
                                    onlineGame.Stop();
                                    onlineGame.WriteCharacterData(Units);
                                }
                                else
                                {
                                    Console.WriteLine(ComputerPlayer.MakeTurn(Map, 1).ToString());

                                    Chosen = LeaderTeam1.Tile;
                                }
                                break;
                            default:
                                break;

                        }
                    }
                    break;
            }

            this.Window.Title = $"Turn: {Turn} Hpst: {host}";
            LastKey = Keyboard.GetState();
            base.Update(gameTime);
        }
        void onlineGame_OnConnection()
        {
            StateOnline = OnlineStates.Playing;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); 
            SpriteBatch.Begin();
            Map.Draw(SpriteBatch);
            Timer += 0.1;
           
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                   
                    
                      
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
                            if (ls.Contains(Grid[i, j]) && (StateGame == GameStates.MOVE ||
                                StateGame == GameStates.ACTION))
                            {
                                if (StateGame == GameStates.ACTION)
                                {
                                    source.Y = TileWidth;
                                }
                                    SpriteBatch.Draw(Highlight,
                                        new Rectangle((int)Grid[i, j].X * TmxMap.TileWidth,
                                        (int)Grid[i, j].Y * TmxMap.TileHeight,
                                        TileWidth, TileWidth),
                                       source,
                                        Color.White * 0.75f);
                                }
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