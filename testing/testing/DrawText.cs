using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public class DrawText
    {
        #region Data
        private SpriteBatch spriteBatch; //Helper class for drawing text strings and sprites
        private SpriteFont spriteFont; //The font
        #endregion
        #region CTOR
        /// <summary>
        /// CTOR. Empty
        /// </summary>
        public DrawText() { }
        #endregion
        #region Function
        /// <summary>
        /// The actual constructor to be honest
        /// </summary>
        /// <param name="spriteBatch">To initialize the variable with the same name</param>
        /// <param name="spriteFont">To initialize the variable with the same name</param>
        public void Initialize(SpriteBatch spriteBatch,SpriteFont spriteFont)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
        }
        /// <summary>
        /// Drawing the text
        /// </summary>
        /// <param name="text">What to draw</param>
        /// <param name="X">The X value of the text's position</param>
        /// <param name="Y">The Y value of the text's position</param>
        public void Write(string text, int X, int Y)
        {
            spriteBatch.DrawString(spriteFont, text, new Vector2(X, Y),
                Color.Black);
        }
        #endregion
    }
}
