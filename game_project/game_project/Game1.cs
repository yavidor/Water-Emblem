using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game_project
{
    enum State { ask, connecting, playing };
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        State state;
        Online online;
        public static string msg = " ";
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            state = State.ask;

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case State.ask:
                    if (Keyboard.GetState().IsKeyDown(Keys.H))
                    {
                        msg = "H";
                        Window.Title = msg;
                        state = State.connecting;
                        online = new Host(666);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.J))
                    {
                        state = State.connecting;
                        Window.Title = "fuck";
                        online = new Join(666, "127.0.0.1");
                    }
                    break;
                case State.connecting:
                    Window.Title = msg;
                    break;
                case State.playing:
                    break;
                default:
                    break;


            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
