
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using Spextria.Master;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.Entities;

namespace Spextria.Statics
{
    static class Particles
    {

        public static void CreateSkidParticles(string particleType, Vector2 position, DataTypes.Directions direction, HitboxObject hitbox, MasterManager master)
        {
            if (particleType != "none" && particleType != null)
            {

                Random random = new Random();
                int amount = 0;
                if (particleType == "sparks" || particleType == "white_flecks")
                    amount = (int)Math.Round((float)random.NextSingle(0f, 0.65f) * master.dt);
                else
                    amount = (int)Math.Round((float)random.NextSingle(2f, 2.5f) * master.dt);
                if (direction == DataTypes.Directions.Left)
                    CreateParticles(master.entityManager, amount, new Vector2(position.X + 7, position.Y - 2 +(hitbox.Full.Bottom - position.Y)), "dust right", 1, particleType);
                else
                    CreateParticles(master.entityManager, amount, new Vector2(position.X + 7, position.Y - 2 +(hitbox.Full.Bottom - position.Y)), "dust left", 1, particleType);
            }
        }

        public static void CreateWalkParticles(string particleType, Vector2 position, DataTypes.Directions movingDir, HitboxObject hitbox, MasterManager master)
        {

            if (particleType != "" && particleType != null)
            {
                Random random = new Random();
                int amount;
                if (particleType == "sparks")
                    amount = (int)Math.Round((float)random.NextSingle(0f, 0.6f) * master.dt);
                else
                    amount = (int)Math.Round((float)random.NextSingle(0.6f, 0.9f) * master.dt);
                CreateParticles(master.entityManager, amount, new Vector2(position.X, position.Y - 1 + (hitbox.Full.Bottom - position.Y)), "dust walk", 1, particleType);
            }
        }

        public static void CreateJumpParticles(string particleType, Vector2 position, HitboxObject hitbox, MasterManager master)
        {

            if (particleType != "" && particleType != null)
            {

                Random random = new Random();
                int amount;
                if (particleType == "sparks")
                    amount = (int)Math.Round((float)random.NextSingle(0f, 0.6f) * master.dt);
                else
                    amount = (int)Math.Round((float)random.NextSingle(0.6f, 0.9f) * master.dt);
                CreateParticles(master.entityManager, amount, new Vector2(position.X, position.Y - 1 + (hitbox.Full.Bottom - position.Y)), "dust jump", 1, particleType);
            }
        }
        public static void CreateParticles(EntityManager entityManager, int amount, Vector2 position, string type, float fade = 0f, string subType = "", Vector2 currentVel = new Vector2())
        {

            //List<ParticleSprite> particleList = new List<ParticleSprite>();
            if (subType == null)
                return;
            switch (type)
            {
                case "soul collected":
                    for (int i = 0; i < amount; i++)
                    {
                        Random random = new Random();

                        MyTexture image = Images.UniqueImage(Images.ImageDict[subType+"_soul_particle"]);
                        float speed = random.NextSingle(0.3f, 0.8f);
                        float weight = 0;
                        float lifetime = (float)random.NextSingle(0.3f, 0.5f);

                        int angle = random.Next(360);
                        Vector2 velocity = new Vector2(speed * (float)Math.Cos(angle), speed * (float)Math.Sin(angle));
                        ParticleSprite particle = new ParticleSprite(image, position, velocity, speed, weight, lifetime, fade);
                        entityManager.ParticlesArray.Add(particle);
                    }
                    break;
                case "player explode":
                    for (int i = 0; i < amount; i++)
                    {
                        Random random = new Random();

                        string name = "sparkle" + random.Next(1, 4).ToString();
                        MyTexture image = Images.UniqueImage(Images.ImageDict[name]);
                        float speed = random.NextSingle(0.4f, 0.6f);
                        float weight = 0.15f;
                        float lifetime = (float)random.NextSingle(0.6f, 1f);

                        int angle = random.Next(360);
                        Vector2 velocity = new Vector2(speed * (float)Math.Cos(angle), speed * (float)Math.Sin(angle));
                        velocity += currentVel/3;
                        velocity.Y -= 0.3f;

                        ParticleSprite particle = new ParticleSprite(image, position, velocity, speed, weight, lifetime, 1);
                        entityManager.ParticlesArray.Add(particle);
                    }
                    break;
                case "player fragments":
                    for (int i = 0; i < amount; i++)
                    {
                        Random random = new Random();
                        string name = "player_fragments" + random.Next(1, 3).ToString();

                        MyTexture image = Images.UniqueImage(Images.ImageDict[name]);
                        image.CurrentFrame = random.Next(0,1);
                        float speed = random.NextSingle(0.8f, 1.0f);
                        float weight = 1.7f;
                        float lifetime = (float)random.NextSingle(1.5f, 1.70f);

                        int angle = random.Next(360);
                        Vector2 velocity = new Vector2(speed * (float)Math.Cos(angle), speed * (float)Math.Sin(angle));
                        velocity.X = velocity.X / 1.7f + currentVel.X;
                        velocity.Y = velocity.Y - 0.6f + currentVel.Y;

                        ParticleSprite particle = new ParticleSprite(image, position, velocity, speed, weight, lifetime, 0, 2);
                        entityManager.ParticlesArray.Add(particle);
                    }
                    break;
                case "yellow_fire":
                    for (int i = 0; i < amount; i++)
                    {
                        Random random = new Random();
                        string name = "luxiar_soul_particle";

                        MyTexture image = Images.UniqueImage(Images.ImageDict[name]);
                        float speed = random.NextSingle(0.8f, 1.8f);
                        float weight = 0;
                        float lifetime = (float)random.NextSingle(0.2f, 0.3f);

                        int angle = random.Next(360);
                        Vector2 velocity = new Vector2(speed * (float)Math.Cos(angle), speed * (float)Math.Sin(angle));
                        ParticleSprite particle = new ParticleSprite(image, position, velocity, speed, weight, lifetime, fade);
                        entityManager.ParticlesArray.Add(particle);
                    }
                    break;
                case "sparkles":
                    for (int i = 0; i < amount; i++)
                    {
                        Random random = new Random();
                        List<string> imageTypes = new List<string> { "sparkle1", "sparkle2", "sparkle3" };
                        string name = "sparkle" + random.Next(1, 4).ToString();

                        MyTexture image = Images.UniqueImage(Images.ImageDict[name]);
                        float speed = random.NextSingle(0.2f, 0.8f);
                        float weight = 0;
                        float lifetime = (float)random.NextSingle(0.2f, 0.5f);

                        int angle = random.Next(360);
                        Vector2 velocity = new Vector2(speed * (float)Math.Cos(angle), speed * (float)Math.Sin(angle));
                        ParticleSprite particle = new ParticleSprite(image, position, velocity, speed, weight, lifetime, fade);
                        entityManager.ParticlesArray.Add(particle);
                    }
                    break;
                case "dust jump":
                    for (int i = 0; i < amount; i++)
                    {
                        Random random = new Random();
                        string name = subType;

                        MyTexture image = Images.UniqueImage(Images.ImageDict[name]);
                        float speed = random.NextSingle(0.55f, 1.2f);
                        float weight = 0.25f;
                        float lifetime = (float)random.NextSingle(0.7f, 0.9f);
                        float velx = random.NextSingle(-0.6f, 0.6f) * speed;
                        float vely = -speed * random.NextSingle(0.5f, 1f);
                        Vector2 velocity = new Vector2(velx, vely);

                        ParticleSprite particle = new ParticleSprite(image, position, velocity, speed, weight, lifetime, fade, 1);
                        entityManager.ParticlesArray.Add(particle);
                    }
                    break;
                case "dust walk":
                    for (int i = 0; i < amount; i++)
                    {
                        Random random = new Random();
                        string name = subType;

                        MyTexture image = Images.UniqueImage(Images.ImageDict[name]);
                        float speed = random.NextSingle(0.15f, 0.4f);
                        float weight = 0.15f;
                        float lifetime = (float)random.NextSingle(0.7f, 0.9f);
                        float velx = random.NextSingle(-0.6f, 0.6f) * speed;
                        float vely = -speed * random.NextSingle(0.8f, 1.1f);
                        Vector2 velocity = new Vector2(velx, vely);

                        ParticleSprite particle = new ParticleSprite(image, position, velocity, speed, weight, lifetime, fade, 1);
                        entityManager.ParticlesArray.Add(particle);
                    }
                    break;
                case "dust left":
                    for (int i = 0; i < amount; i++)
                    {
                        Random random = new Random();
                        string name = subType;

                        MyTexture image = Images.UniqueImage(Images.ImageDict[name]);
                        float speed = random.NextSingle(0.2f, 0.6f);
                        float weight = 0.1f;
                        float lifetime = (float)random.NextSingle(0.6f, 1.1f);
                        float velx = -speed;
                        float vely = -speed * random.NextSingle(0.3f, 1);
                        Vector2 velocity = new Vector2(velx, vely);

                        ParticleSprite particle = new ParticleSprite(image, position, velocity, speed, weight, lifetime, fade);
                        entityManager.ParticlesArray.Add(particle);
                    }
                    break;
                case "dust right":
                    for (int i = 0; i < amount; i++)
                    {
                        Random random = new Random();
                        string name = subType;

                        MyTexture image = Images.UniqueImage(Images.ImageDict[name]);
                        float speed = random.NextSingle(0.2f, 0.6f);
                        float weight = 0.1f;
                        float lifetime = (float)random.NextSingle(0.6f, 1.1f);
                        float velx = speed;
                        float vely = -speed * random.NextSingle(0.3f, 1);
                        Vector2 velocity = new Vector2(velx, vely);

                        ParticleSprite particle = new ParticleSprite(image, position, velocity, speed, weight, lifetime, fade);
                        entityManager.ParticlesArray.Add(particle);
                    }
                    break;
            }
            return;
        }
    }
}