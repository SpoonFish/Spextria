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
    class Attack
    {
        public string Name;
        protected MyTexture Texture;
        protected string Type;
        public bool Melee;
        public float Damage;
        public int Knockback;
        public bool Heavy;
        public float ProjWeight;
        protected double TimeActive;
        protected Vector2 Position;
        protected double Duration;
        public bool Active;
        protected Vector2 DrawPosOffset;
        protected Vector2 PosOffset;
        public double ReloadTime;
        public double ReloadTimeActive;
        public double ChargeTime;
        public double ChargeTimeActive;
        public bool Charging;
        public float SpeedMult;
        public bool Reloaded;
        protected Vector2 OrigSize;
        public int Stages;
        public int Stage;
        public double Direction;
        public double HitDuration; //invincibility frames
        protected float ArmourPierce;
        public string Element;
        public Attack(MyTexture texture, string name, string element, string type, float damage, int knockback, double duration, int stages, double hitDuraion, double reloadTime, double chargeTime, Vector2 offset, Vector2 origSize = new Vector2(), bool heavy = false, float speedMult = 0, bool melee = false)
        {
            Element = element;
            Melee = melee;
            SpeedMult = speedMult;
            Heavy = heavy;
            OrigSize = origSize;
            if (OrigSize == Vector2.Zero)
                OrigSize = new Vector2(64, 32);
            ChargeTime = chargeTime;
            Damage = damage;
            Charging = false;
            Name = name;
            PosOffset = offset;
            DrawPosOffset = offset;
            ReloadTimeActive = 0;
            Reloaded = false;
            Active = false;
            ReloadTime = reloadTime;
            TimeActive = 0;
            Texture = Images.UniqueImage(texture);
            Direction = 1;
            Type = type;
            Knockback = knockback;
            Duration = duration;
            Stages = stages;
            HitDuration = hitDuraion;
            Stage = 0;
        }


        public  virtual bool CollidesWith(Rectangle rect)
        {
            return false;
        }
        public virtual void Update(MasterManager master, Vector2 position, Vector2 initialVel = new Vector2())
        {
            Position = position;
            ReloadTimeActive += master.timePassed;
            if (ReloadTimeActive > ReloadTime)
            {
                Reloaded = true;
                ReloadTimeActive = 0;
                Stage = 0;
            }
            else

            if (Charging)
            {
                ChargeTimeActive += master.timePassed;
                if (ChargeTimeActive > ChargeTime)
                {
                    Active = true;

                    Reloaded = false;
                    ReloadTimeActive = 0;
                    TimeActive = 0;
                    ChargeTimeActive = 0;
                    Charging = false;
                    ChargeTimeActive = 0;
                }
            }
            if (Active)
            {
                TimeActive += master.timePassed;
                Stage = (int)Math.Floor(TimeActive / Duration * Stages);
                Texture.CurrentFrame = Stage;
                if (TimeActive > Duration)
                    Active = false;
            }

        }

        public virtual void Cancel()
        {
            Charging = false;
            ChargeTimeActive = 0;
            TimeActive = 0;
            Active = false;
            Reloaded = false;
            ReloadTimeActive = 0;
        }
        public virtual void Strike(double direction, MasterManager master = null)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                // attack size later?
                Texture.Draw(spriteBatch, Position + DrawPosOffset, 1, OrigSize);
            }
        }
    }
}
