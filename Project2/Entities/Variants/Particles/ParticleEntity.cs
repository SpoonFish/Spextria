using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using System;

namespace Spextria.Entities
{
    class ParticleSprite : Entity
    {
        protected int Collides; //0 no, 1 yes (kinda inside ground), 2 yes (on ground)s
        protected float Speed;
        protected float Weight;
        protected int Rotation;
        protected Vector2 PosOffset;
        protected Vector2 CollisionPointOffset;
        protected float LifeTime;
        protected float LifeMax;
        protected Vector2 Velocity;
        protected float FadeIntensity;

        public ParticleSprite(MyTexture texture, Vector2 position, Vector2 velocity, float speed, float weight, float lifetime, float fadeintensity = 0, int collides = 0)
        {
            Collides = collides;
            Speed = speed;
            Texture = texture;
            PosOffset = new Vector2(Texture.GetWidth() / 2, Texture.GetHeight() / 2);
            CollisionPointOffset = new Vector2(PosOffset.Y, PosOffset.X);
            if (Collides == 2)
                CollisionPointOffset.Y += Texture.GetHeight(); //lowers collision point
            LifeMax = lifetime;
            Position = position;
            Weight = weight;
            FadeIntensity = fadeintensity;
            LifeTime = 0;

            Velocity = velocity;
        }

        public virtual void Update(EntityManager spriteManager, double timePassed, float dt, Rectangle[] collisions)
        {
            if (Collides == 0 || (LifeTime < 0.15f && Collides == 1) || (LifeTime < 0.02f && Collides == 2))
                Position += Velocity * dt;
            else
            {
                bool move = true;
                foreach (Rectangle col in collisions)
                {
                    if (col.Contains(Position + CollisionPointOffset))
                        move = false;
                }
                if (move)
                    Position += Velocity * dt;

            }


            Velocity.Y += 0.1f * Weight * dt;

            LifeTime += (float)timePassed;
            if (LifeTime > LifeMax)
                spriteManager.ParticlesArray.Remove(this);
            Texture.Update(timePassed);
        }

        public override void Draw(SpriteBatch spriteBatch, float mapOpacity = 1)
        {
            
            if (FadeIntensity > 0)
            {
                float opacity = 1f - LifeTime / LifeMax;
                Texture.Draw(spriteBatch, Position + PosOffset, opacity * mapOpacity);
            }
            else
            {
                Texture.Draw(spriteBatch, Position + PosOffset);
            }

        }
    }
}
