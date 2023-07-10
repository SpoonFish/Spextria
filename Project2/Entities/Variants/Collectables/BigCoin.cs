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
    class BigCoin : CollectableEntity
    {
        public BigCoin(MyTexture texture, Vector2 position, float speed = 0, float thrust = 0, bool collides = true, float weight = 0) : base(texture, position, speed, thrust, collides)
        {
            Rectangle full = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 1, 30, 30);
            Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, 32, 28);
            Rectangle upDown = new Rectangle((int)position.X + 2, (int)position.Y, 28, 32);
            Hitbox = new HitboxObject(body, upDown, leftRight, full);
        }


        public override void Update(HitboxObject playerHitbox, MasterManager master)
        {
            if (Hitbox.Body.Intersects(playerHitbox.Body))
            {
                master.entityManager.CollectablesArray.Remove(this);
                Particles.CreateParticles(master.entityManager, 15, new Vector2(Position.X + 16, Position.Y + 16), "sparkles", 1f, "");
                master.storedDataManager.CurrentSaveFile.Coins += 10;
                master.playGuiManager.ReloadCoinCounter(master.storedDataManager.CurrentSaveFile.Coins); 
            }
        }
    }
}
