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
using Spextria.Maps.LevelObjectTypes;
using Spextria.Master;
using Spextria.Statics;

namespace Spextria.Entities
{
    class Bloodscale : SolidMonsterEntity
    {
        public Bloodscale(MyTexture texture, Vector2 position, MonsterStats stats, float speed, float thrust, float weight = 2, List<Attack> attacks = null, HitboxObject customHitbox = null, DataTypes.Intelligence intelligence = DataTypes.Intelligence.Wander, DataTypes.Hostility hostility = DataTypes.Hostility.Neutral) : base(texture, position, stats, speed, thrust, weight, attacks, null, intelligence, hostility)
        {
            AttackList = attacks;
            OriginalIntelligence = intelligence;
            Intelligence = DataTypes.Intelligence.Approach;
            Hostility = DataTypes.Hostility.Hostile;
            MovingDir = DataTypes.Directions.None;
            Dead = false;
            DeathTime = 0f;
            NeedsToJump = false;
            JumpChargeTime = .25f;
            JumpTime = 0f;
            _TurnTime = 0f;
            Rectangle full = new Rectangle((int)position.X, (int)position.Y, texture.GetWidth(), texture.GetHeight());
            Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 1, texture.GetWidth() - 2, texture.GetHeight() - 1);
            Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, texture.GetWidth(), texture.GetHeight() - 4);
            Rectangle upDown = new Rectangle((int)position.X + 4, (int)position.Y, texture.GetWidth() - 8, texture.GetHeight());
            Hitbox = new HitboxObject(body, upDown, leftRight, full);
        }
        private void Kill(MasterManager master)
        {
            if (Dead)
                return;
            Dead = true;
            master.mapManager.CurrentMap.Triggers["1"] = 1;

        }
        private void CheckRect(Rectangle rect, MasterManager master, string particleType)
        {
            if (rect.Intersects(Hitbox.UpDown))
            {
                float prevY = Position.Y;
                JumpTime = 0;
                Physics.Accel.Y = 0;
                if (Hitbox.Full.Y < rect.Y)
                {
                    if (Math.Abs((rect.Top - (Hitbox.Full.Bottom - Position.Y)) - Position.Y) > 24)
                        return;
                    Position.Y = rect.Top - (Hitbox.Full.Bottom - Position.Y);
                    Physics.Falling = false;
                    Physics.OtherFalling = false;
                    if (!Dead)
                        Particles.CreateJumpParticles(ParticleType, Position + new Vector2(new Random().Next(0, Hitbox.Full.Width), 0), Hitbox, master);
                }
                else if (Physics.Velocity.Y < 0)
                {
                    if (Math.Abs(rect.Bottom - Position.Y) > 24)
                        return;
                    Position.Y = rect.Bottom + (Hitbox.Full.Top - Position.Y);


                    Physics.OtherFalling = true;
                    Physics.AirTime = 0.4f;
                    JumpTime = JumpChargeTime;
                }
                Physics.Velocity.Y = 0;
                Physics.KnockbackVelocity.Y = 0;
            }
            else
            {
                Rectangle inGroundRect = new Rectangle(rect.X, rect.Y - 2, rect.Width, rect.Height + 1);
                if (inGroundRect.Intersects(Hitbox.UpDown))
                    Physics.Falling = false;
                if (rect.Intersects(Hitbox.LeftRight))
                {
                    Physics.Velocity.X = 0;
                    Physics.KnockbackVelocity.X = 0;
                    Physics.Accel.X = 0;
                    if (Hitbox.Full.X < rect.X)
                        Position.X = rect.Left - (Hitbox.Full.Right - Position.X);
                    else
                        Position.X = rect.Right + (Hitbox.Full.Left - Position.X) + 1;
                }

            }
        }
        private void CheckCollisions(MasterManager master, string particleType)
        {
            Physics.Falling = true;

            Physics.Falling = true;
            foreach (Rectangle rect in master.mapManager.CurrentMap.Collisions)
            {
                CheckRect(rect, master, particleType);
            }
            foreach (Gate gate in master.mapManager.CurrentMap.Gates)
            {
                if (gate.State == "on")
                    CheckRect(gate.Rect, master, particleType);
            }



            if (!(Hostility == DataTypes.Hostility.Hostile && (Hitbox.Full.Bottom < master.entityManager.Player.Hitbox.Full.Bottom - 24)))
                foreach (Rectangle rect in master.mapManager.CurrentMap.Semisolids)
                {
                    if (rect.Y + 8 < Hitbox.Feet.Bottom || !Physics.Falling)
                        continue;
                    if (rect.Intersects(Hitbox.UpDown))
                    {
                        Physics.Accel.Y = 0;
                        if (Physics.Velocity.Y > 0 && Physics.Falling)
                            JumpTime = 0;
                        Position.Y = rect.Top - Hitbox.UpDown.Height;
                        Physics.Falling = false;
                        Physics.OtherFalling = false;

                        if (!Dead)
                            Particles.CreateJumpParticles(ParticleType, Position + new Vector2(new Random().Next(0, Hitbox.Full.Width), 0), Hitbox, master);

                    }
                    else
                    {
                        Rectangle inGroundRect = new Rectangle(rect.X, rect.Y - 1, rect.Width, 8);
                        if (inGroundRect.Intersects(Hitbox.UpDown))
                            Physics.Falling = false;
                    }
                }
        }
    
        private void CheckNeedsJumping(MasterManager master)
        {

            //middle bottom of hitbox
            Vector2 midPosition = Position + new Vector2(Hitbox.LeftRight.Width / 2, +Hitbox.UpDown.Height);

            Vector2 wallSenseOffset = new Vector2();
            Vector2 wallSensePos = midPosition + new Vector2(0, -8);

            if (MovingDir == DataTypes.Directions.Left)
                wallSenseOffset.X -= 8+ Hitbox.Body.Width;
            else
                wallSenseOffset.X += 8+ Hitbox.Body.Width;
            rect2 = new Rectangle((int)wallSensePos.X + (int)wallSenseOffset.X - 4, (int)wallSensePos.Y + (int)wallSenseOffset.Y - 4, 8, 8);

            Rectangle holeSenseArea = new Rectangle((int)midPosition.X - 8 + (int)(wallSenseOffset.X/1.5f) - 1, (int)midPosition.Y - 8, 14, 35 + (int)(30 * Thrust));
            if (Intelligence == DataTypes.Intelligence.Sidetoside)
                holeSenseArea.Height = 15;
            rect1 = holeSenseArea;
            bool holeSensed = true; //disproven
            bool wallSensed = false; //proven

            // jump over walls and turn at holes
            foreach (Rectangle rect in master.mapManager.CurrentMap.Collisions)
            {
                wallSensed = rect.Contains(wallSensePos + wallSenseOffset);
                if (wallSensed)
                    if (_TurnTime < 0.1f && (Intelligence == DataTypes.Intelligence.Sidetoside || Intelligence == DataTypes.Intelligence.Leap))
                    {
                        if (MovingDir == DataTypes.Directions.Left)
                            MovingDir = DataTypes.Directions.Right;
                        else
                            MovingDir = DataTypes.Directions.Left;

                    }
                    else if (Intelligence == DataTypes.Intelligence.Leap || Intelligence == DataTypes.Intelligence.Approach || Intelligence == DataTypes.Intelligence.Scout)
                        NeedsToJump = true;

                if (rect.Intersects(holeSenseArea))
                    holeSensed = false;
            }

            foreach (Rectangle rect in master.mapManager.CurrentMap.Semisolids)
            {
                wallSensed = rect.Contains(wallSensePos + wallSenseOffset);
                if (wallSensed)
                    if (_TurnTime < 0.1f && (Intelligence == DataTypes.Intelligence.Sidetoside || Intelligence == DataTypes.Intelligence.Leap))
                    {
                        if (MovingDir == DataTypes.Directions.Left)
                            MovingDir = DataTypes.Directions.Right;
                        else
                            MovingDir = DataTypes.Directions.Left;

                    }
                    else if (Intelligence == DataTypes.Intelligence.Leap || Intelligence == DataTypes.Intelligence.Approach || Intelligence == DataTypes.Intelligence.Scout)
                        NeedsToJump = true;

                if (rect.Intersects(holeSenseArea))
                    holeSensed = false;
            }

            if (holeSensed)
                if (master.entityManager.Player.Position.Y+32 > holeSenseArea.Bottom && (Intelligence == DataTypes.Intelligence.Approach || Intelligence == DataTypes.Intelligence.Scout))
                    holeSensed = false;
                else
                    if (!(Intelligence == DataTypes.Intelligence.Approach))
                    if (MovingDir == DataTypes.Directions.Left)
                        MovingDir = DataTypes.Directions.Right;
                    else
                        MovingDir = DataTypes.Directions.Left;
                else
                    MovingDir = DataTypes.Directions.None;
        }
        private void CheckIntelligence(MasterManager master)
        {
            switch (Intelligence)
            {
                case DataTypes.Intelligence.Scout: //always moving
                    _TurnTime -= (float)master.timePassed;
                    if (_TurnTime < 0)
                    {
                        Random rand = new Random();
                        _TurnTime = rand.Next(1, 4);
                        int num = rand.Next(1, 3);
                        if (num == 1)
                            MovingDir = DataTypes.Directions.Left;
                        else
                            MovingDir = DataTypes.Directions.Right;
                    }
                    break;

                case DataTypes.Intelligence.Wander: //high possibility to idle + longer periods of same movement
                    _TurnTime -= (float)master.timePassed;
                    if (_TurnTime < 0)
                    {
                        Random rand = new Random();
                        _TurnTime = rand.Next(2, 6);
                        int num = rand.Next(1, 4);
                        if (num == 1)
                            MovingDir = DataTypes.Directions.Left;
                        else if (num == 2)
                            MovingDir = DataTypes.Directions.Right;
                        else
                            MovingDir = DataTypes.Directions.None;
                    }
                    break;

                case DataTypes.Intelligence.Approach: //approach player as if it is hostile at an infinite range
                    // TODO: ADD ATTACK RANGE WHERE MONSTER STOPS APPROACHNG PLAYER 
                    _TurnTime -= (float)master.timePassed;
                    if (_TurnTime < 0)
                    {
                        Random rand = new Random();
                        _TurnTime = rand.NextSingle(0, 0.4f);
                        float range = Stats.AttackRange;
                        if (Math.Abs(Hitbox.Full.Bottom - master.entityManager.Player.Hitbox.Full.Bottom) > 18)
                            range = 0;

                        if (Hitbox.Body.Center.X < master.entityManager.Player.Hitbox.Body.Center.X - range)
                            MovingDir = DataTypes.Directions.Right;
                        else if (Hitbox.Body.Center.X > master.entityManager.Player.Hitbox.Body.Center.X + range)
                            MovingDir = DataTypes.Directions.Left;
                        else
                             MovingDir = DataTypes.Directions.None;

                        if (Math.Abs(master.entityManager.Player.Hitbox.Body.Center.X - Hitbox.Body.Center.X) > 480)
                            MovingDir = DataTypes.Directions.None;


                        if (rand.Next(1, 20) == 5)
                            NeedsToJump = true;
                        else
                            NeedsToJump = false;
                        if (rand.Next(1, 20) == 3)
                        {
                            _TurnTime = rand.NextSingle(0.7f, 1.2f);
                            if (MovingDir == DataTypes.Directions.Left)
                                MovingDir = DataTypes.Directions.Right;
                            else if (MovingDir == DataTypes.Directions.Right)
                                MovingDir = DataTypes.Directions.Left;
                        }

                    }
                    break;

                case DataTypes.Intelligence.Leap: //jump from side of ground to other
                    _TurnTime -= (float)master.timePassed;
                    if (_TurnTime < 0)
                    {
                        Random rand = new Random();
                        _TurnTime = rand.NextSingle(0.25f, 1f);
                        if (NeedsToJump == false)
                            NeedsToJump = true;
                        else
                            NeedsToJump = false;
                    }
                    if (MovingDir == DataTypes.Directions.None)
                        if (new Random().Next(1,2) == 2)
                            MovingDir = DataTypes.Directions.Right;
                        else
                            MovingDir = DataTypes.Directions.Left;
                    break;


                case DataTypes.Intelligence.Sidetoside: //walk side to side falling of ledges

                    _TurnTime -= (float)master.timePassed;
                    if (_TurnTime < 0)
                        _TurnTime = 0.27f;
                    {
                    }
                    if (MovingDir == DataTypes.Directions.None)
                        if (new Random().Next(1, 2) == 2)
                            MovingDir = DataTypes.Directions.Right;
                        else
                            MovingDir = DataTypes.Directions.Left;
                    break;
            }

        }
        private void UpdateMovement(MasterManager master)
        {
            //Rectangle[] collisions = master.mapManager.CurrentMap.Collisions;

            //DataTypes.Directions movingDir = DataTypes.Directions.None;
            if (Knocked)
            {
                CurrentAttack.Cancel();
                Physics.Jumping = false;
                if (Facing == DataTypes.Directions.Left)
                {
                    Texture.SetType(12);
                }
                else
                {
                    Texture.SetType(13);
                }
                StunTime += master.timePassed;
                if (StunTime > Stats.StunLength)
                {
                    StunTime = 0;
                    Knocked = false;

                    if (Facing == DataTypes.Directions.Left)
                    {
                        Texture.SetType(0);
                    }
                    else
                    {
                        Texture.SetType(1);
                    }
                }

                Texture.CurrentFrame = Math.Min(3, (int)(StunTime / Stats.StunLength * 4));

                NeedsToJump = false;
                JumpChargeTime = 0;
                Physics.UpdateMovement((float)master.dt, (float)master.timePassed);
                Position += Physics.Velocity * (float)master.dt;
                Position += Physics.KnockbackVelocity * (float)master.dt;
            }
            else
            {
                CheckIntelligence(master);
                CheckNeedsJumping(master);
                ParticleType = master.mapManager.GetGroundType(Hitbox.Body).ParticleType;


                if (NeedsToJump || (JumpTime > 0 && JumpTime < JumpChargeTime))
                {
                    JumpTime += (float)master.timePassed;
                    if (JumpTime > JumpChargeTime)
                        Physics.UpdateJump((float)master.dt);
                }
                else
                    Physics.Jumping = false;

                Physics.UpdateMovement((float)master.dt, (float)master.timePassed);

                if (MovingDir == DataTypes.Directions.Left)
                {
                    // change vel and accel to move
                    Physics.Accel.X /= (float)Math.Pow(2, master.dt);
                    Physics.Velocity.X -= Physics.Accel.X * (float)master.dt;

                    // idk abt skidding make some funny particles maybe for some quick monsteores

                    Facing = DataTypes.Directions.Left;
                    Texture.SetType(2);
                }
                else if (Facing == DataTypes.Directions.Left && Physics.Velocity.X == 0)
                {
                    Texture.SetType(0);
                }

                if (MovingDir == DataTypes.Directions.Right || master.mapManager.CurrentMap.LevelCompleted())
                {
                    Physics.Accel.X /= (float)Math.Pow(2, master.dt);
                    Physics.Velocity.X += Physics.Accel.X * (float)master.dt;

                    if (!Physics.Falling && Physics.Velocity.X < 0)
                    {
                        Particles.CreateSkidParticles(ParticleType, Position + new Vector2(new Random().Next(0, Hitbox.Full.Width), 0), MovingDir, Hitbox, master);
                    }

                    Facing = DataTypes.Directions.Right;
                    Texture.SetType(3);
                }
                else if (Facing == DataTypes.Directions.Right && Physics.Velocity.X == 0)
                {
                    Texture.SetType(1);
                }

                if (MovingDir == DataTypes.Directions.None)
                {
                    if (Physics.Falling)
                        Physics.Accel.X /= (float)Math.Pow(4, master.dt);
                    else
                        Physics.Accel.X /= (float)Math.Pow(2, master.dt);



                    if (Physics.Velocity.X > Physics.Accel.X * 2 * master.dt)
                        Physics.Velocity.X -= Physics.Accel.X * (float)master.dt;
                    else if (Physics.Velocity.X < -Physics.Accel.X * 2 * master.dt)
                        Physics.Velocity.X += Physics.Accel.X * (float)master.dt;
                    else
                        Physics.Velocity.X = 0;
                }

                if (!Physics.Falling && Physics.Velocity.X != 0)
                {
                    // walk particles when moving and on ground
                    Particles.CreateWalkParticles(ParticleType, Position + new Vector2(new Random().Next(0, Hitbox.Full.Width), 0), MovingDir, Hitbox, master);
                }

                if (Physics.OtherFalling && JumpTime > JumpChargeTime)
                {
                    if (Facing == DataTypes.Directions.Right)
                        Texture.SetType(5);
                    else
                        Texture.SetType(4);
                }

                if (CurrentAttack.Active && CurrentAttack.Name != "none")
                {
                    Physics.Velocity.X *= (float)Math.Pow(CurrentAttack.SpeedMult, master.dt);
                    if (Physics.Falling && Physics.Velocity.Y > 0)
                        Physics.Velocity.Y += 0.4f * (float)master.dt;
                    if (CurrentAttack.Direction < 180)
                        Texture.SetType(9);
                    else
                        Texture.SetType(8);
                    Texture.CurrentFrame = (int)Math.Floor(4f * ((float)CurrentAttack.Stage / ((float)CurrentAttack.Stages + 1f)));
                }
                else if (CurrentAttack.Charging && CurrentAttack.Name != "none")
                {
                    Physics.Velocity.X /= (float)Math.Pow(1.45, master.dt);
                    if (CurrentAttack.Direction < 180)
                        Texture.SetType(11);
                    else
                        Texture.SetType(10);
                    Texture.CurrentFrame = (int)Math.Floor(4f * ((float)CurrentAttack.ChargeTimeActive / ((float)CurrentAttack.ChargeTime)));

                }

                else if (JumpTime > 0 && JumpTime < JumpChargeTime)
                {
                    Particles.CreateSkidParticles(ParticleType, Position + new Vector2(new Random().Next(0, Hitbox.Full.Width), 0), MovingDir, Hitbox, master);
                    Physics.Velocity.X /= (float)Math.Pow(1.5, master.dt);
                    if (JumpTime < JumpChargeTime / 3)
                        Texture.CurrentFrame = 2;
                    else if (JumpTime < JumpChargeTime / 1.5f)
                        Texture.CurrentFrame = 1;
                    else if (JumpTime < JumpChargeTime)
                        Texture.CurrentFrame = 0;

                    if (Facing == DataTypes.Directions.Right)
                        Texture.SetType(7);
                    else
                        Texture.SetType(6);
                }

                Position += Physics.Velocity * (float)master.dt;
                Position += Physics.KnockbackVelocity * (float)master.dt;
                //Vector2 previousVel = new Vector2(Physics.Velocity.X, Physics.Velocity.Y);
            }
            CheckCollisions(master, ParticleType);



            Hitbox.Update(Position);
        }


        public override void Hurt(float damage, int knockback, Vector2 fromPosition, MasterManager master, double hitTime = 0.8d, string element = "none")
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
                else if (!Knocked)
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

        public override void Update(MasterManager master)
        {

            Attack attack = master.entityManager.Player.PrimaryAttack;
            if (attack is MeleeAttack && attack.CollidesWith(Hitbox.Body))
                Hurt(attack.Damage, attack.Knockback, master.entityManager.Player.Hitbox.Body.Center.ToVector2(), master, attack.HitDuration, attack.Element);
            foreach (ProjectileEntity projectile in master.entityManager.ProjectilesArray)
            {

                if (projectile.PlayerOwned && Hitbox.Body.Intersects(projectile.CollidingRect))
                {
                    Hurt(projectile.Damage, projectile.Knockback, projectile.CollidingRect.Center.ToVector2(), master, projectile.HitTime, projectile.Element);
                    projectile.Pierce -= 1;
                }
            }

            HealthBar.Update(new Vector2(Hitbox.Body.Center.X - 16, Hitbox.Full.Y - 6), master, Stats.Hp);

            if (HitTime > 0f)
                HitTime -= (float)master.timePassed;
            Texture.Update(master.timePassed);
            UpdateAttack(master);
            UpdateMovement(master);
            if (Dead)
                UpdateDeath(master);
            if (Position.Y > master.mapManager.CurrentMap.Bottom)
                Kill(master);


        }
    }
}
