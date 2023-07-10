using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Statics;
using Spextria.Graphics.GUI.Text;
using Spextria.Graphics.GUI.Interactions;
using Microsoft.Xna.Framework.Input;

namespace Spextria.Graphics.GUI
{
    class ButtonImage : IGuiButton

    {
        private ImagePanel OrigImage;
        private ImagePanel Image;
        private ImagePanel HoverImage;
        public Vector2 Position;
        private int Width;
        private int Height;
        public Rectangle ClickArea;
        private bool CurrentlyClicked;
        private bool CurrentlyHovered;
        private ButtonSignalEvent Signal;
        private bool ScreenFixed;

        public ButtonImage(ImagePanel image, ImagePanel hoverImage, Vector2 position, Vector2 size, ButtonSignalEvent signal, bool screenFixed = true)
        {
            Image = image;
            OrigImage = image;
            HoverImage = hoverImage;
            Signal = signal;
            CurrentlyClicked = false;
            ScreenFixed = screenFixed;
            Position = position;
            Width = (int)size.X;
            Height = (int)size.Y;
            ClickArea = new Rectangle((int)position.X-2, (int)position.Y-2, Width+4, Height+4);
            ScreenFixed = screenFixed;
        }

        

        public ButtonSignalEvent Update(Point mousePos, ButtonState leftClick, Point offset = new Point(), Vector2 graphicsPositionScale = new Vector2())
        {
            Rectangle OffsetClickArea = new Rectangle(ClickArea.X + offset.X, ClickArea.Y + offset.Y, ClickArea.Width, ClickArea.Height);
            OffsetClickArea.X = (int)(OffsetClickArea.X * graphicsPositionScale.X);
            OffsetClickArea.Y = (int)(OffsetClickArea.Y * graphicsPositionScale.Y);
            OffsetClickArea.Width = (int)(OffsetClickArea.Width * graphicsPositionScale.X);
            OffsetClickArea.Height = (int)(OffsetClickArea.Height * graphicsPositionScale.Y);
            if (OffsetClickArea.Contains(mousePos))
            {
                Image = HoverImage;

                if (leftClick == ButtonState.Pressed && CurrentlyHovered == true)
                {
                    CurrentlyClicked = true;
                }
                else if (leftClick != ButtonState.Pressed)
                    CurrentlyHovered = true;
                else
                    return new ButtonSignalEvent();


                if (Image != HoverImage)
                {
                    Image = HoverImage;
                }


                if (leftClick == ButtonState.Released && CurrentlyClicked)
                {
                    return Signal;
                }
            }
            else
            {
                CurrentlyHovered = false;
                CurrentlyClicked = false;
                if (Image != OrigImage)
                {
                    Image = OrigImage;
                }
            }
            return new ButtonSignalEvent();
        }

        public void Draw(SpriteBatch spriteBatch, CameraManager cameraManager, Vector2 offset = new Vector2())
        {
            Image.Draw(spriteBatch, cameraManager, offset);
        }
    }
}
