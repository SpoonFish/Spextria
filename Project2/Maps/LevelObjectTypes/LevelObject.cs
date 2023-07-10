using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.Master;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Maps.LevelObjectTypes
{
    abstract class LevelObject
    {
        protected Vector2 Position;
        protected Rectangle Area;

        public virtual void Update(MasterManager master)
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
