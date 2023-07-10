using Spextria.Graphics;
using Spextria.Master;
using System;
using Spextria.Statics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics.GUI.PlayScreen;
using static Spextria.Statics.DataTypes;

namespace Spextria.Entities.AttackObjects
{
    class RangedAttack : Attack
    {
        private bool StrictlyLeftRightShooting;
        private bool PlayerOwned;
        private bool ProjCollides;
        private bool ProjExplodes;
        private int ProjPierce;
        private int Projectiles;
        private int ProjTravelRange;
        private int ProjRounds;
        private double OrigDirection;
        private double ProjInnacuracy;
        private float ProjSpeed;
        private Vector2 ProjSpawnOffset;
        private MyTexture ProjTexture;
        private MyTexture ExplodeTexture;
        private int ExplodeDamage;
        private int RoundsFired;
        private Rectangle ExplodeSize;
        private string ProjParticle;
        private Rectangle ProjSize;

        public RangedAttack(MyTexture texture, string name, string element, string type, float damage, int knockback, double duration, int stages, double hitDuration, double reloadTime, double chargeTime, bool playerOwned, float projWeight, bool projCollides, float projSpeed, int projPierce, Rectangle projSize, MyTexture projTexture , Vector2 offset, Vector2 origSize = new Vector2(), Vector2 projSpawnOffset = new Vector2(), bool heavy = false, float speedMult = 0, int projectiles = 1, string particle = "", int rounds = 1, double inaccuracy = 0, int range = 1600, bool strictDirection = false, int explodeDamage = 0, MyTexture explodeTexture = null, Rectangle explodeSize = new Rectangle(), bool melee = false) : base(texture, name, element, type, damage, knockback, duration, stages, hitDuration, reloadTime, chargeTime, offset, origSize, heavy, speedMult, melee)
        {
            ExplodeDamage = explodeDamage;
            if (explodeTexture != null)
            {
                ProjExplodes = true;
            }
            else
            {
                ProjExplodes = false;
            }
            StrictlyLeftRightShooting = strictDirection;
            ProjSpawnOffset = projSpawnOffset;
            OrigDirection = 0;
            RoundsFired = 0;
            ExplodeSize = explodeSize;
            ExplodeTexture = explodeTexture;
            ProjParticle = particle;
            ProjTexture = projTexture;
            PlayerOwned = playerOwned;
            ProjCollides = projCollides;
            ProjWeight = projWeight;
            ProjSpeed = projSpeed;
            ProjPierce = projPierce;
            ProjSize = projSize;
            Projectiles = projectiles;
            ProjInnacuracy = inaccuracy;
            ProjTravelRange = range;
            ProjRounds = rounds;
        }

        private void Shoot(MasterManager master, Vector2 initialVel)
        {

            Direction = OrigDirection -5 * Projectiles + 5;
            int rotationIncrement = 0;
            for (int i = 0; i < Projectiles; i++)
            {
                Vector2 posOffset;
                if (Direction < 180)
                    posOffset = ProjSpawnOffset;
                else
                    posOffset = new Vector2(-ProjSpawnOffset.X, ProjSpawnOffset.Y);
                ProjectileEntity projectile = new ProjectileEntity(PlayerOwned, Damage, Knockback, ProjPierce, HitDuration, ProjCollides, ProjSpeed, ProjWeight, ProjSize, ProjTexture, new Vector2(Position.X - ProjSize.X / 2, Position.Y - ProjSize.Y / 2) + posOffset, Direction + rotationIncrement + (new Random().NextDouble() - 0.5) * 2 * ProjInnacuracy, initialVel, ProjParticle, ProjRounds, ProjInnacuracy, ProjTravelRange, ProjExplodes, ExplodeDamage, ExplodeSize, ExplodeTexture, Element );
                master.entityManager.ProjectilesArray.Add(projectile);
                rotationIncrement += 10;

            }
            RoundsFired += 1;
        }
        public override void Update(MasterManager master, Vector2 position, Vector2 initialVel)
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
                //if (PlayerOwned)
                    //master.GameSpeed = 0.2f;
                ChargeTimeActive += master.timePassed;
                if (ChargeTimeActive > ChargeTime)
                {
                    Shoot(master, initialVel);
                    Active = true;

                    Reloaded = false;
                    ReloadTimeActive = 0;
                    TimeActive = 0;
                    ChargeTimeActive = 0;
                    Charging = false;
                    ChargeTimeActive = 0;
                }
                Stage = (int)Math.Floor(ChargeTimeActive / ChargeTime * Stages);
                Texture.CurrentFrame = Stage;
            }
            if (Active)
            {
                //master.GameSpeed = 1f;
                TimeActive += master.timePassed;
                if (TimeActive > Duration)
                    Active = false;

                if (ProjRounds > 1)
                {
                    if (RoundsFired == ProjRounds)
                        return;
                    if (TimeActive > Duration / ProjRounds * (RoundsFired))
                        Shoot(master, initialVel);
                }

            }

        }
        public override void Strike(double direction, MasterManager master)
        {
            if (!Reloaded)
                return;

            RoundsFired = 0;
            Charging = true;
            Direction = direction;
            OrigDirection = direction;

            if (direction < 180)
            {
                Texture.SetType(0);
                DrawPosOffset = PosOffset;
                if (StrictlyLeftRightShooting)
                {
                    Direction = 90;
                    OrigDirection = 90;
                }
            }
            else
            {
                DrawPosOffset = new Vector2(-PosOffset.X - OrigSize.X, PosOffset.Y);
                Texture.SetType(1);
                if (StrictlyLeftRightShooting)
                {
                    Direction = 270;
                    OrigDirection = 270;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Charging)
            {
                // attack size later?
                Texture.Draw(spriteBatch, Position + DrawPosOffset, 1, OrigSize);
            }
        }
    }
}
