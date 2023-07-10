using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.Statics;

namespace Spextria.Entities
{
    abstract class MovingEntity : Entity
    {
        protected float Speed;
        protected float Thrust;
        protected float OrigThrust; 
        protected bool Collides;
        protected DataTypes.Directions Facing;

        public MovingEntity(MyTexture texture, Vector2 position, float speed, float thrust = 2, bool collides = true)
        {
            Speed = speed;
            Texture = texture;
            Position = position;
            Thrust = thrust;
            OrigThrust = thrust;
            Collides = collides;
        }

        public virtual void Update(Rectangle[] collision)
        {
        }
    }
}
