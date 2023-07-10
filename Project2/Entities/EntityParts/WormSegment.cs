using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.Graphics.GUI.PlayScreen;
using Spextria.Master;
using Spextria.Statics;
using System;
using System.Collections.Generic;
using System.Text;


namespace Spextria.Entities.EntityParts
{
    class WormSegment
    {
        private double _RefreshTime;
        public Rectangle Hitbox;
        public MyTexture Texture;
        private float ConstraintLength;
        public Vector2 Position;
        public Vector2 OldPos;
        public Vector2 FollowPosition;
        public double Rotation;

        public WormSegment(Rectangle hitbox, MyTexture texture, Vector2 position)
        {
            OldPos = new Vector2();
            _RefreshTime = 0;
            Hitbox = hitbox;
            ConstraintLength = 2;
            Texture = texture;
            Position = position;
            Rotation = 0;
        }

        public void Update(MasterManager master)
        {

            _RefreshTime += master.timePassed;

            double refreshAdd  =0;
            float distance = (float)Math.Sqrt(Math.Pow(Position.Y - FollowPosition.Y, 2) + Math.Pow(Position.X - FollowPosition.X,2));
            if (distance < 1)
                refreshAdd = 0.06;
            if (_RefreshTime > 0.06 + refreshAdd)
            {
                _RefreshTime = 0;
                Rotation = Functions.Bearing(OldPos, Position);
                OldPos = Position;

            }

            if (distance > ConstraintLength)
            {
                OldPos = Position;
                Position += (FollowPosition - Position) / 5f;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            
            Texture.Draw(spriteBatch, Position + new Vector2(16, 16), opacity, new Vector2(Texture.GetHeight(), Texture.GetWidth()), -(float)Functions.DegToRadians(Rotation - 180));
            
        }
    }
}
