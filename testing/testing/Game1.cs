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
        GraphicsDeviceManager graphics; //Used to initialize and control the presentation of the graphics device.
        SpriteBatch SpriteBatch; //Helper class for drawing text strings and sprites
        SpriteFont SpriteFont; //Represents a font texture
        KeyboardState LastKey; //The last key pressed. Used to make the game annoying (:
        bool Pvp;
        enum GameStates { MODE, SELECT, MOVE, ACTION,VICTORY }; //The states of the game
        GameStates StateGame = GameStates.MODE; //The current state of the game
        TeamData Data; //The stats of the units
        Unit ActiveUnit, LeaderTeam0, LeaderTeam1; //The current unit, Ephraim and Eirika
        Attack Attack; //Attack to be executed
        Heal Heal; //Heal to be executed
        Move Move; //Move to be executed
        ComputerPlayer ComputerPlayer; //The computer player
        public static DrawText DrawText = new DrawText(); //To draw the HP in the portrait view
        DrawPortrait DrawPortrait = new DrawPortrait(); //To draw the portrait view
        public static TmxMap TmxMap { get; set; } //From TiledSharp
        public static Tile[,] Grid; //The board
        public static bool Turn = true; //Who's turn is it. True - Player 1, False - Player 2
        Map Map; //The map for drawing purpose
        Texture2D TileSet, Highlight, Cursor,Background;//The tileset, the possible moves highlighter, the cursor and the background
        bool WalkOrAttack = true; //Walking or attacking, for the BFS. True - Walking, False - Attacking
        double Timer = 0; //To draw the highlight
        public static int TileWidth; //32
        public static int TilesetTilesWide; //The amount of tiles in a row of the tileset
        public static List<Unit> Units = new List<Unit>(); //The units
        public static Tile Chosen; //Current tile chosen

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
            Background = Content.Load<Texture2D>("BG");
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
            foreach (UnitData unitData in Data.Team)
            {
                Units.Add(new Unit(unitData, Content, false)
                {

                    Tile = Grid[unitData.X, unitData.Y]
                });
                Unit current = Units.Last();
                Grid[unitData.X, unitData.Y].Unit = current;
                current.Manager.Play(current.Sprite);
                Units.Add(new Unit(unitData, Content,true)
                {
                    Tile = Grid[15-unitData.X, 20]
                });
                current = Units.Last();
                Grid[15-unitData.X, 20].Unit = current;
                current.Manager.Play(current.Sprite);
            }
            LeaderTeam0 = new Unit(Data.Ephraim, Content,false)
            {
                Tile = Grid[Data.Ephraim.X, Data.Ephraim.Y]
            };
            Grid[LeaderTeam0.X, LeaderTeam0.Y].Unit = LeaderTeam0;
            LeaderTeam0.Manager.Play(LeaderTeam0.Sprite);
            LeaderTeam1 = new Unit(Data.Eirika, Content,true)
            {
                Tile = Grid[Data.Eirika.X, Data.Eirika.Y]
            };
            Grid[LeaderTeam1.X, LeaderTeam1.Y].Unit = LeaderTeam1;
            LeaderTeam1.Manager.Play(LeaderTeam1.Sprite);
            Units.Add(LeaderTeam0);
            Units.Add(LeaderTeam1);
            Chosen = LeaderTeam1.Tile;
            Map = new Map(TmxMap, Grid, Units, TileSet);
            DrawText.Initialize(SpriteBatch, SpriteFont);
            DrawPortrait.Initialize(SpriteBatch, Content);
            ComputerPlayer = new ComputerPlayer();

        }
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
                if (StateGame == GameStates.MODE)
                {
                    Pvp = false;
                    StateGame = GameStates.SELECT;
                }
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
                    case GameStates.MODE:
                        Pvp = true;
                        StateGame = GameStates.SELECT;
                        break;
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
                            ActiveUnit.Manager.PauseOrPlay();
                            ActiveUnit = null;
                        }
                        StateGame = CheckAndDrawWinner();
                        if (StateGame == GameStates.SELECT)
                        {
                            if (Pvp)
                            {
                                Turn = !Turn;

                            }
                            else
                            {
                                this.Window.Title = "Computing...";
                                ComputerPlayer.MakeTurn(Map, 1);
                                StateGame = CheckAndDrawWinner();
                                if (StateGame == GameStates.VICTORY)
                                {
                                    break;
                                }
                            }

                            Chosen = TileNextTurn(Turn);
                        }
                        break;
                    default:
                        break;

                }
            }
            this.Window.Title = $"Water Emblem Player {(Turn?1:2)}'s Turn";
            LastKey = Keyboard.GetState();
            base.Update(gameTime);
        }
        private GameStates CheckAndDrawWinner()
        {
            bool Player1Alive = Units.Any(unit => unit.Player == true);
            bool Player2Alive = Units.Any(unit => unit.Player == false);
            if (Player1Alive && Player2Alive)
                return GameStates.SELECT;
            else
            {
                if (Player1Alive)
                    Background = Content.Load<Texture2D>("Player1Win");
                else
                    Background = Content.Load<Texture2D>("Player2Win"); 
            }
            return GameStates.VICTORY;
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
            if (StateGame == GameStates.MODE||StateGame==GameStates.VICTORY)
            {
                SpriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            }
            else
            {
                SpriteBatch.Draw(Content.Load<Texture2D>("Background"), new Vector2(0, 0), Color.White);
                Map.Draw(SpriteBatch);
                Timer += 0.1;
                if (ActiveUnit != null)
                {
                    if (Chosen.Unit != ActiveUnit && Chosen.Unit != null)
                    {
                        DrawPortrait.Draw(Chosen.Unit, false);
                    }
                    DrawPortrait.Draw(ActiveUnit, true);
                    Rectangle source = new Rectangle((int)Timer
                                                % 16 * TileWidth,
                                                0, TileWidth, TileWidth);
                    List<Tile> ls = ActiveUnit.ReachableTiles(Grid, WalkOrAttack);
                    foreach (Tile tile in ls)
                    {
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

                SpriteBatch.Draw(Cursor, new Vector2((int)Chosen.X * TmxMap.TileWidth,
                    Chosen.Y * TmxMap.TileHeight), Color.White * 0.75f);
                foreach (Unit unit in Units)
                {
                    unit.Manager.Draw(gameTime, SpriteBatch,
                        new Vector2(unit.X * TmxMap.TileWidth, unit.Y * TmxMap.TileHeight));
                }
            }
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}