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
    class Soul : CollectableEntity
    {
        private float LifeTime;
        private Vector2 Velocity;
        public Soul(MyTexture texture, Vector2 position, float speed = 0, float thrust = 0, bool collides = true, float weight = 0) : base(texture, position, speed, thrust, collides)
        {
            Rectangle full = new Rectangle((int)position.X, (int)position.Y, 16, 16);
            Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 1, 14, 14);
            Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, 16, 12);
            Rectangle upDown = new Rectangle((int)position.X + 2, (int)position.Y, 12, 16);
            Random random = new Random();
            float x = random.NextSingle(-2, 2);
            float y = random.NextSingle(-2, 2);
            Hitbox = new HitboxObject(body, upDown, leftRight, full);
            Velocity = new Vector2(x, y);
        }


        public override void Update(HitboxObject playerHitbox, MasterManager master)
        {
            Hitbox.Update(Position);
            Particles.CreateParticles(master.entityManager, 1, new Vector2(Position.X + 6, Position.Y + 6), "soul collected", 1f, master.entityManager.Player.CurrentPlanet);
            if (Hitbox.Body.Intersects(playerHitbox.Body))
            {
                Particles.CreateParticles(master.entityManager, 15, new Vector2(Position.X + 6, Position.Y + 6), "soul collected", 1f, master.entityManager.Player.CurrentPlanet);
                switch (master.entityManager.Player.CurrentPlanet)
                {
                    case "luxiar":
                        master.storedDataManager.CurrentSaveFile.LightSouls += 1;
                        master.playGuiManager.ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.LightSouls);
                        break;
                    case "gramen":
                        master.storedDataManager.CurrentSaveFile.GrowthSouls += 1;
                        master.playGuiManager.ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.GrowthSouls);
                        break;
                    case "freone":
                        master.storedDataManager.CurrentSaveFile.FrostSouls += 1;
                        master.playGuiManager.ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.FrostSouls);
                        break;
                    case "umbrac":
                        master.storedDataManager.CurrentSaveFile.ShadowSouls += 1;
                        master.playGuiManager.ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.ShadowSouls);
                        break;
                    case "inferni":
                        master.storedDataManager.CurrentSaveFile.FlameSouls += 1;
                        master.playGuiManager.ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.FlameSouls);
                        break;
                    case "ossium":
                        master.storedDataManager.CurrentSaveFile.TormentSouls += 1;
                        master.playGuiManager.ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.TormentSouls);
                        break;
                }
                master.entityManager.CollectablesArray.Remove(this);
            }
            int attractionDistance = 46;
            float attractionSpeed = 1f;

            if (master.storedDataManager.CheckSkillUnlock("attraction_2"))
            {
                attractionDistance = 80;
                attractionSpeed = 2f;
            }
            else if (master.storedDataManager.CheckSkillUnlock("attraction_1"))
            {
                attractionDistance = 60;
                attractionSpeed = 1.5f;
            }

            Vector2 distance = Position - new Vector2(master.entityManager.Player.Hitbox.Body.Center.X, master.entityManager.Player.Hitbox.Body.Center.Y);
            if (Math.Sqrt(distance.X * distance.X + distance.Y * distance.Y) < attractionDistance)
            {
                Velocity = -distance.NormalizedCopy();
                Position += Velocity * attractionSpeed * (float)master.dt;
            }
            else
            {

                if (LifeTime < 0.18f)
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


            Velocity /= (float)Math.Pow(1.04f, master.dt);
            if (Math.Abs(Velocity.X + Velocity.Y) < 0.1f)
                Velocity = Vector2.Zero;
            LifeTime += master.gameTime.GetElapsedSeconds();
        }
    }
}
