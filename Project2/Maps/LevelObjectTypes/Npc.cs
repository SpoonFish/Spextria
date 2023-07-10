using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spextria.Entities;
using Spextria.Entities.EntityParts;
using Spextria.Graphics;
using Spextria.Graphics.GUI;
using Spextria.Master;
using Spextria.Statics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Maps.LevelObjectTypes
{
    class Npc : LevelObject
    {
        private MyTexture Texture;
        private List<Dialog> Dialogs = new List<Dialog>();
        private string Name;
        private bool NextDialog;
        private int CurrentDialog;
        private bool DialogLoaded;
        public Npc(Vector2 position, Rectangle area, string npc)
        {
            CurrentDialog = 0;
            DialogLoaded = false;
            NextDialog = false;
            Position = position;
            Area = area;
            Name = npc;
            Texture = Images.ImageDict[npc];
            Dialogs = Npcs.NpcDialogs[npc];
        }

        public override void Update(MasterManager master)
        {
            Vector2 distanceFromPlayerVec = new Vector2(Math.Abs(master.entityManager.Player.Hitbox.Body.Center.X - (Position.X + Area.Width / 2)), Math.Abs(master.entityManager.Player.Hitbox.Body.Center.Y - (Position.Y + Area.Height / 2)));
            float distanceFromPlayer = (float)Math.Sqrt(distanceFromPlayerVec.X * distanceFromPlayerVec.X + distanceFromPlayerVec.Y * distanceFromPlayerVec.Y);

            if (!master.NpcTalking)
            {
                if (distanceFromPlayer < 48)
                {
                    Texture.SetType(1);
                    if (master.CurrentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        master.NpcTalking = true;
                        master.NpcTalkTime = 0;
                    }
                }
                else
                {
                    Texture.SetType(0);
                }
            }
            else
            {
                if (distanceFromPlayer > 124)
                {
                    master.NpcTalkTime = Math.Max(10, master.NpcTalkTime);

                    DialogLoaded = false;
                    NextDialog = false;
                    CurrentDialog = 0;
                }
                Texture.SetType(0);

                if (master.NpcTalkTime > 9)
                {
                    master.NpcTalkTime += master.timePassed;
                    if (master.NpcTalkTime > 12.5)
                    {
                        master.NpcTalking = false;
                        master.mapManager.CurrentMapDrawn = false;
                        master.CurrentMapEffect = null;
                        master.CurrentSpriteEffect = null;
                    }
                    return;
                }
                if (master.NpcTalkTime < 2)
                    master.NpcTalkTime += master.timePassed*2;
                else
                {
                    Dialog dialog = Dialogs[CurrentDialog];


                    if (!DialogLoaded)
                    {

                        bool conditionFailed = false;
                        switch (dialog.Condition)
                        {
                            case "!has_weapon":
                                if (master.storedDataManager.CheckSkillUnlock(dialog.ConditionValue))
                                    conditionFailed = true;
                                break;
                        }
                        if (dialog.Condition != "")
                            if (conditionFailed)
                            {
                                if (dialog.ConditionalContinue)
                                {
                                    master.NpcTalkTime = Math.Max(10, master.NpcTalkTime);
                                    DialogLoaded = false;
                                    NextDialog = false;
                                    CurrentDialog = 0;
                                }
                                else
                                {
                                    CurrentDialog += 1;
                                    dialog = Dialogs[CurrentDialog];
                                }
                            }

                        if (!conditionFailed)
                            switch (dialog.Action)
                            {
                                case "give_weapon":
                                    master.storedDataManager.CurrentSaveFile.Purchases.Add(dialog.ActionValue);
                                    master.playGuiManager.NotifyNewItem(dialog.ActionValue);
                                    break;
                            }
                        master.playGuiManager.CurrentDialog = new TextBox(dialog.Message, Position - master.cameraManager.Camera.Position, 150, "dialog", true);
                        DialogLoaded = true;
                    }
                    int height = dialog.LineAmount;
                    master.playGuiManager.CurrentDialog.Position = Position - master.cameraManager.Camera.Position + new Vector2(25,-8 - 20 * height);
                    if (master.NpcTalkTime < 3.1)
                        master.NpcTalkTime += master.timePassed*2;
                    else if (master.CurrentMouseState.LeftButton == ButtonState.Pressed)
                        NextDialog = true;

                    if (NextDialog)
                    {
                        if (master.NpcTalkTime < 4)
                        {
                            master.NpcTalkTime += master.timePassed*2;
                        }
                        else
                        {
                            CurrentDialog += 1;
                            master.NpcTalkTime = 2;
                            DialogLoaded = false;
                            NextDialog = false;
                            if (CurrentDialog > Dialogs.Count-1)
                            {
                                CurrentDialog = 0;
                                master.NpcTalkTime = 10;
                            }
                        }
                    }
                }

            }
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture.Draw(spriteBatch, Position, 1);
        }
    }
}
