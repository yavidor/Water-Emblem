using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
   public struct AnimationManager
    {
        /// <summary>
        /// The animation itself
        /// </summary>
        public Animation Animation
        {
            get { return animation; }
            set { }
        }
        Animation animation;
        /// <summary>
        /// Which frame is playing right now
        /// </summary>
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;
        /// <summary>
        /// Time since last frame change
        /// </summary>
        private float time;
        /// <summary>
        /// Origin of the rectangle that represents the current frame
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, animation.FrameWidth);}
        }
/// <summary>
/// Start playing the animation
/// </summary>
/// <param name="animation">The animation to play</param>
        public void Play(Animation animation) {

            if (Animation == animation)
            {
                return;
            }
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
        }
        /// <summary>
        /// Draw the current frame
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="spriteBatch">The thing that draws things</param>
        /// <param name="position">Where to draw the frame</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position) {
            if (Animation == null)
                throw new NotSupportedException("There is no animation playing.");
            time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while (time > Animation.TimePerFrame)
            {
                time -= Animation.TimePerFrame;
                if (Animation.Repeating)
                {
                    frameIndex = (frameIndex + 1) % Animation.count;
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, Animation.count - 1);
                }
            }
            Rectangle source = new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.Texture.Height);
            spriteBatch.Draw(Animation.Texture, position,source, Color.White);

        }
    }
}
