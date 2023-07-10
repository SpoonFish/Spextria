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


namespace Spextria.Entities.Variants
{
    class Coin : CollectableEntity
    {
        public Coin(MyTexture texture, Vector2 position, float speed = 0, float thrust = 0, bool collides = true, float weight = 0) : base(texture, position, speed, thrust, collides)
        {
            Rectangle full = new Rectangle((int)position.X, (int)position.Y, 16, 16);
            Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 1, 14, 14);
            Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, 16, 12);
            Rectangle upDown = new Rectangle((int)position.X + 2, (int)position.Y, 12, 16);
            Hitbox = new HitboxObject(body, upDown, leftRight, full);
        }


        public override void Update(HitboxObject playerHitbox, MasterManager master)
        {
            if (Hitbox.Body.Intersects(playerHitbox.Body))
            {
                master.entityManager.CollectablesArray.Remove(this);
                Particles.CreateParticles(master.entityManager, 5, new Vector2(Position.X + 8, Position.Y + 8), "sparkles", 1f, "");
                master.storedDataManager.CurrentSaveFile.Coins += 1;
                master.playGuiManager.ReloadCoinCounter(master.storedDataManager.CurrentSaveFile.Coins); 
            }
        }
    }
}
