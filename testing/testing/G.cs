using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    static class G
    {
        #region Names
        public static GraphicsDeviceManager graphicsDeviceManager;
        public static GraphicsDevice graphicsDevice;
        public static ContentManager contentManager;
        public static SpriteBatch spriteBatch;
        public static KeyboardState keyBoardState;
        #endregion
        #region Initialize
        public static void Init(GraphicsDeviceManager graphicsDeviceManager,
            GraphicsDevice graphicsDevice, ContentManager contentManager,
            SpriteBatch spriteBatch, KeyboardState keyboardState)
        {
            G.graphicsDeviceManager = graphicsDeviceManager;
            G.graphicsDevice = graphicsDevice;
            G.contentManager = contentManager;
            G.spriteBatch = spriteBatch;
            G.keyBoardState = keyboardState;
        }
        #endregion

    }
}
