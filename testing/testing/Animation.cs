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
    [Serializable]
    public class Animation
    {
        #region Data
        /// <summary>
        /// The source of the animation
        /// </summary>
        public Texture2D Texture => texture;

        private Texture2D texture;
        /// <summary>
        /// How long does each frame stay for before moving to the next one
        /// </summary>
        public float TimePerFrame => timePerFrame;

        private float timePerFrame;
        /// <summary>
        /// How many frames in the animation
        /// </summary>
        public int Count => texture.Width / frameWidth;
        /// <summary>
        /// The width of each frame in the animation
        /// </summary>
        public int FrameWidth => frameWidth;

        private int frameWidth;
        #endregion
        #region CTOR
        public Animation(Texture2D texture, float timePerFrame, int frameWidth)
        {
            this.texture = texture;
            this.timePerFrame = timePerFrame;
            this.frameWidth = frameWidth;
        }
        #endregion
    }
}
