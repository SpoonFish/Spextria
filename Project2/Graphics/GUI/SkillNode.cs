using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Master;
using Spextria.Statics;
using Spextria.Graphics.GUI.Text;
using Spextria.Graphics.GUI.Interactions;
using Microsoft.Xna.Framework.Input;

namespace Spextria.Graphics.GUI
{
    class SkillNode : IGuiButton

    {
        private ImagePanel Image;
        public Vector2 Position;
        private int Width;
        private int Height;
        private string State;
        private Rectangle ClickArea;
        private bool CurrentlyClicked;
        private bool CurrentlyHovered;
        private ButtonSignalEvent Signal;

        public SkillNode(string type, string purchaseName, List<string> reliesOn, Vector2 position, ButtonSignalEvent signal, MasterManager master)
        {

            State = "locked";
            if (reliesOn.Contains(""))
                State = "next";

            foreach (string purchase in master.storedDataManager.CurrentSaveFile.Purchases)
            {
                if (purchase == purchaseName)
                {
                    State = "unlocked";
                    break;
                }
                else if (reliesOn.Contains(purchase))
                {
                    State = "next";
                }
            }

            string imagePrefix;
            if (State == "locked")
                imagePrefix = "locked_";
            else if (State == "next")
                imagePrefix = "next_";
            else
                imagePrefix = "";

            if (type == "info")
                imagePrefix = "";
            
            Image = new ImagePanel(position, Images.ImageDict[imagePrefix + type + "_node"]);
            //Image = new ImagePanel(position, Images.ImageDict["coin"]);

            Signal = signal;
            CurrentlyClicked = false;
            Position = position;
            Width = 30;
            Height = 30;
            ClickArea = new Rectangle((int)position.X + 4, (int)position.Y + 3, Width, Height);
        }

        

        public ButtonSignalEvent Update(Point mousePos, ButtonState leftClick, Point offset = new Point(), Vector2 graphicsPositionScale = new Vector2())
        {
            Rectangle OffsetClickArea = new Rectangle(ClickArea.X + offset.X, ClickArea.Y + offset.Y, ClickArea.Width, ClickArea.Height);
            OffsetClickArea.X = (int)(OffsetClickArea.X * graphicsPositionScale.X);
            OffsetClickArea.Y = (int)(OffsetClickArea.Y * graphicsPositionScale.Y);
            OffsetClickArea.Width = (int)(OffsetClickArea.Width * graphicsPositionScale.X);
            OffsetClickArea.Height = (int)(OffsetClickArea.Height * graphicsPositionScale.Y);
            if (OffsetClickArea.Contains(mousePos))
            {

                if (leftClick == ButtonState.Pressed && CurrentlyHovered == true)
                {
                    CurrentlyClicked = true;
                }
                else if (leftClick != ButtonState.Pressed)
                    CurrentlyHovered = true;
                else
                    return new ButtonSignalEvent();




                if (leftClick == ButtonState.Released && CurrentlyClicked)
                {
                    if (State != "locked")
                        return Signal;
                    else
                        return new ButtonSignalEvent();
                }
            }
            else
            {
                CurrentlyHovered = false;
                CurrentlyClicked = false;
            }
            return new ButtonSignalEvent();
        }

        public void Draw(SpriteBatch spriteBatch, CameraManager cameraManager, Vector2 offset = new Vector2())
        {
            Image.Draw(spriteBatch, cameraManager, offset);
        }
    }
}
