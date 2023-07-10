using Microsoft.Xna.Framework;
using Spextria.Entities;
using Spextria.Master;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Maps.LevelObjectTypes
{
    class DamageArea : LevelObject
    {
        private float Damage;
        private int Knockback;
        public DamageArea(Vector2 position, Rectangle area, float damage, int knockback)
        {
            Position = position;
            Area = area;
            Damage = damage;
            Knockback = knockback;
        }

        public override void Update(MasterManager master)
        {
            if (Area.Intersects(master.entityManager.Player.Hitbox.Body))
            {
                master.entityManager.Player.Hurt(Damage, Knockback, Area.Center.ToVector2(), master, 0.4);
            }

            foreach (MonsterEntity monster in master.entityManager.EnemiesArray)
            {

                if (Area.Intersects(monster.Hitbox.Body))
                {
                    monster.Hurt(Damage, 0, Area.Center.ToVector2(), master, 0.4, "environment");
                }
            }
        }
    }
}
