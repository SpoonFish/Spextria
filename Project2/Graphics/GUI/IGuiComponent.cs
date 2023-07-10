using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Spextria.Graphics.GUI
{
    interface IGuiComponent
    {
        public void Draw(SpriteBatch spriteBatch, CameraManager cameraManager, Vector2 offset = new Vector2(), float opacity = 1)
        {
        }
    }
}
