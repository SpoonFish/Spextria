using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.StoredData;
using Spextria.Statics;
using System;
using System.Collections.Generic;
using System.Text;
using Spextria.Master;

namespace Spextria.Content.Maps.LevelObjectsTypes
{
    class Checkpoint
    {
        private Rectangle GoalArea;
        public Vector2 SpawnPosition;
        private Vector2 Position;
        private string Type;
        private bool Active;
        private double ActiveTime;
        private bool Activating;
        private MyTexture Texture;
        private MyTexture ActiveTexture;
        public Checkpoint(Vector2 spawnPosition, Vector2 position, Rectangle area, string type)
        {
            ActiveTime = 0;
            Active = false;
            Activating = false;
            Position = position;
            SpawnPosition = spawnPosition;
            GoalArea = area;
            Type = type;
            switch (type)
            {
                case "luxiar1":
                    Texture = Images.ImageDict["luxiar_checkpoint1_inactive"];
                    ActiveTexture = Images.ImageDict["luxiar_checkpoint1_active"];
                    break;
                case "neutral1":
                    Texture = Images.ImageDict["neutral_checkpoint1_inactive"];
                    ActiveTexture = Images.ImageDict["neutral_checkpoint1_active"];
                    break;
            }
        }
        public void Update(MasterManager master)
        {
            if (!Activating)
            {
                int latestLevel = master.storedDataManager.CurrentSaveFile.CurrentLevel;
                if (master.storedDataManager.CurrentSaveFile.Checkpoint == 1 || latestLevel > master.entityManager.Player.CurrentLevel)
                    Activating = true;

                if (master.entityManager.Player.Hitbox.Body.Intersects(GoalArea))
                {
                    Activating = true;
                    ActiveTexture.CurrentFrame = 0;
                    if (latestLevel == master.entityManager.Player.CurrentLevel)
                        master.storedDataManager.CurrentSaveFile.Checkpoint = 1;
                    master.entityManager.Player.Stats.Hp = master.entityManager.Player.Stats.MaxHp;
                    master.storedDataManager.SaveFile();

                }
            }

            else if (Activating && !Active)
            {
                ActiveTime += master.timePassed;
                if (ActiveTime > 0.72)
                {
                    ActiveTexture.SetType(1);
                    ActiveTexture.CurrentFrame = 0;
                    Active = true;
                }
                else if (ActiveTime > 0.54)
                    ActiveTexture.CurrentFrame = 3;
            }


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Activating)
                ActiveTexture.Draw(spriteBatch, Position);
            else
                Texture.Draw(spriteBatch, Position);
        }
    }
}
