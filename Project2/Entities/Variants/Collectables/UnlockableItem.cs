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
    class UnlockableItem : CollectableEntity
    {
        private string Item;
        public UnlockableItem(MyTexture texture, Vector2 position, float speed = 0, float thrust = 0, bool collides = true, float weight = 1) : base(texture, position, speed, thrust, collides)
        {
            Item = texture.Name;
            Weight = weight;
            Rectangle full = new Rectangle((int)position.X, (int)position.Y, 16, 16);
            Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 1, 14, 14);
            Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, 16, 12);
            Rectangle upDown = new Rectangle((int)position.X + 2, (int)position.Y, 12, 16);
            Random random = new Random();
            float xDeviation = random.NextSingle(-1, 1);
            float y = random.NextSingle(-1, -3);
            Hitbox = new HitboxObject(body, upDown, leftRight, full);
        }


        public override void Update(HitboxObject playerHitbox, MasterManager master)
        {
            if (master.storedDataManager.CheckSkillUnlock(Item))
                master.entityManager.CollectablesArray.Remove(this);
            if (Hitbox.Body.Intersects(playerHitbox.Body))
            {
                master.entityManager.CollectablesArray.Remove(this);
                master.storedDataManager.CurrentSaveFile.Purchases.Add(Item);
                master.playGuiManager.NotifyNewItem(Item);
            }

        }
    }
}
