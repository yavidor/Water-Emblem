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
        public Animation Animation;
        /// <summary>
        /// Which frame is playing right now
        /// </summary>
        public int FrameIndex;
        /// <summary>
        /// Time since last frame change
        /// </summary>
        private float time;
        /// <summary>
        /// Origin of the rectangle that represents the current frame
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth,Animation.Texture.Height);}
        }
        public bool Playing;
        /// <summary>
        /// Start playing the animation
        /// </summary>
        /// <param name="animation">The animation to play</param>
        public void Play(Animation animation) {

            if (Animation == animation)
            {
                return;
            }
            this.Animation = animation;
            this.FrameIndex = 0;
            this.time = 0.0f;
        }
        public void PauseOrPlay()
        {
            this.Playing = !this.Playing;
        }
        /// <summary>
        /// Draw the current frame
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="spriteBatch">The thing that draws things</param>
        /// <param name="position">Where to draw the frame</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 pos) {
            if (Animation == null)
                throw new NotSupportedException("There is no animation playing.");
            if (Playing)
            {
                time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                while (time > Animation.TimePerFrame)
                {
                    time -= Animation.TimePerFrame;
                        FrameIndex = (FrameIndex + 1) % Animation.Count;
                }
            }
            Rectangle source = new Rectangle(FrameIndex * Animation.FrameWidth,
                0, Animation.FrameWidth, Animation.Texture.Height);
            spriteBatch.Draw(Animation.Texture,new Vector2(pos.X+Game1.TileWidth,pos.Y+Game1.TileWidth)
                ,source,Color.White,0.0f,Origin,1.0f,SpriteEffects.None,1.0f);

        }
    }
}
