using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.Master;

namespace Spextria.Entities
{
    abstract class CollectableEntity : Entity
    {
        protected float Speed;
        protected float Weight;
        protected bool Collides;
        protected HitboxObject Hitbox;

        public CollectableEntity(MyTexture texture, Vector2 position, float speed, float weight, bool collides = true)
        {
            Texture = texture;
            Position = position;
            Speed = speed;
            Weight = weight;
            Collides = collides;
        }

        public void Debug(SpriteBatch spriteBatch)
        {
            Hitbox.Debug(spriteBatch, Position);
        }
        public virtual void Update(HitboxObject playerHitbox, MasterManager master)
        {
        }
    }
}
