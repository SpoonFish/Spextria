using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Spextria.Statics;

namespace Spextria.Entities
{
    class PhysicsObject
    {
        public Vector2 Velocity;
        public bool Falling;
        public bool OtherFalling;
        public Vector2 Accel;
        public Vector2 OrigAccel;
        public bool Jumping;
        public bool PotentiallyIllegal;
        public float Weight;
        public float Thrust;
        private float KnockbackHitStrength;
        public float AirTime;
        private Vector2 TerminalVelocity;
        public Vector2 KnockbackVelocity;
        private Vector2 OrigTerminalVelocity;

        public PhysicsObject(float weight = 2f, float speed = 2f, float thrust = 3f)
        {
            KnockbackHitStrength = 0;
            KnockbackVelocity = new Vector2(0, 0);
            Jumping = false;
            AirTime = 0f;
            Weight = weight;
            Thrust = thrust;
            Falling = false;
            OtherFalling = false;
            Velocity = new Vector2(0,0);
            PotentiallyIllegal = false;
            Accel.Y = 0f;
            Accel.X = speed / 10 * Weight;
            OrigAccel = Accel;
            OrigTerminalVelocity.Y =4f * Weight;
            OrigTerminalVelocity.X = speed;
            TerminalVelocity.Y = 4f * Weight;
            TerminalVelocity.X = speed;
        }
        public void UpdateJump(float dt)
        {
            if (AirTime < 0.3)
            {
                if (!Falling)
                {
                    Velocity.Y = -0.8f * Thrust;
                    Accel.Y = -1.1f * Thrust;
                }
                Velocity.Y += -0.05f * Thrust * dt * dt;
                Jumping = true;
                Falling = true;
                OtherFalling = true;
            }
            else if (AirTime > 0.05)
                Jumping = false;
        }

        public void UpdateMovement(float dt, float timePassed)
        {
            if (KnockbackVelocity.Y < -1)
                Jumping = false;
            if (Falling)
            {
                AirTime += timePassed;
                if (!Jumping)
                {
                    // goofy gravity
                    Accel.Y += 0.1f * Weight * dt;// * (1+AirTime);

                    // y velocity decreases faster when your not moving upwards for an interesting jump path
                    if (Velocity.Y > -2)
                        Accel.Y += 0.05f * Weight * dt;
                    if (Velocity.Y > 0)
                        Accel.Y += 0.05f * Weight * dt;
                }
            }
            else
                AirTime = 0;

            
            Velocity.Y += Accel.Y * dt;

            // natural decrease of y velocity
            Velocity.Y /= (float)Math.Pow(2, dt);
            
            // stop velocity going above terminal velocity

            if (Velocity.Y > TerminalVelocity.Y)
                Velocity.Y = TerminalVelocity.Y;
            //Velocity.Y = TerminalVelocity.Y;

            else if (Velocity.Y < -TerminalVelocity.Y)
                Velocity.Y = -TerminalVelocity.Y;


            if (Velocity.X > TerminalVelocity.X)
                Velocity.X = TerminalVelocity.X;

            else if (Velocity.X < -TerminalVelocity.X)
                Velocity.X = -TerminalVelocity.X;

            if (KnockbackVelocity.X > 0.1f)
            {
                KnockbackVelocity.X *= (float)Math.Pow(0.97, dt);// * (float)Math.Pow(2, dt);
                KnockbackVelocity.X -= 0.01f * dt;
                Velocity.X /= (float)Math.Pow((1 + KnockbackHitStrength/25f), dt);
            }
            else if (KnockbackVelocity.X < -0.1f)
            {
                KnockbackVelocity.X *= (float)Math.Pow(0.97, dt);// * (float)Math.Pow(2, dt);
                KnockbackVelocity.X += 0.01f * dt;
                Velocity.X /= (float)Math.Pow((1 + KnockbackHitStrength / 25f), dt);
            }
            else
                KnockbackVelocity.X = 0;

            if (KnockbackVelocity.Y > 0.1f)
            {
                KnockbackVelocity.Y *= (float)Math.Pow(0.95, dt);// * (float)Math.Pow(2, dt);
            }
            else if (KnockbackVelocity.Y < -0.1f)
            {
                KnockbackVelocity.Y *= (float)Math.Pow(0.95, dt);// * (float)Math.Pow(2, dt);
            }
            else
                KnockbackVelocity.Y = 0;

            if (KnockbackVelocity.Y + Velocity.Y > TerminalVelocity.Y)
                KnockbackVelocity.Y = Math.Max(0,TerminalVelocity.Y-Velocity.Y);

            Accel.X = OrigAccel.X;

        }

        public void Knockback(double angle, float knockback= 5, float knockbackResistance = 0)
        {
            if (knockbackResistance == 1)
                return;
            knockback *= knockbackResistance * -1 + 1;
            float xMult = (float)Math.Sin(Functions.DegToRadians(angle)) * 0.7f;
            float yMult = (float)Math.Cos(Functions.DegToRadians(angle)) * 0.4f;
            Velocity /= knockback+1;
            Accel /= knockback + 1;
            KnockbackVelocity = new Vector2(xMult * knockback, yMult * knockback - knockback/2);
            KnockbackHitStrength = knockback;
            Falling = true;
        }

        public void Ground()
        {
            OtherFalling = false;
            Falling = false;
            Velocity.Y = 0f;
            Accel.Y = 0f;

        }
    }
}
