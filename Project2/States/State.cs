using System;
using System.Collections.Generic;
using System.Text;
using Spextria.Master;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spextria.States
{
    abstract class State
    {
        public abstract void Update(MasterManager master, GameTime gameTime, SpextriaGame game, GraphicsDevice graphicsDevice);

        public abstract void Draw(MasterManager master, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);
    }
}
