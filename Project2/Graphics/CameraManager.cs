using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Spextria.Graphics
{
    class CameraManager
    {
        public OrthographicCamera Camera;
        public OrthographicCamera StaticCamera;
        public Vector2 CameraPosition;
        public Vector2 OldPosition;

        public CameraManager(GameWindow window, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsDeviceManager)
        {
            int windowHeight = graphicsDeviceManager.PreferredBackBufferHeight;
            int windowWidth = graphicsDeviceManager.PreferredBackBufferWidth;
            var viewportadapter = new BoxingViewportAdapter(window, graphicsDevice, windowWidth, windowHeight);
            Camera = new OrthographicCamera(viewportadapter);
            StaticCamera = new OrthographicCamera(viewportadapter);
            CameraPosition = new Vector2(0,300);
            OldPosition = CameraPosition;
        }

        private Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down))
            {
                movementDirection += Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                movementDirection -= Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                movementDirection += Vector2.UnitX;
            }

            // Can't normalize the zero vector so test for it before normalizing
            if (movementDirection != Vector2.Zero)
            {
                movementDirection.Normalize();
            }

            return movementDirection;
        }

        public Vector2 GetFixedScreenPos(int x, int y)
        {
            Vector2 camPos = new Vector2(Camera.Position.X, Camera.Position.Y);
            Vector2 newPosition = new Vector2(x + camPos.X / 9, y + camPos.Y / 9);
            return newPosition;
        }

        public Vector2 GetFixedScreenPosInverse(int x, int y)
        {
            Vector2 newPosition = new Vector2(x - Camera.Position.X / 9, y - Camera.Position.Y / 9);
            return newPosition;
        }

        public void MoveCamera(GameTime gameTime, Vector2 position)
        {
            var seconds = gameTime.GetElapsedSeconds();
            var movementDirection = GetMovementDirection();
            //CameraPosition += speed * movementDirection * seconds;
            CameraPosition = position;
        }
    }
}
