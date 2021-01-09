using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    class Painter
    {
        Texture2D texture;
        Vector2 position;
        Rectangle? sourceRectangle;
        Color color;
        float rotation;
        Vector2 origin;
        Vector2 scale;
        SpriteEffects effects;
        float layerDepth;
        public Painter(Texture2D texture, Vector2 position, Rectangle? sourceRectangle,
            Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects,
            float layerDepth)
        {
            this.texture = texture;
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effects = effects;
            this.layerDepth = layerDepth;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, this.sourceRectangle,
                this.color, this.rotation, this.origin, this.scale, this.effects, this.layerDepth);
        }
    }
}
