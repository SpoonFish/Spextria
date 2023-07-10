using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Spextria;
using Spextria.Entities.AttackObjects;
using Spextria.Entities.EntityParts;
using Spextria.Graphics;
using Spextria.Graphics.GUI.PlayScreen;
using Spextria.Maps.LevelObjectTypes;
using Spextria.Master;
using Spextria.Statics;

namespace Spextria.Entities
{
    class WormEntity : MonsterEntity
    {
        private List<WormSegment> Segments;
        private double _RefreshTime;
        private double _TurnTime;
        private Vector2 OldPos;
        private bool Colliding;
        private float Weight;
        private int Length;
        public double Rotation;
        public WormEntity(MyTexture texture, Vector2 position, MonsterStats stats, float speed, float thrust, float weight = 2, List<Attack> attacks = null, int length = 1, MyTexture bodyImage = null, MyTexture tailImage= null, HitboxObject customHitbox = null, DataTypes.Intelligence intelligence = DataTypes.Intelligence.Wander, DataTypes.Hostility hostility = DataTypes.Hostility.Neutral) : base(texture, position, stats, speed, thrust, weight, attacks, intelligence, hostility)
        {
            Length = length;
            Weight = weight;
            OldPos = Vector2.Zero;
            _RefreshTime = 0;
            Rotation = 180;
            Colliding = true;
            OriginalIntelligence = intelligence;
            Intelligence = intelligence;
            Hostility = hostility;
            MovingDir = DataTypes.Directions.None;
            Dead = false;
            DeathTime = 0f;
            Physics = new PhysicsObject(weight, speed, thrust);
            if (customHitbox == null)
            {
                Rectangle full = new Rectangle((int)position.X, (int)position.Y, texture.GetWidth(), texture.GetHeight());
                Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 1, texture.GetWidth() - 2, texture.GetHeight() - 1);
                Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, texture.GetWidth(), texture.GetHeight() - 4);
                Rectangle upDown = new Rectangle((int)position.X + 4, (int)position.Y, texture.GetWidth() - 8, texture.GetHeight());
                Hitbox = new HitboxObject(body, upDown, leftRight, full);

            }
            else
                Hitbox = customHitbox;

            Segments = new List<WormSegment>();
            Segments.Add(new WormSegment(new Rectangle(0, 0, texture.GetWidth(), texture.GetHeight()), tailImage, Position));
            for (int i = 0; i < Length; i ++)
                Segments.Add(new WormSegment(new Rectangle(0, 0, texture.GetWidth(), texture.GetHeight()), bodyImage, Position));

        }

        private void Kill()
        {
            if (Dead)
                return;
            Dead = true;
        }
        private void CheckRect(Rectangle rect, MasterManager master, string particleType)
        {
            if (rect.Intersects(Hitbox.Body))
            { 
                Colliding = true;
            }
        }
        private void CheckCollisions(MasterManager master, string particleType)
        {
            Colliding = false;
            foreach (Rectangle rect in master.mapManager.CurrentMap.Collisions)
            {
                CheckRect(rect, master, particleType);
            }
            foreach (Gate gate in master.mapManager.CurrentMap.Gates)
            {
                if (gate.State == "on")
                    CheckRect(gate.Rect, master, particleType);
            }

        }
        
        private void CheckIntelligence(MasterManager master)
        {
            switch (Intelligence)
            {
                case DataTypes.Intelligence.Worm1: //always moving
                    _TurnTime -= (float)master.timePassed;
                    if (_TurnTime < 0)
                    {
                        //Position = master.entityManager.Player.Position;
                        Random rand = new Random();
                        _TurnTime = rand.Next(0, 2);
                        int num = rand.Next(1, 3);
                        if (num == 1)
                            MovingDir = DataTypes.Directions.Left;
                        else if (num == 2)
                            MovingDir = DataTypes.Directions.Right;
                        else
                            if (!(Rotation >90 && Rotation < 270))
                                MovingDir = DataTypes.Directions.None;
                    }
                    break;
                case DataTypes.Intelligence.Approach: //always moving
                    _TurnTime -= (float)master.timePassed;
                    if (_TurnTime < 0)
                    {
                        //Position = master.entityManager.Player.Position;
                        Random rand = new Random();
                        _TurnTime = rand.Next(0, 1);
                        MovingDir = DataTypes.Directions.None;
                        if (Math.Abs(master.entityManager.Player.Hitbox.Body.Center.X - Hitbox.Body.Center.X) < 48)
                            return;
                        if (Hitbox.Full.Center.X < master.entityManager.Player.Hitbox.Full.Center.X)
                        {

                            if (Hitbox.Full.Center.Y < master.entityManager.Player.Hitbox.Full.Center.Y)
                            {
                                if (Rotation < 45)
                                    MovingDir = DataTypes.Directions.Right;
                                else
                                    MovingDir = DataTypes.Directions.Left;

                            }
                            else
                            {
                                if (Rotation < 135)
                                    MovingDir = DataTypes.Directions.Right;
                                else
                                    MovingDir = DataTypes.Directions.Left;

                            }
                        }
                        else
                        {

                            if (Hitbox.Full.Center.Y < master.entityManager.Player.Hitbox.Full.Center.Y)
                            {
                                if (Rotation < 315)
                                    MovingDir = DataTypes.Directions.Right;
                                else
                                    MovingDir = DataTypes.Directions.Left;

                            }
                            else
                            {
                                if (Rotation < 225)
                                    MovingDir = DataTypes.Directions.Right;
                                else if (Rotation > 250)
                                    MovingDir = DataTypes.Directions.Left;

                            }
                        }
                    }
                    break;
            }
        }
        private void UpdateMovement(MasterManager master)
        {
            CheckCollisions(master, ParticleType);
            if (Colliding)
            {
                CheckIntelligence(master);
                if (MovingDir == DataTypes.Directions.Left)
                {
                    Rotation -= 1.9 * master.dt;
                }
                else if (MovingDir == DataTypes.Directions.Right)

                {
                    Rotation += 1.9 * master.dt;
                }

                double radians = Functions.DegToRadians(Rotation);
                float xMult = (float)Math.Sin(radians);
                float yMult = (float)Math.Cos(radians);
                Physics.Velocity = new Vector2(Speed * xMult, Speed * yMult);
                OldPos = Position;
                Position += Physics.Velocity * (float)master.dt;
                //Vector2 previousVel = new Vector2(Physics.Velocity.X, Physics.Velocity.Y);

            }
            else
            {

                _RefreshTime += master.timePassed;

                Position += Physics.Velocity * (float)master.dt*1.5f;

                Physics.Velocity.Y += 0.01f * Weight * (float)master.dt;
                if (_RefreshTime > 0.09f)
                {
                    _RefreshTime = 0;
                    Rotation = Functions.Bearing(OldPos, Position);
                    OldPos = Position;

                }
            }


            Hitbox.Update(Position);
        }



        public override void Update(MasterManager master)
        {
            //master.playGuiManager.SetDebugVal(Rotation.ToString());

            for (int i = Segments.Count - 1; i >= 0; i--)
            {
                if (i == Segments.Count - 1)
                {
                    double radians = Functions.DegToRadians(Rotation+180);
                    float xMult = (float)Math.Sin(radians);
                    float yMult = (float)Math.Cos(radians);
                    Segments[i].FollowPosition = Position + new Vector2(16 * xMult, 16 * yMult);
                }
                else
                {
                    double radians = Functions.DegToRadians(Segments[i + 1].Rotation+180);
                    float xMult = (float)Math.Sin(radians);
                    float yMult = (float)Math.Cos(radians);
                    Segments[i].FollowPosition = Segments[i + 1].Position + new Vector2(16 * xMult, 16 * yMult);
                }
                Segments[i].Update(master);
            }

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
                Kill();


        }

        public override void Draw(SpriteBatch spriteBatch, float mapOpacity = 1)
        {
            if (Stats.Hp < Stats.MaxHp && !Dead)
                HealthBar.Draw(spriteBatch);
            float opacity = 1f;
            if (!Dead)
            {
                if (HitTime > 0 && (int)Math.Round(HitTime * 10) % 2 == 0)
                    opacity = 0.5f;

                foreach (WormSegment segment in Segments)
                    segment.Draw(spriteBatch, mapOpacity*opacity);
                Texture.Draw(spriteBatch, Position+new Vector2(16,16), opacity * mapOpacity, new Vector2(Texture.GetHeight(), Texture.GetWidth()), -(float)Functions.DegToRadians(Rotation-180));
                CurrentAttack.Draw(spriteBatch);
            }
        }
    }
}
