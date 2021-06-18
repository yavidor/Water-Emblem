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
    public class DrawPortrait
    {
        private const int X = 40;
        private const int Y = 34;
        private const int xText = 128;
        private const float Opacity = 0.75f;
        private int OtherSide;
        private SpriteBatch spriteBatch;
        private ContentManager Content;
        public DrawPortrait() { }
        public void Initialize(SpriteBatch spriteBatch, ContentManager Content)
        {
            this.spriteBatch = spriteBatch;
            this.Content = Content;
            this.OtherSide = (Game1.TmxMap.Height-3) * (Game1.TileWidth);
        }
        public void Draw(Unit unit, bool isActive)
        {
            if (isActive)
            {
                spriteBatch.Draw(this.Content.Load<Texture2D>("Background"),
                        new Vector2(0, 0), Color.White * Opacity);
                Game1.DrawText.Write($"{unit.Stats["HP"]}/{unit.Stats["MaxHp"]}", X, Y);
                this.spriteBatch.Draw(this.Content.Load<Texture2D>
                       ($"Sprites/Portraits/{unit.Name}{Convert.ToInt32(unit.Player)}")
                       , new Vector2(0, Game1.TileWidth), Color.White);
            }
            else
            {
                spriteBatch.Draw(this.Content.Load<Texture2D>("Background"),
                                       new Vector2(0, OtherSide), Color.White * Opacity);
                Game1.DrawText.Write($"{unit.Stats["HP"]}/{unit.Stats["MaxHp"]}",
                    X, OtherSide+Y);
                this.spriteBatch.Draw(this.Content.Load<Texture2D>
                       ($"Sprites/Portraits/{unit.Name}{Convert.ToInt32(unit.Player)}")
                       , new Vector2(0,OtherSide+Game1.TileWidth), Color.White);
            }
        }
    }
}
