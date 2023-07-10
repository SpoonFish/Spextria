using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Entities;
using Spextria.Graphics;
using Spextria.Master;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Maps.LevelObjectTypes
{
    class AnimatedObject : LevelObject
    {
        private MyTexture Texture;
        public AnimatedObject(Vector2 position, Rectangle area, MyTexture texture)
        {
            Position = position;
            Area = area;
            Texture = texture;
        }

        public override void Update(MasterManager master)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture.Draw(spriteBatch, Position, 1, new Vector2(Area.Width, Area.Height));
        }
    }
}
