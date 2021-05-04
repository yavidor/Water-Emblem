using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace testing
{
    public class Animation
    {
        #region Data
        /// <summary>
        /// The source of the animation
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;
        /// <summary>
        /// Does this animation loop
        /// </summary>
        public bool Repeating
        {
            get { return repeating; }
            set { }
        }
        bool repeating;
        /// <summary>
        /// How long does each frame stay for before moving to the next one
        /// </summary>
        public float TimePerFrame
        {
            get { return timePerFrame; }
        }
        float timePerFrame;
        /// <summary>
        /// How many frames in the animation
        /// </summary>
        public int count
        {
            get { return texture.Width / frameWidth; }
        }
        /// <summary>
        /// The width of each frame in the animation
        /// </summary>
        public int FrameWidth
        {
            get { return frameWidth; }
        }
        int frameWidth;
        #endregion
        #region CTOR
        public Animation(Texture2D texture, bool repeating, float timePerFrame, int frameWidth)
        {
            this.texture = texture;
            this.repeating = repeating;
            this.timePerFrame = timePerFrame;
            this.frameWidth = frameWidth;
        }
        public Animation(string texture, string repeating, string timePerFrame, string frameWidth,ContentManager content)
        {
            this.texture = content.Load<Texture2D>(texture);
            this.repeating = bool.Parse(repeating);
            this.timePerFrame = float.Parse(timePerFrame);
            this.frameWidth = int.Parse(frameWidth);


        }
        #endregion
    }
}
