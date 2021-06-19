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
    public class Animation
    {
        #region Data
        /// <summary>
        /// The source of the animation
        /// </summary>
        public Texture2D Texture;
        /// <summary>
        /// How long does each frame stay for before moving to the next one
        /// </summary>
        public float TimePerFrame;
        /// <summary>
        /// How many frames in the animation
        /// </summary>
        public const int Count = 4;
        /// <summary>
        /// The width of each frame in the animation
        /// </summary>
        public int FrameWidth;
        #endregion
        #region CTOR
        public Animation(Texture2D texture, float timePerFrame, int frameWidth)
        {
            this.Texture = texture;
            this.TimePerFrame = timePerFrame;
            this.FrameWidth = frameWidth;
        }
        #endregion
    }
}
