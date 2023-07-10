using System;
using System.Collections.Generic;
using System.Text;
using Spextria.Graphics.GUI.Interactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Spextria.Graphics.GUI
{
    interface IGuiButton
    {
        public void Draw(SpriteBatch spriteBatch, CameraManager cameraManager, Vector2 offset = new Vector2())
        {
        }

        public ButtonSignalEvent Update(Point mousePos, ButtonState leftClick, Point offset = new Point(), Vector2 graphicsPositionScale = new Vector2())
        {
            return new ButtonSignalEvent();
        }
    }
}
