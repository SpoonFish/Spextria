using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.StoredData;
using Spextria.Statics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Spextria.Master;


namespace Spextria.Maps.LevelObjectTypes
{
    class Goalpoint
    {

        private Rectangle GoalArea;
        public bool Reached;
        public string GoalAction;
        private Vector2 Position;
        private string Type;
        private bool Active;
        private double ActiveTime;
        private bool Activating;
        private MyTexture Texture;
        private MyTexture ActiveTexture;
        public Goalpoint(Vector2 position, Rectangle area, string type, string action)
        {
            GoalAction = action;
            Reached = false;
            ActiveTime = 0;
            Active = false;
            Activating = false;
            Position = position;
            GoalArea = area;
            Type = type;
            switch (type)
            {
                case "arrow":
                    Texture = Images.ImageDict["goal_arrow1_inactive"];
                    ActiveTexture = Images.ImageDict["goal_arrow1_active"];
                    break;
            }
        }

        public void Reset()
        {
            Reached = false;
            ActiveTime = 0;
            Active = false;
            Activating = false;
        }
        public void Update(MasterManager master)
        {
            if (!Activating)
            {
                if (master.entityManager.Player.Hitbox.Body.Intersects(GoalArea) && !Reached && !master.entityManager.Player.Inactive)
                {
                    master.entityManager.Player.Inactive = true;
                    Activating = true;
                    ActiveTexture.CurrentFrame = 0;
                    Reached = true;

                    // increments savefile's current level if the latest level is being played 
                    int latestLevel = master.storedDataManager.CurrentSaveFile.CurrentLevel;
                    if (latestLevel < master.entityManager.Player.CurrentLevel + 1)
                        master.storedDataManager.CurrentSaveFile.Checkpoint = 0;
                    master.storedDataManager.CurrentSaveFile.CurrentLevel = Math.Max(latestLevel, master.entityManager.Player.CurrentLevel + 1);
                    master.storedDataManager.SaveFile();
                }
            }

            else if (Activating && !Active)
            {
                ActiveTime += master.timePassed;
                if (ActiveTime > 1)
                {
                    ActiveTexture.CurrentFrame = 0;
                    Active = true;
                    Activating = false;
                }
                else if (ActiveTime > 0.9)
                    ActiveTexture.CurrentFrame = 9;
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
