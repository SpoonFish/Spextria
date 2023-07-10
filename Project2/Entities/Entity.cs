using System;
using System.Collections.Generic;
using System.Text;
using Spextria.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spextria.Entities
{
    abstract class Entity
    {
        protected MyTexture Texture;
        public Vector2 Position;

        public virtual void Update()
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            Texture.Draw(spriteBatch, Position, opacity);
        }

        public void GoTo(Vector2 position)
        {
            Position = position;
        }
    }
}
