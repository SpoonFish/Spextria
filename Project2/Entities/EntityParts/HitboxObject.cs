using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Spextria;
using Spextria.Graphics;
using Spextria.Statics;

namespace Spextria.Entities
{
    class HitboxObject
    {
        public Rectangle Feet;
        public Rectangle Full;
        public float Width;
        public float Height;
        public Rectangle Body;
        public Rectangle UpDown;
        public Rectangle LeftRight;
        private MyTexture Texture;
        public Point Offset;

        public HitboxObject(Rectangle body, Rectangle updown, Rectangle leftright, Rectangle full, Point offset = new Point())
        {
            Full = full;
            Width = full.Width;
            Height = full.Height;
            Feet = new Rectangle(0, 0, Body.Width, 6);
            Body = body;
            UpDown = updown;
            LeftRight = leftright;
            Offset = offset;
            Texture = Images.ImageDict["button_red"];
        }

        public void Update(Vector2 position, Size size = new Size(), int deltaHeight = 0, int deltaHeightBody = 0)
        {
            //Size size = new Vector2(spriteSize.GetWidth(), spriteSize.GetHeight());
            int x = (int)position.X + Offset.X;
            int y = (int)position.Y + Offset.Y;


            Full.X = x;
            Full.Y = y - deltaHeight;

            Body.X = x + 1;
            Body.Y = y + 1 + -deltaHeightBody;

            Feet.X = x + 1;
            Feet.Y = y + Body.Height-4;

            LeftRight.X = x;
            LeftRight.Y = y + 2 + -deltaHeight;

            UpDown.X = x + 4;
            UpDown.Y = y + -deltaHeight;

            if (size == Size.Empty)
                return;
            int width = size.Width;
            int height = size.Height;

            Full.Height = height + deltaHeight;
            Full.Width = width;

            Body.Width = width - 2;
            Body.Height = height - 2 + deltaHeightBody;

            LeftRight.Width = width;
            LeftRight.Height = height - 4 + deltaHeight;

            UpDown.Width = width - 8;
            UpDown.Height = height + deltaHeight;
        }

        public void Debug(SpriteBatch spriteBatch, Vector2 position)
        {
            Texture.Draw(spriteBatch, position, 1, new Vector2(Body.Width, Body.Height));
        }
    }
}
