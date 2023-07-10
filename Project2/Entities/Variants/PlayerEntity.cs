using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Spextria;
using Spextria.Entities;
using Spextria.Entities.AttackObjects;
using Spextria.Graphics;
using Spextria.Maps.LevelObjectTypes;
using Spextria.Master;
using Spextria.Statics;

namespace Spextria.Entities
{
    class PlayerEntity : MovingEntity
    {
        private KeyboardState _prevKey;
        private KeyboardState _currentKey;
        public HitboxObject Hitbox;
        public PlayerStats Stats;
        public int CurrentLevel;
        public bool Dead;
        private bool Win;
        public bool Inactive;
        private float JumpChargeTime;
        private float DeathTime;
        private float WinTime;
        private float SpaceTime;
        private double HitTime;
        public string CurrentPlanet;
        public string ParticleType;
        //public string CurrentPlanet;
        public PhysicsObject Physics;


        public Attack PrimaryAttack;
        public PlayerEntity(MyTexture texture, Vector2 position, PlayerStats stats, float speed, float thrust, bool collides = true, float weight = 2) : base(texture, position, speed, thrust, collides)
        {
            PrimaryAttack = Attacks.CreateMeleeAttack("steel_fists");
            Win = false;
            WinTime = 0;
            Stats = stats;
            Dead = false;
            Inactive = false;
            DeathTime = 0f;
            JumpChargeTime = 0f;
            SpaceTime = 0f;
            CurrentLevel = 1;
            CurrentPlanet = "luxiar";
            Physics = new PhysicsObject(weight, speed, thrust);
            Rectangle full = new Rectangle((int)position.X, (int)position.Y, texture.GetWidth(), texture.GetHeight());
            Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 1, texture.GetWidth()-2, texture.GetHeight()-1);
            Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, texture.GetWidth(), texture.GetHeight() - 4);
            Rectangle upDown = new Rectangle((int)position.X + 4, (int)position.Y, texture.GetWidth() - 8, texture.GetHeight());
            Hitbox = new HitboxObject(body, upDown, leftRight, full);
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
        public void Hurt(float damage, int knockback, Vector2 fromPosition, MasterManager master, double hitTime = 0.8d)
        {
            if (HitTime > 0 || Dead)
                return;

            double angle = Functions.Bearing(fromPosition, Hitbox.Body.Center.ToVector2());
            //master.playGuiManager.SetDebugVal(angle.ToString());
            Physics.Knockback(angle, knockback, Stats.KnockbackResistance);

            HitTime = hitTime;
            Stats.Hp = Math.Max(0, Stats.Hp - damage);
            if (Stats.Hp == 0)
                Kill(master);
        }
        private void Kill(MasterManager master)
        {
            Stats.Hp = 0;
            if (Dead)
                return;
            Inactive = true;
            Dead = true;
            master.mapManager.CurrentMapDrawn = false;
            master.CurrentSpriteEffect = Effects.Greyscale;
            master.CurrentMapEffect = Effects.MapGreyscale;
        }
        private void CheckRect(Rectangle rect, MasterManager master, string particleType)
        {
            if (rect.Intersects(Hitbox.UpDown))
            {
                SpaceTime = 0;
                Physics.Accel.Y = 0;
                if (Position.Y < rect.Y)
                {
                    if (Math.Abs((rect.Top - Hitbox.UpDown.Height) - Position.Y) > 22)
                        return;
                    Position.Y = rect.Top - Hitbox.UpDown.Height;
                    Physics.Falling = false;
                    Physics.OtherFalling = false;
                    if (!Dead)
                        Particles.CreateJumpParticles(ParticleType, Position, Hitbox, master);
                }
                else if (Physics.Velocity.Y < 0)
                {
                    if (Math.Abs(rect.Bottom - Position.Y) > 22)
                        return;
                    Position.Y = rect.Bottom;
                    Physics.OtherFalling = true;
                    Physics.AirTime = 0.4f;
                    SpaceTime = JumpChargeTime;
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
                    if (Position.X < rect.X)
                        Position.X = rect.Left - Hitbox.LeftRight.Width;
                    else
                        Position.X = rect.Right;
                }

            }
        }
        private void CheckCollisions(Rectangle[] collisions, MasterManager master, string particleType)
        {
            Physics.Falling = true;
            foreach (Rectangle rect in collisions)
            {
               CheckRect(rect, master, particleType);
            }
            foreach (Gate gate in master.mapManager.CurrentMap.Gates)
            {
                if (gate.State == "on")
                    CheckRect(gate.Rect, master, particleType);
            }

            if (!(Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)))
                foreach (Rectangle rect in master.mapManager.CurrentMap.Semisolids)
                {
                    if (rect.Y+8 < Hitbox.Feet.Bottom || !Physics.Falling || Physics.Velocity.Y <-2)
                        continue;
                    if (rect.Intersects(Hitbox.UpDown))
                    {
                        Physics.Accel.Y = 0;
                        if (Physics.Velocity.Y > 0 && Physics.Falling || rect.Y < Hitbox.Feet.Bottom)
                            SpaceTime = 0;
                        Position.Y = rect.Top - Hitbox.UpDown.Height;
                        Physics.Falling = false;
                        Physics.OtherFalling = false;

                        if (!Dead)
                            Particles.CreateJumpParticles(ParticleType, Position, Hitbox, master);

                    }
                    else
                    {
                        Rectangle inGroundRect = new Rectangle(rect.X, rect.Y - 1, rect.Width, 8);
                        if (inGroundRect.Intersects(Hitbox.UpDown))
                            Physics.Falling = false;
                    }
                }
        }

        public void Reset(MasterManager master)
        {
            Stats.Reset();
            Stats.ApplyBuffs(master);
            Dead = false;
            Win = false;
            Inactive = false;
            DeathTime = 0f;
            Physics.Accel = Vector2.Zero;
            Physics.Velocity = Vector2.Zero;
            Physics.KnockbackVelocity = Vector2.Zero;
            Physics.Jumping = false;
            Physics.Ground();
            SpaceTime = 0f;
            HitTime = 0f;
        }

        private void UpdateMovement(MasterManager master)
        {
            Rectangle[] collisions = master.mapManager.CurrentMap.Collisions;
            _prevKey = _currentKey;
            _currentKey = Keyboard.GetState();
            bool leftPressed = _currentKey.IsKeyDown(Keys.A) || _currentKey.IsKeyDown(Keys.Left);
            bool rightPressed = _currentKey.IsKeyDown(Keys.D) || _currentKey.IsKeyDown(Keys.Right);
            bool upPressed = _currentKey.IsKeyDown(Keys.W) || _currentKey.IsKeyDown(Keys.Up);
            bool downPressed = _currentKey.IsKeyDown(Keys.S) || _currentKey.IsKeyDown(Keys.Down);
            bool keyPressed = false;

            DataTypes.Directions movingDir = DataTypes.Directions.None;
            keyPressed = false;
            if (leftPressed)
            {
                movingDir = DataTypes.Directions.Left;
                keyPressed = true;
            }
            else if (rightPressed)
            {
                movingDir = DataTypes.Directions.Right;
                keyPressed = true;
            }
            if (leftPressed && rightPressed || Inactive)
            {
                movingDir = DataTypes.Directions.None;
                keyPressed = false;
            }

            ParticleType = master.mapManager.GetGroundType(Hitbox.Body).ParticleType;

            if ((_currentKey.IsKeyDown(Keys.Space) || _currentKey.IsKeyDown(Keys.W) || (SpaceTime > 0 && SpaceTime < JumpChargeTime)) && !Inactive && !(PrimaryAttack.Heavy && (PrimaryAttack.Charging || PrimaryAttack.Active)))
            {
                SpaceTime += (float)master.timePassed;
                if (SpaceTime > JumpChargeTime)
                    Physics.UpdateJump((float)master.dt);
            }
            else
                Physics.Jumping = false;

            Physics.UpdateMovement((float)master.dt, (float)master.timePassed);

            if (movingDir == DataTypes.Directions.Left && !master.mapManager.CurrentMap.LevelCompleted())
            {
                // change vel and accel to move
                Physics.Accel.X /= (float)Math.Pow(2, master.dt);
                Physics.Velocity.X -= Physics.Accel.X * (float)master.dt;

                // if skidding make some funny particles

                if (!Physics.Falling && Physics.Velocity.X > 0 && !Dead)
                {
                    Particles.CreateSkidParticles(ParticleType, Position + new Vector2(new Random().Next(0, Hitbox.Full.Width), 0), movingDir, Hitbox, master);
                }


                Facing = DataTypes.Directions.Left;
                Texture.SetType(2);
            }
            else if (Facing == DataTypes.Directions.Left && Physics.Velocity.X == 0)
            {
                Texture.SetType(0);
            }

            if (movingDir == DataTypes.Directions.Right || master.mapManager.CurrentMap.LevelCompleted())
            {
                Physics.Accel.X /= (float)Math.Pow(2, master.dt);
                Physics.Velocity.X += Physics.Accel.X * (float)master.dt;

                if (!Physics.Falling && Physics.Velocity.X < 0 && !Dead)
                {
                    Particles.CreateSkidParticles(ParticleType, Position + new Vector2(new Random().Next(0, Hitbox.Full.Width), 0), movingDir, Hitbox, master);
                }

                Facing = DataTypes.Directions.Right;
                Texture.SetType(3);
            }
            else if (Facing == DataTypes.Directions.Right && Physics.Velocity.X == 0)
            {
                Texture.SetType(1);
            }

            if (!keyPressed)
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

            if (!Physics.Falling && Physics.Velocity.X != 0 && !Dead)
            {
                // walk particles when moving and on ground
                Particles.CreateWalkParticles(ParticleType, Position, movingDir, Hitbox, master);
            }

            if (Physics.OtherFalling && SpaceTime > JumpChargeTime)
            {
                if (Facing == DataTypes.Directions.Right)
                    Texture.SetType(5);
                else
                    Texture.SetType(4);
            }

            if (PrimaryAttack.Active && PrimaryAttack.Name != "none")
            {
                Physics.Velocity.X *= (float)Math.Pow(PrimaryAttack.SpeedMult, master.dt);
                if (Physics.Falling && Physics.Velocity.Y > 0)
                    Physics.Velocity.Y += 0.4f * (float)master.dt;
                if (PrimaryAttack.Direction < 180)
                    Texture.SetType(9);
                else
                    Texture.SetType(8);
                Texture.CurrentFrame = (int)Math.Floor(4f * ((float)PrimaryAttack.Stage /((float)PrimaryAttack.Stages+1f)));
            }
            else if (PrimaryAttack.Charging && PrimaryAttack.Name != "none")
            {
                Physics.Velocity.X /= (float)Math.Pow(1.25, master.dt);
                if (PrimaryAttack.Direction < 180)
                    Texture.SetType(11);
                else
                    Texture.SetType(10);
                Texture.CurrentFrame = (int)Math.Floor(4f * ((float)PrimaryAttack.ChargeTimeActive / ((float)PrimaryAttack.ChargeTime)));
            
            }



            else if (SpaceTime > 0 && SpaceTime < JumpChargeTime && !Dead)
            {
                Particles.CreateSkidParticles(ParticleType, Position, movingDir, Hitbox, master);
                Physics.Velocity.X /= (float)Math.Pow(1.5, master.dt);
                if (SpaceTime < JumpChargeTime / 3)
                    Texture.CurrentFrame = 2;
                else if (SpaceTime < JumpChargeTime / 1.5f)
                    Texture.CurrentFrame = 1;
                else if (SpaceTime < JumpChargeTime)
                    Texture.CurrentFrame = 0;

                if (Facing == DataTypes.Directions.Right)
                    Texture.SetType(7);
                else
                    Texture.SetType(6);
            }
            if (master.mapManager.CurrentMap.LevelCompleted())
                Physics.Velocity.X /= 1.12f; //slow walk when finish level
            if (Dead)
                Physics.Velocity.X *= 1.04f;

            Position += Physics.Velocity * (float)master.dt;
            Position += Physics.KnockbackVelocity * (float)master.dt;
            //Vector2 previousVel = new Vector2(Physics.Velocity.X, Physics.Velocity.Y);
            CheckCollisions(collisions, master, ParticleType);


            if (Texture.GetHeight() == 24)
            {
                //Hitbox.Update(Position, Texture, deltaHeight: -8);
                //if (Physics.OtherFalling || Physics.Velocity.X > Physics.Accel.X * 2 || Physics.Velocity.X < -Physics.Accel.X * 2)
                //    Hitbox.Update(Position, Texture, deltaHeight: -8, deltaHeightBody: -8);
            }
            else
                Hitbox.Update(Position);
        }

        private void UpdateDeath(MasterManager master)
        {
            if (DeathTime == 0)
                Particles.CreateParticles(master.entityManager, 10, new Vector2(Position.X + 6, Position.Y + 16), "player explode", 0, "", Physics.Velocity);
            DeathTime += (float)master.timePassed;
            if (DeathTime < 0.3f * master.dt)
            {
                Particles.CreateParticles(master.entityManager, 1, new Vector2(Position.X + 6, Position.Y + 16), "player fragments", 0, "", Physics.Velocity);
                Particles.CreateParticles(master.entityManager, 1, new Vector2(Position.X + 6, Position.Y + 16), "player explode", 0, "", Physics.Velocity);
            }
            else if (DeathTime > 2.1f && DeathTime < 10f)
            {
                DeathTime = 10f;
                master.playGuiManager.StartFadeoutSequence();
            }
            else if (DeathTime > 11f && DeathTime < 20f)
            {
                DeathTime = 20f;
                master.levelFailed = true;
            }
            else if (DeathTime > 20.5f && DeathTime < 30f)
            {
                DeathTime = 20f;
                master.playGuiManager.ResetFadeoutSequence();
            }
        }
        private void UpdateWin(MasterManager master)
        {
            WinTime += (float)master.timePassed;
            if (WinTime > 2.1f && WinTime < 10f)
            {
                WinTime = 10f;
                master.playGuiManager.StartFadeoutSequence();
            }
            else if (WinTime > 11f && WinTime < 20f)
            {
                WinTime = 20f;
                master.levelWon = true;
            }
            else if (WinTime > 20.5f && WinTime < 30f)
            {
                WinTime = 0f;
                master.playGuiManager.ResetFadeoutSequence();
            }
        }
        public void Update(MasterManager master)
        {
            PrimaryAttack.Update(master, new Vector2(Hitbox.Body.Center.X, Hitbox.Body.Center.Y), Physics.Velocity+Physics.KnockbackVelocity);

            foreach (MonsterEntity monster in master.entityManager.EnemiesArray)
            {
                if (monster.CurrentAttack == null)
                    continue;
                Attack attack = monster.CurrentAttack;
                if (attack is MeleeAttack && attack.CollidesWith(Hitbox.Body))
                    Hurt(attack.Damage, attack.Knockback, monster.Hitbox.Body.Center.ToVector2(), master, attack.HitDuration);
            }
            foreach (ProjectileEntity projectile in master.entityManager.ProjectilesArray)
            {
                if (!projectile.PlayerOwned && Hitbox.Body.Intersects(projectile.CollidingRect))
                {
                    bool reflect = false;
                    if (reflect && projectile.ExplodingTime == 0)
                    {
                        projectile.Reflect(Physics.Velocity);
                    }
                    else
                    {

                        Hurt(projectile.Damage, projectile.Knockback, projectile.CollidingRect.Center.ToVector2(), master, projectile.HitTime);
                        projectile.Pierce -= 1;
                    }
                }
            }

            if (master.CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (PrimaryAttack.Melee)
                {
                    if (master.storedDataManager.Settings.AttackTowardsMouse)
                        if (master.MousePos.X >= 213)
                            PrimaryAttack.Strike(90, master);
                        else
                            PrimaryAttack.Strike(270, master);
                    else
                        if (Facing == DataTypes.Directions.Right)
                            PrimaryAttack.Strike(90, master);
                        else
                            PrimaryAttack.Strike(270, master);
                }
                else
                {

                    double angle = Functions.Bearing(Hitbox.Body.Center.ToVector2()-master.cameraManager.Camera.Position, master.MousePos.ToVector2());
                    PrimaryAttack.Strike(angle, master);
                }
            }


            if (master.mapManager.CurrentMap.LevelCompleted())
            {
                Win = true;
                Dead = false;
                Stats.Hp = Stats.MaxHp;
            }
            if (HitTime > 0f)
                HitTime -= master.timePassed;
            _currentKey = Keyboard.GetState();
            bool upPressed = _currentKey.IsKeyDown(Keys.W) || _currentKey.IsKeyDown(Keys.Up);
            bool downPressed = _currentKey.IsKeyDown(Keys.S) || _currentKey.IsKeyDown(Keys.Down);
            UpdateMovement(master);
            if (Win)
                UpdateWin(master); 
            else if (Dead)
                UpdateDeath(master);
            if (downPressed)
            {
                Stats.Hp = Stats.MaxHp;
                Stats.Se = Stats.MaxSe;
                master.CurrentSpriteEffect = null;
                master.CurrentMapEffect = null;
                Dead = false;
                Inactive = false;
                DeathTime = 0;
            }
            if (Position.Y > master.mapManager.CurrentMap.Bottom)
                Kill(master);
        }

        public override void Draw(SpriteBatch spriteBatch, float mapOpacity =1)
        {

            float opacity = 1f;
            if (!Dead)
            {
                if (HitTime > 0 && (int)Math.Round(HitTime * 10) % 2 == 0)
                    opacity = 0.5f;
                Texture.Draw(spriteBatch, Position, opacity);
                PrimaryAttack.Draw(spriteBatch);
            }
        }
    }
}
