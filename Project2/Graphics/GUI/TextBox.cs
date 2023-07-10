using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Statics;
using Spextria.Graphics.GUI.Text;

namespace Spextria.Graphics.GUI
{
    class TextBox : IGuiComponent

    {
        private Panel Box;
        public string Text;
        public Vector2 Position;
        private int Width;
        private int MaxWidth;
        private int MaxHeight;
        private string BoxType;
        public bool ScreenFixed;
        public List<LetterSprite> BoxImageText;

        public TextBox(string text, Vector2 position, int width, string boxType = "basic", bool screenFixed = false)
        {
            BoxType = boxType;
            ScreenFixed = screenFixed;
            Text = text.ToUpper();
            Position = position;
            Width = width;
            MaxWidth = 0;
            MaxHeight = 0;
            ScreenFixed = screenFixed;
            BoxImageText = new List<LetterSprite>();
            LoadText();
        }

        private void LoadText()
        {
            BoxImageText.Clear();
            int lineX = 0;
            int lineY = 0;
            string colourString = "";
            Color currentColour = Color.White;
            bool searchingColour = false;

            
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


                    if ((lineX + image.Texture.Width > Width && letter == ' ') || letter == '\\')
                    {
                        lineX = 0;
                        lineY += 20;
                        continue;
                    }
                    LetterSprite letterSprite = new LetterSprite(image, new Vector2(lineX, lineY), currentColour);
                    lineX += image.Texture.Width + image.Spacing;
                    if (MaxWidth < lineX)
                        MaxWidth = lineX;
                    if (MaxHeight < lineY + 17)
                        MaxHeight = lineY + 17;
                    BoxImageText.Add(letterSprite);

                }
            }
            switch (BoxType)
            {
                case "black":
                    Box = new Panel(Images.ImageDict["black"], new Vector2(MaxWidth, MaxHeight), 1);
                    break;
                case "none":
                    Box = new Panel(Images.ImageDict["no_box"], new Vector2(MaxWidth, MaxHeight), 1);
                    break;
                case "slanted":
                    Box = new Panel(Images.ImageDict["slanted_box"], new Vector2(MaxWidth, MaxHeight), 10);
                    break;
                case "basic":
                    Box = new Panel(Images.ImageDict["text_box"], new Vector2(MaxWidth, MaxHeight), 5);
                    break;
                case "metal":
                    Box = new Panel(Images.ImageDict["metal_box"], new Vector2(MaxWidth, MaxHeight), 4);
                    break;
                case "dialog":
                    Box = new Panel(Images.ImageDict["speech_box_right"], new Vector2(MaxWidth, MaxHeight), 10);
                    break;
            }

            
        }

        public void ReloadText(string newText)
        {
            Text = newText;
            LoadText();
        }
        public void Draw(SpriteBatch spriteBatch, CameraManager cameraManager, Vector2 offset = new Vector2(), float opacity = 1)
        {
            Vector2 origin = new Vector2(0, 0);
            Box.Draw(spriteBatch, Position + offset, opacity);
            foreach (LetterSprite letterSprite in BoxImageText)
            {
                Texture2D image = letterSprite.Letter.Texture;
                int width = image.Width;
                int height = image.Height;
                Vector2 drawPos = new Vector2(Position.X + letterSprite.Position.X + offset.X, Position.Y + letterSprite.Position.Y + offset.Y);


                Rectangle sourceRectangle = new Rectangle(0, 0, width, height);

                Rectangle destinationRectangle = new Rectangle((int)drawPos.X, (int)drawPos.Y, width, height);

                spriteBatch.Draw(image, destinationRectangle, sourceRectangle, letterSprite.Colour * opacity, 0, origin, SpriteEffects.None, 1);
            }
        }
    }
}
