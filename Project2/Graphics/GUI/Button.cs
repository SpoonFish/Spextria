using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Statics;
using Spextria.Graphics.GUI.Text;
using Spextria.Graphics.GUI.Interactions;
using Microsoft.Xna.Framework.Input;

namespace Spextria.Graphics.GUI
{
    class Button : IGuiButton

    {
        private Panel Box;
        private string Text;
        private string OrigText;
        private string HoverText;
        public Vector2 Position;
        public Vector2 TextOffset;
        private int Width;
        private int Height;
        private string BoxType;
        private string OrigBoxType;
        private Rectangle ClickArea;
        private string HoverBoxType;
        private string ClickBoxType;
        private bool CurrentlyClicked;
        private bool CurrentlyHovered;
        private ButtonSignalEvent Signal;
        private bool ScreenFixed;
        public List<LetterSprite> BoxImageText;

        public Button(string text, string hoverText, Vector2 position, Vector2 size, ButtonSignalEvent signal, string boxType = "button", string hoverBoxType = "button_hovered", string clickBoxType = "button_clicked", bool screenFixed = true, Vector2 textOffset = new Vector2())
        {
            TextOffset = textOffset;
            Signal = signal;
            CurrentlyClicked = false;
            BoxType = boxType;
            OrigBoxType = boxType;
            HoverBoxType = hoverBoxType;
            ClickBoxType = clickBoxType;
            ScreenFixed = screenFixed;
            Text = text.ToUpper();
            OrigText = text.ToUpper();
            HoverText = hoverText.ToUpper();
            Position = position;
            Width = (int)size.X;
            Height = (int)size.Y;
            ClickArea = new Rectangle((int)position.X-2, (int)position.Y-2, Width+4, Height+4);
            ScreenFixed = screenFixed;
            BoxImageText = new List<LetterSprite>();
            LoadText();
            LoadBoxType();
        }

        private void LoadText()
        {
            BoxImageText.Clear();
            int lineX = 0;
            string colourString = "";
            Color currentColour = Color.White;
            bool searchingColour = false;
            int lineWidth = 0;
            foreach (char letter in Text)
            {
                if (searchingColour)
                {
                    if (letter == '#')
                    {
                        searchingColour = false;
                        continue;
                    }
                    colourString += letter;
                }
                else
                {
                    if (letter == '#')
                    {
                        searchingColour = true;
                        continue;
                    }
                    TextLetter image = Images.FontDict[' '];
                    if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ.,:'!?-+1234567890% ".Contains(letter))
                        image = Images.FontDict[letter];
                    lineX += image.Texture.Width + image.Spacing;

                }

            }
            lineWidth = lineX;
            int margin = (Width - lineWidth) / 2;
            lineX = 0;
            colourString = "";
            currentColour = Color.White; searchingColour = false;
            foreach (char letter in Text)
            {
                if (searchingColour)
                {
                    if (letter == '#')
                    {
                        searchingColour = false;
                        try
                        {
                            currentColour = Colours.ColourDict[colourString.ToLower()];
                        }
                        catch
                        {
                            currentColour = Colours.ColourDict["grey"];
                        }
                        colourString = "";
                        continue;
                    }
                    colourString += letter;
                }
                else
                {
                    if (letter == '#')
                    {
                        searchingColour = true;
                        continue;
                    }
                    TextLetter image = Images.FontDict[' '];
                    if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ.,:'!?-+1234567890% ".Contains(letter))
                        image = Images.FontDict[letter];
                    LetterSprite letterSprite = new LetterSprite(image, new Vector2(lineX + margin, (Height - 17) / 2), currentColour);
                    lineX += image.Texture.Width + image.Spacing;
                    BoxImageText.Add(letterSprite);

                }

            }


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
                BoxType = HoverBoxType;

                if (leftClick == ButtonState.Pressed && CurrentlyHovered == true)
                {
                    BoxType = ClickBoxType;
                    CurrentlyClicked = true;
                }
                else if (leftClick != ButtonState.Pressed)
                    CurrentlyHovered = true;
                else
                    return new ButtonSignalEvent();


                if (Text != HoverText)
                {
                    Text = HoverText;
                    ReloadText(HoverText);
                }

                LoadBoxType();

                if (leftClick == ButtonState.Released && CurrentlyClicked)
                {
                    return Signal;
                }
            }
            else
            {
                CurrentlyHovered = false;
                CurrentlyClicked = false;
                if (BoxType != OrigBoxType)
                {
                    BoxType = OrigBoxType;
                    LoadBoxType();
                }
                if (Text != OrigText)
                {
                    Text = OrigText;
                    ReloadText(OrigText);
                }
            }
            return new ButtonSignalEvent();
        }

        public void ReloadText(string newText)
        {
            Text = newText;
            LoadText();
        }

        private void LoadBoxType()
        {
            switch (BoxType)
            {
                case "none":
                    Box = new Panel(Images.ImageDict["no_box"], new Vector2(Width, Height), 1);
                    break;
                case "button":
                    Box = new Panel(Images.ImageDict["button"], new Vector2(Width, Height), 3);
                    break;
                case "button_hovered":
                    Box = new Panel(Images.ImageDict["button_hovered"], new Vector2(Width, Height), 3);
                    break;
                case "button_clicked":
                    Box = new Panel(Images.ImageDict["button_clicked"], new Vector2(Width, Height), 3);
                    break;
                case "button_red":
                    Box = new Panel(Images.ImageDict["button_red"], new Vector2(Width, Height), 3);
                    break;
                case "button_red_hovered":
                    Box = new Panel(Images.ImageDict["button_red_hovered"], new Vector2(Width, Height), 3);
                    break;
                case "button_red_clicked":
                    Box = new Panel(Images.ImageDict["button_red_clicked"], new Vector2(Width, Height), 3);
                    break;
                default:
                    Box = new Panel(Images.ImageDict[BoxType], new Vector2(Width, Height), 3);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, CameraManager cameraManager, Vector2 offset = new Vector2())
        {
            Vector2 origin = new Vector2(0, 0);
            Box.Draw(spriteBatch, Position + offset);
            foreach (LetterSprite letterSprite in BoxImageText)
            {
                Texture2D image = letterSprite.Letter.Texture;
                int width = image.Width;
                int height = image.Height;
                Vector2 drawPos = Position;
                drawPos = new Vector2((int)Position.X + (int)letterSprite.Position.X, (int)Position.Y + (int)letterSprite.Position.Y);
                drawPos += TextOffset;


                Rectangle sourceRectangle = new Rectangle(0, 0, width, height);

                Rectangle destinationRectangle = new Rectangle((int)drawPos.X + (int)offset.X, (int)drawPos.Y+(int)offset.Y, width, height);

                spriteBatch.Draw(image, destinationRectangle, sourceRectangle, letterSprite.Colour, 0, origin, SpriteEffects.None, 1);
            }
        }
    }
}
