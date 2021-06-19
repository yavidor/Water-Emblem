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
        #region Data
        private const int X = 40; //Where to draw the portrait
        private const int Y = 34; //Where to draw the portrait
        private const int xText = 128; //Where to draw the text
        private const float Opacity = 0.75f; //How clear the portrait view will be
        private int OtherSide; //Where to draw the other portrait view
        private SpriteBatch spriteBatch; //Helper class for drawing text strings and sprites
        private ContentManager Content; //Content Manager
        #endregion
        #region CTOR
        /// <summary>
        /// CTOR. Empty
        /// </summary>
        public DrawPortrait() { }
        #endregion
        #region Functions
        /// <summary>
        /// The actual constructor to be honest.
        /// </summary>
        /// <param name="spriteBatch">To initialize the variable with the same name</param>
        /// <param name="Content">To initialize the variable with the same name</param>
        public void Initialize(SpriteBatch spriteBatch, ContentManager Content)
        {
            this.spriteBatch = spriteBatch;
            this.Content = Content;
            this.OtherSide = (Game1.TmxMap.Height-3) * (Game1.TileWidth);
        }
        /// <summary>
        /// Drawing the portrait view
        /// </summary>
        /// <param name="unit">The unit to draw the portrait of</param>
        /// <param name="isActive">True - the main portrait, False - the secondary portrait</param>
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
        #endregion
    }
}
