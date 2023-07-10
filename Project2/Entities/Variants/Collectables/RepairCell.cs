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
    class RepairCell : CollectableEntity
    {
        private bool Moves;
        private float LifeTime;
        private Vector2 Velocity;
        public RepairCell(MyTexture texture, Vector2 position, float speed = 0, bool moves = true, float thrust = 0, bool collides = true, float weight = 1) : base(texture, position, speed, thrust, collides)
        {
            Moves = moves;
            Weight = weight;
            Rectangle full = new Rectangle((int)position.X, (int)position.Y, 16, 16);
            Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 1, 14, 14);
            Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, 16, 12);
            Rectangle upDown = new Rectangle((int)position.X + 2, (int)position.Y, 12, 16);
            Random random = new Random();
            float xDeviation = random.NextSingle(-1, 1);
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
                int heal = 10;
                if (master.storedDataManager.CurrentSaveFile.Purchases.Contains("better_repair_cells"))
                    heal = 18;
                master.entityManager.Player.Stats.Hp = Math.Min(master.entityManager.Player.Stats.Hp + heal, master.entityManager.Player.Stats.MaxHp);
            }
            if (LifeTime < 0.05f && Moves)
                Position += Velocity * (float)master.dt;
            else
            {
                bool move = true;
                foreach (Rectangle col in master.mapManager.CurrentMap.Collisions)
                {
                    if (col.Intersects(Hitbox.Body))
                        move = false;
                }
                if (move & Moves)
                    Position += Velocity * (float)master.dt;
            }


            Velocity.Y += 0.1f * Weight * (float)master.dt;
            LifeTime += master.gameTime.GetElapsedSeconds();
        }
    }
}
