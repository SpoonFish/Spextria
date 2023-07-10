
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
using Spextria.Master;
using Spextria.Statics;

namespace Spextria.Entities
{
    class Arenube : SolidMonsterEntity
    {
        private double TurnTime;
        private double AttackTime;
        private float TornadoSpeed;
        public Arenube(MyTexture texture, Vector2 position, MonsterStats stats, float speed, float thrust, float weight = 2, List<Attack> attacks = null, HitboxObject customHitbox = null, float tornadoSpeed = 1, DataTypes.Directions startingDirection = DataTypes.Directions.Left, DataTypes.Intelligence intelligence = DataTypes.Intelligence.Wander, DataTypes.Hostility hostility = DataTypes.Hostility.Neutral) : base(texture, position, stats, speed, thrust, weight, attacks, null, intelligence, hostility)
        {
            Facing = startingDirection;
            TornadoSpeed = tornadoSpeed;
            CurrentAttack = Attacks.CreateMeleeAttack("none");
            TurnTime = 0;
            AttackTime = 0;
            OriginalIntelligence = intelligence;
            Intelligence = DataTypes.Intelligence.Stationary;
            Hostility = DataTypes.Hostility.Hostile;
            MovingDir = DataTypes.Directions.None;
            Dead = false;
            DeathTime = 0f;
            NeedsToJump = false;
            JumpChargeTime = .25f;
            JumpTime = 0f;
            _TurnTime = 0f;
            Physics = new PhysicsObject(weight, speed, thrust);
            Rectangle full = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            Rectangle body = new Rectangle((int)position.X + 1, (int)position.Y + 2, 30, 30);
            Rectangle upDown = new Rectangle((int)position.X + 2, (int)position.Y, 28, 32);
            Rectangle leftRight = new Rectangle((int)position.X, (int)position.Y + 2, 32, 28);
            Hitbox = new HitboxObject(body, upDown, leftRight, full);
        }

        private void Kill()
        {
            if (Dead)
                return;
            Dead = true;
        }
        private void CheckCollisions(MasterManager master, string particleType)
        {
            return;
        }
        private void CheckNeedsJumping(MasterManager master)
        {
            return;
        }
        private void CheckIntelligence(MasterManager master)
        {
            return;
        }
        private void UpdateMovement(MasterManager master)
        {
            return;
        }



        public override void Update(MasterManager master)
        {

            Attack attack = master.entityManager.Player.PrimaryAttack;
            if (attack is MeleeAttack && attack.CollidesWith(Hitbox.Body))
            {
                if (TurnTime <= 0)
                {
                    if (Facing == DataTypes.Directions.Left)
                        Facing = DataTypes.Directions.Right;
                    else
                        Facing = DataTypes.Directions.Left;
                    TurnTime = 1;
                    Texture.CurrentFrame = 0;

                }
            }
            foreach (ProjectileEntity projectile in master.entityManager.ProjectilesArray)
            {

                if (projectile.PlayerOwned && Hitbox.Body.Intersects(projectile.CollidingRect))
                {

                    if (TurnTime <= 0)
                    {
                        if (Facing == DataTypes.Directions.Left)
                            Facing = DataTypes.Directions.Right;
                        else
                            Facing = DataTypes.Directions.Left;
                        TurnTime = 1;

                    }
                    projectile.Pierce -= 1000;
                }
            }

            HealthBar.Update(new Vector2(Hitbox.Body.Center.X - 16, Hitbox.Full.Y - 6), master, Stats.Hp);


            if (TurnTime <= 0)
            {

                if (Facing == DataTypes.Directions.Left)
                    Texture.SetType(0);
                else
                    Texture.SetType(1);
                AttackTime -= master.timePassed;

                if (AttackTime <= 0)
                {
                    AttackTime = 3.5/TornadoSpeed;
                    if (Facing == DataTypes.Directions.Left)
                        master.entityManager.EnemiesArray.Add(new Tornado(Images.UniqueImage(Images.ImageDict["tornado"]), new Vector2(Position.X - 50, Position.Y - 32), new MonsterStats(100, 100, 100, 100, 1, 0,0,0,1,0,0,1,1,1,1,1,1,1,1), 2, 1, 2, new List<Attack>() { Attacks.CreateMeleeAttack("none") }, null, TornadoSpeed, Facing));
                    else
                        master.entityManager.EnemiesArray.Add(new Tornado(Images.UniqueImage(Images.ImageDict["tornado"]), new Vector2(Position.X +22, Position.Y - 32), new MonsterStats(100, 100, 100, 100, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1), 2, 1, 2, new List<Attack>() { Attacks.CreateMeleeAttack("none") }, null, TornadoSpeed, Facing));
                }
                if (AttackTime <= 0.5)
                {
                    if (Facing == DataTypes.Directions.Left)
                        Texture.SetType(4);
                    else
                        Texture.SetType(5);
                }
                else if (AttackTime <= 1)
                {
                    if (Facing == DataTypes.Directions.Left)
                        Texture.SetType(6);
                    else
                        Texture.SetType(7);
                }
                Texture.Update(master.timePassed);
            }
            else
            {
                if (Facing == DataTypes.Directions.Left)
                    Texture.SetType(2);
                else
                    Texture.SetType(3);
                AttackTime = 4/TornadoSpeed;
                TurnTime -= master.timePassed;
                Texture.CurrentFrame = (int)Math.Round(Math.Min(3, TurnTime * 4));
            }




        }
    }
}
