
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
    class Tornado : SolidMonsterEntity
    {
        private float TornadoSpeed;
        private float Opacity;
        private int CollideCounter;
        private double LifeTime;
        public Tornado(MyTexture texture, Vector2 position, MonsterStats stats, float speed, float thrust, float weight = 2, List<Attack> attacks = null, HitboxObject customHitbox = null, float tornadoSpeed = 0, DataTypes.Directions direction = DataTypes.Directions.Left, DataTypes.Intelligence intelligence = DataTypes.Intelligence.Wander, DataTypes.Hostility hostility = DataTypes.Hostility.Neutral) : base(texture, position, stats, speed, thrust, weight, attacks, null, intelligence, hostility)
        {
            TornadoSpeed = tornadoSpeed;
            LifeTime = 0;
            CollideCounter = 0;
            MovingDir = direction;
            OriginalIntelligence = intelligence;
            Intelligence = intelligence;
            Hostility = hostility;
            Dead = false;
            DeathTime = 0f;
            NeedsToJump = false;
            JumpChargeTime = .25f;
            Opacity = 0;
            JumpTime = 0f;
            _TurnTime = 0f;
            Physics = new PhysicsObject(weight, speed, thrust);
            if (customHitbox == null)
            {
                Rectangle full = new Rectangle((int)position.X+20, (int)position.Y+60, 20, 20);
                Rectangle body = new Rectangle((int)position.X + 21, (int)position.Y + 62, 18, 18);
                Rectangle upDown = new Rectangle((int)position.X + 22, (int)position.Y+60, 18, 20);
                Rectangle leftRight = new Rectangle((int)position.X +20, (int)position.Y + 62, 20, 18);
                Hitbox = new HitboxObject(body, upDown, leftRight, full);

            }
            else
                Hitbox = customHitbox;
        }

        private void Kill()
        {
            if (Dead)
                return;
            Dead = true;
        }
        private void CheckCollisions(MasterManager master)
        {
            Physics.Falling = true;
            foreach (Rectangle rect in master.mapManager.CurrentMap.Collisions)
            {
                if (Hitbox.UpDown.Intersects(rect))
                {
                    float oldY = (float)Math.Round(Position.Y, 2);
                    Position.Y = rect.Y - 80;
                    if (Math.Abs(Position.Y - oldY) > 38)
                    {
                        LifeTime = Math.Max(3.75, LifeTime);
                        Position.Y = oldY;
                    }
                    CollideCounter = 3;
                }
                else if (Position.Y == rect.Y -80 && new Rectangle(Hitbox.UpDown.X, Hitbox.UpDown.Y, Hitbox.UpDown.Width, Hitbox.UpDown.Height+2).Intersects(rect))
                {
                    CollideCounter = 3;
                }
            }
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
            if (MovingDir == DataTypes.Directions.Left)
                Position.X -= 1*TornadoSpeed;
            else
                Position.X += 1*TornadoSpeed;
            if (CollideCounter < 0)
                Position.Y += 2*TornadoSpeed;
        }



        public override void Update(MasterManager master)
        {
            LifeTime += master.timePassed;

            if (LifeTime < 0.5)
                Opacity = (float)LifeTime * 2;
            else if (LifeTime > 4)
            {
                master.entityManager.EnemiesArray.Remove(this);
                return;
            }
            else if (LifeTime > 3.75)
                Opacity = (float)((-1 * (LifeTime - 3.75) + 0.25) * 4);


            Hitbox.Update(new Vector2(Position.X + 20, Position.Y + 60));
            Hitbox.Body = new Rectangle((int)Position.X + 10, (int)Position.Y+20, 40, 50);

            if (master.entityManager.Player.Hitbox.Body.Intersects(Hitbox.Body))
            {
                master.entityManager.Player.Physics.Accel.Y = -5;
                if (MovingDir == DataTypes.Directions.Right)
                    master.entityManager.Player.Physics.Velocity.X += 2;
                else
                    master.entityManager.Player.Physics.Velocity.X += -2;

            }

            CollideCounter -= 1;
            UpdateMovement(master);
            CheckCollisions(master);
            Texture.Update(master.timePassed);

        }


        public override void Draw(SpriteBatch spriteBatch, float mapOpacity = 1)
        {
            //Images.ImageDict["button_red"].Draw(spriteBatch, new Vector2(rect2.X, rect2.Y), 0.5F, new Vector2(rect2.Width, rect2.Height));

            //Images.ImageDict["button_red"].Draw(spriteBatch, new Vector2(rect1.X, rect1.Y), 0.5F, new Vector2(rect1.Width, rect1.Height));

            if (Stats.Hp < Stats.MaxHp && !Dead)
                HealthBar.Draw(spriteBatch);
            if (!Dead)
            {
                Texture.Draw(spriteBatch, Position, Opacity * mapOpacity);
                CurrentAttack.Draw(spriteBatch);
            }
        }
    }
}
