using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.Graphics.GUI;
using Spextria.Entities.Variants;
using Spextria.Entities;
using Spextria.Statics;
using Spextria.Master;
using Spextria.Maps;

namespace Spextria.Entities
{
    class EntityManager
    {
        public PlayerEntity Player;
        public List<MonsterEntity> EnemiesArray;
        public List<CollectableEntity> CollectablesArray;
        public List<MyTexture> AttacksArray;
        public List<MovingEntity> FriendlyArray;
        public List<ParticleSprite> ParticlesArray;
        public List<TextBox> TextBoxArray;

        public List<ProjectileEntity> ProjectilesArray;

        public EntityManager(PlayerEntity player)
        {
            Player = player;
            AttacksArray = new List<MyTexture>();
            EnemiesArray = new List<MonsterEntity>();
            CollectablesArray = new List<CollectableEntity>();
            FriendlyArray = new List<MovingEntity>();
            ParticlesArray = new List<ParticleSprite>();
            TextBoxArray = new List<TextBox>();
            ProjectilesArray = new List<ProjectileEntity>();
        }

        public void DropLoot(MonsterStats stats, Vector2 position, MasterManager master)
        {
            int coins = stats.CoinReward;
            int souls = stats.SoulReward;

            if (new Random().NextDouble() < Player.Stats.ExtraSoulChance)
                souls = (int)Math.Ceiling(souls * 1.3f);

            for (int i = 0; i < coins; i++)
            {
                CollectablesArray.Add(new MovingCoin(Images.ImageDict["coin"], position, 0, 0, true, 1));
            }
            for (int i = 0; i < souls; i++)
            {
                CollectablesArray.Add(new Soul(Images.ImageDict[Player.CurrentPlanet+"_soul"], new Vector2(position.X, position.Y - 8)));
            }
            Random random = new Random();

            int chance = 7;

            if (master.storedDataManager.CheckSkillUnlock("better_repair_cells"))
                chance = 11;

            if (random.Next(1, 100) <= chance)
            {

                CollectablesArray.Add(new RepairCell(Images.ImageDict["repair_cell"], new Vector2(position.X, position.Y - 8)));
            }


            chance = 7;

            if (master.storedDataManager.CheckSkillUnlock("better_energy_cells"))
                chance = 11;

            if (random.Next(1, 100) <= chance)
            {

                CollectablesArray.Add(new EnergyCell(Images.ImageDict["energy_cell"], new Vector2(position.X, position.Y - 8)));
            }
        }

        private void DrawTextbox(TextBox text, SpriteBatch spriteBatch, CameraManager cameraManager)
        {

            if (text.ScreenFixed)
            {

                spriteBatch.End();
                const int scaleX = 8;//(float)_graphics.PreferredBackBufferWidth / 80;
                const int scaleY = 8;//(float)_graphics.PreferredBackBufferHeight / 48;
                Matrix matrix_scaler = Matrix.CreateScale(scaleX, scaleY, 1.0f);
                Matrix unfixed_matrix = cameraManager.Camera.GetViewMatrix();
                Matrix fixed_matrix = cameraManager.StaticCamera.GetViewMatrix();
                unfixed_matrix = matrix_scaler + unfixed_matrix;
                fixed_matrix = matrix_scaler + fixed_matrix;

                spriteBatch.Begin(transformMatrix: unfixed_matrix, samplerState: SamplerState.PointClamp);
                text.Draw(spriteBatch, cameraManager);
                spriteBatch.End();
                spriteBatch.Begin(transformMatrix: fixed_matrix, samplerState: SamplerState.PointClamp);
            }
            else
                text.Draw(spriteBatch, cameraManager);
        }
        public void DrawSprites(SpriteBatch spriteBatch, CameraManager cameraManager, MasterManager master, GraphicsDevice graphicsDevice)
        {
            Matrix matrix_scaler = Matrix.CreateScale(8, 8, 1.0f);
            Matrix cam_matrix = cameraManager.Camera.GetViewMatrix();
            Matrix matrix = matrix_scaler + cam_matrix;

            foreach (CollectableEntity collectable in CollectablesArray)
            {
                collectable.Draw(spriteBatch, master.MapOpacity);
                //collectable.Debug(spriteBatch);
            }
            foreach (ParticleSprite particle in ParticlesArray)
            {
                particle.Draw(spriteBatch, master.MapOpacity);
            }
            foreach (MonsterEntity monster in EnemiesArray)
            {
                monster.Draw(spriteBatch, master.MapOpacity);
                //monster.Debug(spriteBatch);
            }

            foreach (TextBox text in TextBoxArray)
            {
                DrawTextbox(text, spriteBatch, cameraManager);
            }

            foreach (ProjectileEntity projectile in ProjectilesArray)
            {
                projectile.Draw(spriteBatch, master.MapOpacity);
            }

        }

        

        public void UpdateSprites(MasterManager master)
        {
            for (int i = CollectablesArray.Count-1; i >= 0; i--)
            {
                CollectablesArray[i].Update(Player.Hitbox, master);
            }
            for (int i = ParticlesArray.Count - 1; i >= 0; i--)
            {
                ParticlesArray[i].Update(this, master.timePassed, (float)master.dt, master.mapManager.CurrentMap.Collisions);
            }
            for (int i = EnemiesArray.Count - 1; i >= 0; i--)
            {
                EnemiesArray[i].Update(master);
            }
            for (int i = ProjectilesArray.Count - 1; i >= 0; i--)
            {
                ProjectilesArray[i].Update(master.entityManager, master.timePassed, (float)master.dt, master.mapManager.CurrentMap.Collisions);
            }
        }
    }
}
