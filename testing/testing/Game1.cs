﻿using Microsoft.Xna.Framework;
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
        enum OnlineState
        {
            AskingRole, //host or join
            Connecting,
            Playing
        }
        OnlineGame onlineGame;
        OnlineState state = OnlineState.AskingRole;
        bool host; //True = The host. False = The second player
        public enum GameStates {SELECT,MOVE,ACTION,TARGET};
        GameStates State = GameStates.SELECT;
        bool Pvp = true;//True = Player vs Player. False = Player vs Computer
        TeamData Data;
        Unit ActiveUnit;
        Unit LeaderTeam0, LeaderTeam1;
        Attack attack;
        Heal heal;
        Move move;
        MiniMax AI;
        SpriteFont spriteFont;
        public static DrawText drawText = new DrawText();
        PortraitDraw portraitDraw = new PortraitDraw();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static TmxMap map { get; set; }
        internal static Tile[,] Grid { get => grid; set => grid = value; }
        Map Map = new Map();
        private static Tile[,] grid;
        Texture2D tileset, Highlight, Cursor;
        bool WalkOrAttack = true, Turn = true; 
        public static List<Unit> Units = new List<Unit>();
        double timer = 0;
        int damage;
        Tile Chosen;
        KeyboardState lastkey;
        public static int tileWidth;
        public static int tilesetTilesWide;

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
            Cursor = Content.Load<Texture2D>("Chosen");
            Highlight = Content.Load<Texture2D>("Highlight");
            spriteFont = Content.Load<SpriteFont>("font");
            map = new TmxMap("Content/balanced.tmx");
            tileset = Content.Load<Texture2D>(map.Tilesets[0].Name.ToString());
            Grid = new Tile[map.Width, map.Height];
            tileWidth = map.Tilesets[0].TileWidth;
            graphics.PreferredBackBufferHeight = map.Height * map.TileHeight;
            graphics.PreferredBackBufferWidth = map.Width * map.TileWidth;
            graphics.ApplyChanges();
            tilesetTilesWide = tileset.Width / tileWidth;
            for (int i = 0; i < map.Layers[0].Tiles.Count; i++)
            {
                Tile Tile = new Tile(map.Layers[0].Tiles[i].Gid - 1, i);
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
                    Tile = grid[unitData.x, unitData.y]
                });
                Unit current = Units.Last();
                grid[unitData.x, unitData.y].Unit = current;
                current.Manager.Play(current.Sprite);
            }
            LeaderTeam0 = Units.Find(unit => unit.Name == "Epharim");
            LeaderTeam1 = Units.Find(unit => unit.Name == "Eirika");
            Chosen = LeaderTeam1.Tile;
            Map.Initialize(map,Grid,Units,Content);
            drawText.Initialize(spriteBatch, spriteFont);
            portraitDraw.Initialize(spriteBatch, Content);
            AI = new MiniMax();
            
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
            switch (state)
            {
                case OnlineState.AskingRole:
                    if (Keyboard.GetState().IsKeyDown(Keys.H))
                    {
                        onlineGame = new Host(int.Parse(File.ReadAllText("port.txt")));
                        onlineGame.OnConnection += onlineGame_OnConnection;
                        //onlineGame.StartCommunication();
                        host = true;

                        state = OnlineState.Connecting;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.J))
                    {
                        onlineGame = new JoinOnlineGame(File.ReadAllText("ip.txt"), int.Parse(File.ReadAllText("port.txt")));
                        onlineGame.OnConnection += onlineGame_OnConnection;
                        // onlineGame.StartCommunication();
                        host = false;
                        state = OnlineState.Connecting;
                    }
                    break;

                case OnlineState.Connecting:

                    break;

                case OnlineState.Playing:
                    #region Directions
                    if (Keyboard.GetState().IsKeyDown(Keys.Right) && lastkey.IsKeyUp(Keys.Right) &&
                        ((int)Chosen.X) < map.Width - 1)
                    {
                        Chosen = Grid[((int)Chosen.X) + 1, ((int)Chosen.Y)];
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Left) && lastkey.IsKeyUp(Keys.Left) &&
                        ((int)Chosen.X) > 0)
                    {
                        Chosen = Grid[((int)Chosen.X) - 1, ((int)Chosen.Y)];
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) && lastkey.IsKeyUp(Keys.Up) &&
                        ((int)Chosen.Y) > 0)
                    {
                        Chosen = Grid[((int)Chosen.X), ((int)Chosen.Y) - 1];
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) && lastkey.IsKeyUp(Keys.Down) &&
                        ((int)Chosen.Y) < map.Height - 1)
                    {
                        Chosen = Grid[((int)Chosen.X), ((int)Chosen.Y) + 1];
                    }
                    #endregion
                    if (Keyboard.GetState().IsKeyDown(Keys.X) && lastkey.IsKeyUp(Keys.X))
                    {
                        if (State == GameStates.MOVE)
                        {
                            ActiveUnit.Manager.PauseOrPlay();
                            ActiveUnit = null;
                            State = GameStates.SELECT;
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Z) && lastkey.IsKeyUp(Keys.Z))
                    {
                        switch (State)
                        {
                            case GameStates.SELECT:
                                if (Chosen.Unit != null && Chosen.Unit.Player == Turn)
                                {
                                    damage = 0;
                                    WalkOrAttack = true;
                                    State = GameStates.MOVE;
                                    ActiveUnit = Chosen.Unit;
                                    ActiveUnit.Manager.PauseOrPlay();
                                }
                                break;
                            case GameStates.MOVE:
                                if (Chosen.Unit == null && ActiveUnit.ReachableTiles(Grid, true)
                                    .Contains(Chosen))
                                {
                                    State = GameStates.ACTION;
                                    move = new Move(ActiveUnit, Chosen, false);
                                    move.Execute();

                                    WalkOrAttack = false;
                                }
                                break;
                            case GameStates.ACTION:
                                if (Chosen.Unit != null && ActiveUnit.ReachableTiles(Grid, false)
                                    .Contains(Chosen))
                                {
                                    State = GameStates.SELECT;
                                    Console.WriteLine(Chosen.Unit.Stats["HP"]);
                                    heal = new Heal(ActiveUnit, Chosen.Unit, false);
                                    if (ActiveUnit.GetActions(Grid).Any(
                                        action => action.Heal != null && action.Heal.Target.Player && action.Heal.GetType() == heal.GetType() &&
                                        action.Heal.Equals(heal)))
                                    {
                                        Console.WriteLine(heal.GetType());
                                        damage = heal.Execute();
                                    }
                                    attack = new Attack(ActiveUnit, Chosen.Unit, false);

                                    if (ActiveUnit.GetActions(Grid).Any(
                                        action => action.Attack != null && action.Attack.GetType() == attack.GetType() &&
                                        action.Attack.Equals(attack)))
                                    {
                                        damage = attack.Execute();
                                        Console.WriteLine("Hello");
                                    }
                                    ActiveUnit.Manager.PauseOrPlay();

                                }
                                else
                                {
                                    State = GameStates.SELECT;
                                    ActiveUnit.Manager.PauseOrPlay();
                                    ActiveUnit = null;
                                }
                                if (Pvp)
                                {
                                    if (Turn)
                                    {
                                        Chosen = LeaderTeam0.Tile;
                                    }
                                    else
                                    {
                                        Chosen = LeaderTeam1.Tile;
                                    }
                                    Turn = !Turn;
                                }
                                else
                                {
                                    Console.WriteLine(AI.MakeTurn(Map, 1).ToString());

                                    Chosen = LeaderTeam1.Tile;
                                }
                                break;
                            default:
                                break;

                        }
                    }
                    break;
            }
         
            lastkey = Keyboard.GetState();
            base.Update(gameTime);
        }
        void onlineGame_OnConnection()
        {
            state = OnlineState.Playing;
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
                            if (Chosen.Unit != ActiveUnit && Chosen.Unit != null)
                            {
                                portraitDraw.Draw(Chosen.Unit, false);
                            }
                            portraitDraw.Draw(ActiveUnit, true);
                            Rectangle source = new Rectangle((int)timer % 16 * tileWidth,
                                0, tileWidth, tileWidth);
                            List<Tile> ls = ActiveUnit.ReachableTiles(Grid, WalkOrAttack);
                            if (ls.Contains(Grid[i, j]) && (State == GameStates.MOVE ||
                                State == GameStates.ACTION))
                            {
                                if (State == GameStates.ACTION)
                                {
                                    source.Y = tileWidth;
                                }
                                    spriteBatch.Draw(Highlight,
                                        new Rectangle((int)Grid[i, j].X * map.TileWidth,
                                        (int)Grid[i, j].Y * map.TileHeight,
                                        tileWidth, tileWidth),
                                       source,
                                        Color.White * 0.75f);
                                }
                            }
                        }
                    }
                }
            spriteBatch.Draw(Cursor, new Vector2((int)Chosen.X*map.TileWidth,
                (int)Chosen.Y*map.TileHeight), Color.White * 0.75f);
            foreach (Unit unit in Units)
            {
                unit.Manager.Draw(gameTime, spriteBatch,
                    new Vector2(unit.X * map.TileWidth, unit.Y * map.TileHeight));
            }
            spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }