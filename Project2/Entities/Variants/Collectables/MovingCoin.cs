using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spextria;
using Spextria.Entities;
using Spextria.Graphics;
using Spextria.Master;
using Spextria.Statics;
using MonoGame.Extended;

namespace Spextria.Entities.Variants
{
    class MovingCoin : CollectableEntity
    {
        private float LifeTime;
        private Vector2 Velocity;
        public MovingCoin(MyTexture texture, Vector2 position, float speed = 0, float thrust = 0, bool collides = true, float weight = 1) : base(texture, position, speed, thrust, collides)
        {
            Weight = weight;
            Rectangle full = new Rectangle((int)position.X, (int)position.Y, 16, 16);
            Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 1, 14, 14);
            Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, 16, 12);
            Rectangle upDown = new Rectangle((int)position.X + 2, (int)position.Y, 12, 16);
            Random random = new Random();
            float xDeviation = random.NextSingle(-1,1);
            float y = random.NextSingle(-1, -3);
            Hitbox = new HitboxObject(body, upDown, leftRight, full);
            Velocity = new Vector2(xDeviation, y);
        }


        public override void Update(HitboxObject playerHitbox, MasterManager master)
        {
            Hitbox.Update(Position);
            if (Hitbox.Body.Intersects(playerHitbox.Body))
            {
                master.entityManager.CollectablesArray.Remove(this);
                Particles.CreateParticles(master.entityManager, 5, new Vector2(Position.X + 8, Position.Y + 8), "sparkles", 1f, "");
                master.storedDataManager.CurrentSaveFile.Coins += 1;
                master.playGuiManager.ReloadCoinCounter(master.storedDataManager.CurrentSaveFile.Coins);
            }


            int attractionDistance = 0;
            float attractionSpeed = 1f;

            if (master.storedDataManager.CheckSkillUnlock("attraction_2"))
            {
                attractionDistance = 50;
                attractionSpeed = 1.5f;
            }
            else if (master.storedDataManager.CheckSkillUnlock("attraction_1"))
            {
                attractionDistance = 30;
            }

            Vector2 distance = Position - new Vector2(master.entityManager.Player.Hitbox.Body.Center.X, master.entityManager.Player.Hitbox.Body.Center.Y);
            if (master.storedDataManager.CheckSkillUnlock("attraction_1") && Math.Sqrt(distance.X * distance.X + distance.Y * distance.Y) < attractionDistance)
            {
                Velocity = -distance.NormalizedCopy();
                Position += Velocity * attractionSpeed * (float)master.dt;
            }
            else
            {
                if (LifeTime < 0.05f)
                    Position += Velocity * (float)master.dt;
                else
                {
                    bool move = true;
                    foreach (Rectangle col in master.mapManager.CurrentMap.Collisions)
                    {
                        if (col.Intersects(Hitbox.Body))
                            move = false;
                    }
                    if (move)
                        Position += Velocity * (float)master.dt;
                }
            }

            Velocity.Y += 0.1f * Weight * (float)master.dt;
            LifeTime += master.gameTime.GetElapsedSeconds();
        }
    }
}
