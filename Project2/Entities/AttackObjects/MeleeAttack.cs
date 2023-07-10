using Spextria.Graphics;
using Spextria.Master;
using System;
using Spextria.Statics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics.GUI.PlayScreen;

namespace Spextria.Entities.AttackObjects
{
    class MeleeAttack : Attack
    {
        private AttackHurtbox Hurtbox;

        public MeleeAttack(MyTexture texture, string name, string element, string type, float damage, int knockback, double duration, int stages, double hitDuraion, double reloadTime, double chargeTime, AttackHurtbox hurtbox, Vector2 offset, Vector2 origSize = new Vector2(), bool heavy = false, float speedMult = 0, bool melee = true) : base(texture, name, element, type, damage, knockback, duration, stages, hitDuraion, reloadTime, chargeTime, offset, origSize, heavy, speedMult, melee)
        {
            Hurtbox = hurtbox;
        }

        public override bool CollidesWith(Rectangle rect)
        {
            if (Active)
                return Hurtbox.CollidesWith(new Rectangle(rect.X - (int)Position.X, rect.Y - (int)Position.Y, rect.Width, rect.Height));
            else
                return false;
        }

        public override void Strike(double direction, MasterManager master = null)
        {
            if (!Reloaded)
                return;

            Charging = true;
            Direction = direction;

            if (direction == 90)
            {
                Texture.SetType(0);
                Hurtbox.SetDirection("right");
                DrawPosOffset = PosOffset;
            }
            else
            {
                DrawPosOffset = new Vector2(-PosOffset.X - OrigSize.X, PosOffset.Y);
                Hurtbox.SetDirection("left");
                Texture.SetType(1);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                // attack size later?
                Texture.Draw(spriteBatch, Position + DrawPosOffset, 1, OrigSize);

                //DEBUG ATTACK HITBOX
                for (int i = 0; i < Hurtbox.Rectangles.Length; i++)
                {
                    Rectangle rect = Hurtbox.Rectangles[i];
                    //Images.ImageDict["button_red"].Draw(spriteBatch, new Vector2(rect.X+Position.X, rect.Y+Position.Y), 0.5F, new Vector2(rect.Width, rect.Height));
                
                }
            }
        }
    }
}
