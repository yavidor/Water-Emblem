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
    public class PortraitDraw
    {
        private int x = 40;
        private int y = 34;
        private int OtherSide;
        private int xText = 128;
        private float Opacity = 0.75f;
        private SpriteBatch spriteBatch;
        private ContentManager Content;
        public PortraitDraw() { }
        public void Initialize(SpriteBatch spriteBatch, ContentManager Content)
        {
            this.spriteBatch = spriteBatch;
            this.Content = Content;
            this.OtherSide = (Game1.map.Height-3) * (Game1.tileWidth);
        }
        public void Draw(Unit unit, bool isActive)
        {
            if (isActive)
            {
                spriteBatch.Draw(this.Content.Load<Texture2D>("Background"),
                        new Vector2(0, 0), Color.White * Opacity);
                Game1.drawText.Write($"{unit.Stats["HP"]}/{unit.Stats["MaxHp"]}", x, y);
                this.spriteBatch.Draw(this.Content.Load<Texture2D>
                       ($"Sprites/Portraits/{unit.Name}{Convert.ToInt32(unit.Player)}")
                       , new Vector2(0, Game1.tileWidth), Color.White);
            }
            else
            {
                spriteBatch.Draw(this.Content.Load<Texture2D>("Background"),
                                       new Vector2(0, OtherSide), Color.White * Opacity);
                Game1.drawText.Write($"{unit.Stats["HP"]}/{unit.Stats["MaxHp"]}",
                    x, OtherSide+y);
                this.spriteBatch.Draw(this.Content.Load<Texture2D>
                       ($"Sprites/Portraits/{unit.Name}{Convert.ToInt32(unit.Player)}")
                       , new Vector2(0,OtherSide+Game1.tileWidth), Color.White);
            }
        }
    }
}
