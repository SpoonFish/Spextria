using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Spextria;
using Spextria.Entities.AttackObjects;
using Spextria.Graphics;
using Spextria.Graphics.GUI.PlayScreen;
using Spextria.Master;
using Spextria.Statics;

namespace Spextria.Entities
{
    class MonsterEntity : MovingEntity
    {
        public HitboxObject Hitbox;
        protected bool Knocked;
        protected bool Dead;
        protected float DeathTime;
        public MonsterStats Stats;
        protected Rectangle rect1;
        protected Rectangle rect2;
        public MonsterHealthBar HealthBar;
        public string ParticleType;
        protected DataTypes.Intelligence OriginalIntelligence;
        protected DataTypes.Intelligence Intelligence;
        protected DataTypes.Hostility Hostility;
        protected List<Attack> AttackList;
        public Attack CurrentAttack;
        protected DataTypes.Directions MovingDir;

        protected double StunTime;
        protected double HitTime;
        //public string CurrentPlanet;
        protected PhysicsObject Physics;
        public MonsterEntity(MyTexture texture, Vector2 position, MonsterStats stats, float speed, float thrust, float weight, List<Attack> attacks, DataTypes.Intelligence intelligence = DataTypes.Intelligence.Wander, DataTypes.Hostility hostility = DataTypes.Hostility.Neutral) : base(texture, position, speed, thrust)
        {
            StunTime = 0;
            Knocked = false;
            rect2 = new Rectangle(0, 0, 0, 0);
            rect1 = new Rectangle(0, 0, 0, 0);
            AttackList = attacks;
            CurrentAttack = AttackList[new Random().Next(0, AttackList.Count - 1)];
            Stats = stats;
            Intelligence = intelligence;
            Hostility = hostility;
            MovingDir = DataTypes.Directions.None;
            Dead = false;
            HitTime = 0f;
            DeathTime = 0f;
            Physics = new PhysicsObject(weight, speed, thrust);
            Rectangle full = new Rectangle((int)position.X, (int)position.Y, texture.GetWidth(), texture.GetHeight());
            Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 1, texture.GetWidth() - 2, texture.GetHeight() - 1);
            Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, texture.GetWidth(), texture.GetHeight() - 4);
            Rectangle upDown = new Rectangle((int)position.X + 4, (int)position.Y, texture.GetWidth() - 8, texture.GetHeight());
            Hitbox = new HitboxObject(body, upDown, leftRight, full);
            HealthBar = new MonsterHealthBar(new Bar(new Vector2(Hitbox.Body.Center.X - 16, Hitbox.Full.Y - 6), Stats.MaxHp, 0, "enemy_hp"));
        }

        public Vector2 GetPosition()
        {
            return Position;
        }


        private enum directions
        {
            left,
            right,
            up,
            down,
            none
        }

        public virtual void Hurt(float damage, int knockback, Vector2 fromPosition, MasterManager master, double hitTime = 0.8d, string element = "none")
        {

            if (Hostility == DataTypes.Hostility.Neutral)
                Intelligence = DataTypes.Intelligence.Approach;
            if (HitTime > 0 || Dead)
                return;

            if (Stats.HasTorpor)
            {
                Stats.Torpor -= knockback;
                if (Stats.Torpor <= 0)
                {
                    Knocked = true;

                    Stats.Torpor = Stats.MaxTorpor;
                }
                else if(!Knocked)
                {
                    knockback = 0;
                }
            }
            double angle = Functions.Bearing(fromPosition, Hitbox.Body.Center.ToVector2());
            //master.playGuiManager.SetDebugVal(angle.ToString());

            HitTime = hitTime;

            switch (element)
            {
                case "none":
                    damage *= Stats.NoneRes * -1 + 1;
                    break;
                case "light":
                    damage *= Stats.LightRes * -1 + 1;
                    break;
                case "growth":
                    damage *= Stats.GrowthRes * -1 + 1;
                    break;
                case "frost":
                    damage *= Stats.FrostRes * -1 + 1;
                    break;
                case "shadow":
                    damage *= Stats.ShadowRes * -1 + 1;
                    break;
                case "flame":
                    damage *= Stats.FlameRes * -1 + 1;
                    break;
                case "torment":
                    damage *= Stats.TormentRes * -1 + 1;
                    break;
                case "environment":
                    damage *= Stats.EnvironmentRes * -1 + 1;
                    break;
            }
            if (damage == 0)
                HitTime = 0;
            else
                Physics.Knockback(angle, knockback, Stats.KnockbackResistance);
            Stats.Hp = Math.Max(0, Stats.Hp - damage);
            if (Stats.Hp == 0)
                Kill(master);
        }
        private void Kill(MasterManager master)
        {
            if (Dead)
                return;
            Dead = true;
        }
        private void UpdateMovement(MasterManager master) 
        { 
        }


        protected void UpdateAttack(MasterManager master)
        {
            CurrentAttack.Update(master, Hitbox.Body.Center.ToVector2(), Physics.Velocity);
            Rectangle playerRect = master.entityManager.Player.Hitbox.Body;

            if (Stats.ContactDamage > 0 && Hitbox.Body.Intersects(playerRect))
            {
                master.entityManager.Player.Hurt(Stats.ContactDamage, Stats.Knockback, Hitbox.Body.Center.ToVector2(), master, Stats.ContactDmgHitTime);
            }

            if (CurrentAttack.Reloaded && !CurrentAttack.Charging)
                CurrentAttack = AttackList[new Random().Next(0, AttackList.Count)];
            //Monster attacks player slightly before its considered as in range (10% increased range)
            if (Math.Abs(master.entityManager.Player.Hitbox.Body.Center.X - Hitbox.Body.Center.X) < Stats.AttackRange * 1.1f || (!CurrentAttack.Melee && Math.Abs(master.entityManager.Player.Hitbox.Body.Center.X - Hitbox.Body.Center.X) < 320 && Stats.Hp < Stats.MaxHp))
            {

                if (Hostility == DataTypes.Hostility.Hostile)
                    Intelligence = DataTypes.Intelligence.Approach;
                if (CurrentAttack.Melee)
                    if (master.entityManager.Player.Hitbox.Body.Center.X > Hitbox.Body.Center.X)
                        CurrentAttack.Strike(90);
                    else
                        CurrentAttack.Strike(270);
                else
                    if (Physics.Jumping || master.entityManager.Player.Hitbox.Body.Center.Y - 32  <  Hitbox.Body.Center.Y)
                        CurrentAttack.Strike(Functions.Bearing(Hitbox.Body.Center.ToVector2(), master.entityManager.Player.Hitbox.Body.Center.ToVector2() - new Vector2(0, (32*CurrentAttack.ProjWeight))), master);
                    else
                        CurrentAttack.Strike(Functions.Bearing(Hitbox.Body.Center.ToVector2(), master.entityManager.Player.Hitbox.Body.Center.ToVector2() - new Vector2(0, (23 * CurrentAttack.ProjWeight))), master);
            }
        }
        protected void UpdateDeath(MasterManager master)
        {
            DeathTime += (float)master.timePassed;
            if (DeathTime < 0.3f * master.dt)
            {
                Particles.CreateParticles(master.entityManager, 1, new Vector2(Hitbox.Body.Center.X, Hitbox.Body.Center.Y), "player fragments", 0, "", Physics.Velocity/7);
                Particles.CreateParticles(master.entityManager, 1, new Vector2(Hitbox.Body.Center.X, Hitbox.Body.Center.Y), "player explode", 0, "", Physics.Velocity/7);
            }
            else
            {
                master.entityManager.DropLoot(Stats, new Vector2(Hitbox.Body.Center.X, Hitbox.Body.Center.Y), master);
                master.entityManager.EnemiesArray.Remove(this);
            }

        }
     
        public virtual void Update(MasterManager master)
        {
            HealthBar.Update(new Vector2(Hitbox.Body.Center.X - 16, Hitbox.Full.Y - 6), master, Stats.Hp);

            if (HitTime > 0f)
                HitTime -= (float)master.timePassed;
            Texture.Update(master.timePassed);
            if (Dead)
                UpdateDeath(master);
            else
                UpdateMovement(master);
                UpdateAttack(master);
            if (Position.Y > master.mapManager.CurrentMap.Bottom)
                Kill(master);
        }

        public override void Draw(SpriteBatch spriteBatch, float mapOpacity = 1)
        {
            //Images.ImageDict["button_red"].Draw(spriteBatch, new Vector2(rect2.X, rect2.Y), 0.5F, new Vector2(rect2.Width, rect2.Height));

            //Images.ImageDict["button_red"].Draw(spriteBatch, new Vector2(rect1.X, rect1.Y), 0.5F, new Vector2(rect1.Width, rect1.Height));

            if (Stats.Hp < Stats.MaxHp && !Dead)
                HealthBar.Draw(spriteBatch);
            float opacity = 1f;
            if (!Dead)
            {
                if (HitTime > 0 && (int)Math.Round(HitTime * 10) % 2 == 0)
                    opacity = 0.5f;
                Texture.Draw(spriteBatch, Position, opacity * mapOpacity);
                CurrentAttack.Draw(spriteBatch);
            }
        }
        public void Debug(SpriteBatch spriteBatch)
        {
            Hitbox.Debug(spriteBatch, Position);
        }
    }
}
