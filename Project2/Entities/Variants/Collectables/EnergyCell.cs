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
    class EnergyCell : CollectableEntity
    {
        private float LifeTime;
        private Vector2 Velocity;
        public EnergyCell(MyTexture texture, Vector2 position, float speed = 0, float thrust = 0, bool collides = true, float weight = 1) : base(texture, position, speed, thrust, collides)
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
                int heal = 15;
                if (master.storedDataManager.CheckSkillUnlock("better_energy_cells"))
                    heal = 25;
                master.entityManager.CollectablesArray.Remove(this);
                master.entityManager.Player.Stats.Se = Math.Min(master.entityManager.Player.Stats.Se + heal, master.entityManager.Player.Stats.MaxSe);
            }
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


            Velocity.Y += 0.1f * Weight * (float)master.dt;
            LifeTime += master.gameTime.GetElapsedSeconds();
        }
    }
}
