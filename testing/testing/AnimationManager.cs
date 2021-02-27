using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    struct AnimationManager
    {
        public Animation Animation
        {
            get { return animation; }
        }
        Animation animation;
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;
        private float time;
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, animation.FrameWidth);}
        }
        public void Play(Animation animation) {

            if (Animation == animation)
            {
                return;
            }
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
        }
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
            Rectangle source = new Rectangle(FrameIndex * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);
            spriteBatch.Draw(Animation.Texture, position,source, Color.White);

        }
    }
}
