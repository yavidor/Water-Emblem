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
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        public DrawText() { }
        public void Initialize(SpriteBatch spriteBatch,SpriteFont spriteFont)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
        }
        public void Write(string text, int x, int y)
        {
            spriteBatch.DrawString(spriteFont, text, new Vector2(x, y),
                Color.Black);
        }
    }
}
