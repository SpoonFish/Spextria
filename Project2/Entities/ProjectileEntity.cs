using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Spextria.Graphics;
using Spextria.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Spextria.Entities
{
    class ProjectileEntity
    {
        private MyTexture Texture;
        private Vector2 Position;
        public bool PlayerOwned;
        public float Damage;
        public int Knockback;
        public int Pierce;
        public Rectangle ProjectileSize;
        public Rectangle CollidingRect;
        private Rectangle ExplosionArea;
        private bool Exploding;
        private bool Explodes;
        private float ExplosionDmg;
        private MyTexture ExplosionTexture;
        private string ParticleType;
        public string Element;

        public double HitTime;
        public bool Collides;
        public float Speed;
        public float Weight;
        private float TravelRange;
        private int Rounds;
        private double Innacuracy;
        private double DistanceTravelled;
        public double RefreshTime;
        public double WallDieTime;
        public double ExplodingTime;
        public double MaxWallDieTime;
        public bool WallDying;
        private double Rotation;
        private Vector2 Velocity;

        private Vector2 OldPos;

        public ProjectileEntity(bool playerOwned, float damage, int knockback, int pierce, double hitTime, bool collides, float speed, float weight, Rectangle projectileSize, MyTexture texture, Vector2 position, double angle, Vector2 initialVelocity, string particle, int rounds, double inaccuracy, int range, bool explodes, int explosionDmg, Rectangle explosionArea, MyTexture explosionTexture, string element)
        {
            Element = Element;
            DistanceTravelled = 0;
            TravelRange = range;
            Rounds = rounds;
            Innacuracy = inaccuracy;
            ParticleType = particle;
            ExplosionArea = explosionArea;
            ExplosionDmg = explosionDmg;
            Explodes = explodes;
            if (explosionTexture != null)
            {
                ExplosionTexture = Images.UniqueImage(explosionTexture);
                ExplosionTexture.CurrentFrame = 0;
            }
            OldPos = position;
            Position = position;
            WallDieTime = 0;
            Texture = Images.UniqueImage(texture);
            Texture.CurrentFrame = 0;
            Pierce = pierce;
            Exploding = false;
            PlayerOwned = playerOwned;
            ExplodingTime = 0;
            Damage = damage;
            Knockback = knockback;
            Weight = weight;
            RefreshTime = 0;
            HitTime = hitTime;
            Collides = collides;
            Speed = speed;
            Weight = weight;
            MaxWallDieTime = 0.4d * (1 / Speed);
            CollidingRect = new Rectangle(0, 0, Math.Min(projectileSize.Width, projectileSize.Height), Math.Min(projectileSize.Width, projectileSize.Height));
            ProjectileSize = projectileSize;
            Rotation = Functions.DegToRadians(angle) * -1 + 3.14;
            float xMult = (float)Math.Sin(Functions.DegToRadians(angle));
            float yMult = (float)Math.Cos(Functions.DegToRadians(angle));

            Velocity = new Vector2(xMult * Speed, yMult * Speed) + initialVelocity / 2;
        }

        public void Reflect(Vector2 addedVelocity = new Vector2())
        {
            Rotation -= Math.PI;
            PlayerOwned = !PlayerOwned;
            Velocity = -Velocity*0.65f + addedVelocity / 2;
            RefreshTime = 0f;
            OldPos = Position;
        }
        public void Update(EntityManager spriteManager, double timePassed, float dt, Rectangle[] collisions)
        {
            if (!Exploding)
            {
                RefreshTime += timePassed;

                Position += Velocity * dt;
                DistanceTravelled += Math.Sqrt(Velocity.X * Velocity.X + Velocity.Y * Velocity.Y);
                if (Texture.Frames > 1 && !WallDying)
                    Texture.CurrentFrame = Math.Min(Texture.Frames - 1, (int)(DistanceTravelled / TravelRange * Texture.Frames));
                else if (WallDying)
                    Texture.CurrentFrame = Texture.Frames - 1;

                if (RefreshTime > 0.15f)
                {
                    RefreshTime = 0;
                    Rotation = Functions.DegToRadians(Functions.Bearing(OldPos, Position)-180)*-1;
                    OldPos = Position;

                }
                ProjectileSize.Y = (int)Position.Y - ProjectileSize.Width / 2;
                ProjectileSize.X = (int)Position.X - ProjectileSize.Height / 2;

                if (Pierce >= 0)
                {
                    CollidingRect.Y = (int)Position.Y - CollidingRect.Width / 2;
                    CollidingRect.X = (int)Position.X - CollidingRect.Height / 2;

                }

                Velocity.Y += 0.08f * Weight * dt;
                if (Velocity.Y > 0)
                    Velocity.Y += 0.02f * Weight * dt;


                if (WallDieTime > MaxWallDieTime)
                {
                    spriteManager.ProjectilesArray.Remove(this);
                    return;
                }
                if (Collides)
                    foreach (Rectangle col in collisions)
                    {
                        if (col.Intersects(CollidingRect))
                        {
                            if (Explodes)
                            {
                                Damage = ExplosionDmg;
                                Exploding = true;
                                CollidingRect.X -= ExplosionArea.Width / 2 - CollidingRect.Width / 2;
                                CollidingRect.Y -= ExplosionArea.Height / 2 - CollidingRect.Height / 2;
                                CollidingRect.Width = ExplosionArea.Width;
                                CollidingRect.Height = ExplosionArea.Height;
                            }
                            else
                                WallDying = true;
                            break;
                        }
                    }

                if (Pierce < 0 || DistanceTravelled > TravelRange)
                    if (Explodes)
                    {
                        Damage = ExplosionDmg;
                        Exploding = true;
                        CollidingRect.X -= ExplosionArea.Width / 2 - CollidingRect.Width / 2;
                        CollidingRect.Y -= ExplosionArea.Height / 2 - CollidingRect.Height / 2;
                        CollidingRect.Width = ExplosionArea.Width;
                        CollidingRect.Height = ExplosionArea.Height;
                    }
                    else
                    {
                        CollidingRect.X = -100;
                        WallDying = true;
                    }
                if (WallDying)
                {
                    if (WallDieTime == 0)
                        Particles.CreateParticles(spriteManager, 10, Position, ParticleType, 1);
                    WallDieTime += timePassed;
                }
                Texture.Update(timePassed);
            }
            else
            {
                ExplodingTime += timePassed;
                ExplosionTexture.Update(timePassed);
                if (ExplodingTime > 0.5f)
                {
                    Particles.CreateParticles(spriteManager, 10, Position, ParticleType, 1);
                    spriteManager.ProjectilesArray.Remove(this);
                    return;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch, float mapOpacity = 1)
        {
            Rectangle rect = CollidingRect;
            //Images.ImageDict["button_red"].Draw(spriteBatch, new Vector2(rect.X, rect.Y), 0.5F, new Vector2(rect.Width, rect.Height));
            if (!Exploding)
                if (!WallDying)
                    Texture.Draw(spriteBatch, Position, mapOpacity, new Vector2(ProjectileSize.Height, ProjectileSize.Width), (float)Rotation);
                else
                    Texture.Draw(spriteBatch, Position, ((float)(WallDieTime / MaxWallDieTime) * -1f + 1)*mapOpacity, new Vector2(ProjectileSize.Height, ProjectileSize.Width), (float)Rotation);
            else
                ExplosionTexture.Draw(spriteBatch, new Vector2(Position.X - ExplosionArea.Width / 2, Position.Y - ExplosionArea.Height / 2), mapOpacity, new Vector2(ExplosionArea.Width, ExplosionArea.Height));
        }
    }
}
