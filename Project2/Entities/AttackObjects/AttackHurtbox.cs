using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Entities.AttackObjects
{
    class AttackHurtbox
    {
        public Rectangle[] Rectangles;
        private Rectangle[] OrigRectangles;
        // Maybe add a single target optin

        public AttackHurtbox(Rectangle[] rectangles)
        {
            List<Rectangle> rects =  new List<Rectangle>();
            OrigRectangles = rectangles;
            for (int i = 0; i < OrigRectangles.Length; i++)
            {
                Rectangle rect = OrigRectangles[i];
                rects.Add(new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
            }

            Rectangles = rects.ToArray();
        }

        public bool CollidesWith(Rectangle rect)
        {
            for (int i = 0; i < Rectangles.Length; i++)
            {
                if (Rectangles[i].Intersects(rect))
                    return true;

            }
            return false;
        }

        public void SetDirection(string direction)
        {
            if (direction == "right")
            {
                for (int i = 0; i < OrigRectangles.Length; i++)
                {
                    Rectangle rect = OrigRectangles[i];
                    Rectangles[i] = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
            else
            {

                for (int i = 0; i < OrigRectangles.Length; i++)
                {
                    Rectangle rect = OrigRectangles[i];
                    Rectangles[i] = new Rectangle(rect.X * -1 - rect.Width, rect.Y, rect.Width, rect.Height);
                }
            }
        }
    }
}
