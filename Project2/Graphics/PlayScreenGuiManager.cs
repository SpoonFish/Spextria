using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spextria.Entities;
using Spextria.Graphics.GUI;
using Spextria.Graphics.GUI.Interactions;
using Spextria.Graphics.GUI.PlayScreen;
using Spextria.Master;
using Spextria.Statics;
using System;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;

namespace Spextria.Graphics
{
    class PlayScreenGuiManager
    {
        private ImagePanel NotificationImage;
        private Button NotificationPopup;
        private Bar HpBar;
        private Bar SeBar;
        private bool SequenceActive;
        private double NotifTime;
        private MyTexture BarUnderlay;
        private MyTexture ChargeRing;
        private MyTexture BarOverlay;
        private TextBox Fps;
        private TextBox CoinCounter;
        private ImagePanel CoinIcon;
        private ImagePanel DebugImage;
        private ImagePanel SoulIcon;
        private TextBox SoulCounter;
        private bool Notifying;
        private TextBox DebugVal;
        public TextBox CurrentDialog;
        private FadingImage Fadeout;
        private ButtonImage WeaponButton;
        private ImagePanel PriWeaponIcon;
        private ImagePanel WeaponCdOverlay;

        public PlayScreenGuiManager(EntityManager entityManager)
        {
            NotificationPopup = new Button("NEW WEAPON UNLOCKED!", "NEW WEAPON UNLOCKED!", new Vector2(90, 233), new Vector2(256, 40), new ButtonSignalEvent(), textOffset: new Vector2(20, 0));


            NotificationImage = new ImagePanel(new Vector2(104, 241), Images.ImageDict["none"], new Vector2(24, 24));
            NotifTime = 0;
            Notifying = false;
            WeaponCdOverlay = new ImagePanel(new Vector2(0, 46), Images.ImageDict["weapon_cooldown_overlay"]);
            WeaponButton = new ButtonImage(new ImagePanel(new Vector2(0, 46), Images.ImageDict["weapon_button"]), new ImagePanel(new Vector2(0, 46), Images.ImageDict["weapon_button_hovered"]), new Vector2(2, 48), new Vector2(34, 32), new ButtonSignalEvent());
            PriWeaponIcon = new ImagePanel(new Vector2(1, 52), Images.ImageDict[entityManager.Player.PrimaryAttack.Name]);
            DebugImage = new ImagePanel(new Vector2(90, 50), Images.ImageDict["slash_attack"], new Vector2(96, 48));
            CoinIcon = new ImagePanel(new Vector2(360, 4), Images.ImageDict["coin"], new Vector2(16, 16));
            SoulIcon = new ImagePanel(new Vector2(360, 26), Images.ImageDict[entityManager.Player.CurrentPlanet+"_soul"], new Vector2(16, 16));
            SequenceActive = false;
            ImagePanel blackImage = new ImagePanel(new Vector2(0, 0), Images.ImageDict["blackout"], new Vector2(427, 234));
            Fadeout = new FadingImage(blackImage, "in", 2, false, 0, true);
            HpBar = new Bar(new Vector2(14, 6), entityManager.Player.Stats.MaxHp, 0, "player_hp");
            SeBar = new Bar(new Vector2(5, 24), entityManager.Player.Stats.MaxSe, 0, "player_se");
            BarUnderlay = Images.ImageDict["bar_underlay"];
            BarOverlay = Images.ImageDict["bar_overlay"];
            ChargeRing = Images.ImageDict["charge_ring"];
            CurrentDialog = new TextBox("", new Vector2(-100, -100), 1, "none", true);
            CoinCounter = new TextBox(":", new Vector2(380, 4), 100, "none");
            SoulCounter = new TextBox(":", new Vector2(380, 26), 100, "none");
            Fps = new TextBox("FPS: 60", new Vector2(10, 60), 75, "basic");
            DebugVal = new TextBox("P", new Vector2(60, 60), 75, "basic");
        }

        public void StartFadeoutSequence()
        {
            SequenceActive = true;
            Fadeout.Fade();
        }
        public void ResetFadeoutSequence()
        {
            SequenceActive = false;

            ImagePanel blackImage = new ImagePanel(new Vector2(0, 0), Images.ImageDict["blackout"], new Vector2(427, 234));
            Fadeout = new FadingImage(blackImage, "in", 1, false, 0, true);
        }
        public void Update(MasterManager master)
        {
            string planet;
            if (CoinCounter.Text == ":")
                ReloadCoinCounter(master.storedDataManager.CurrentSaveFile.Coins);
            if (SoulCounter.Text == ":")
            {
                planet = master.entityManager.Player.CurrentPlanet;
                switch (planet)
                {
                    case "luxiar":
                        ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.LightSouls);
                        break;
                    case "gramen":
                        ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.GrowthSouls);
                        break;
                    case "freone":
                        ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.FrostSouls);
                        break;
                    case "umbrac":
                        ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.ShadowSouls);
                        break;
                    case "inferni":
                        ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.FlameSouls);
                        break;
                    case "ossium":
                        ReloadSoulCounter(master.storedDataManager.CurrentSaveFile.TormentSouls);
                        break;
                }
            }
            Fadeout.Update(master.gameTime);
            HpBar.MaxValue = master.entityManager.Player.Stats.MaxHp;
            HpBar.Update(master.entityManager.Player.Stats.Hp, master);
            SeBar.MaxValue = master.entityManager.Player.Stats.MaxSe;
            SeBar.Update(master.entityManager.Player.Stats.Se, master);

            float percent;
            if (master.entityManager.Player.PrimaryAttack.Charging)
            {
                percent = (float)master.entityManager.Player.PrimaryAttack.ChargeTimeActive / (float)master.entityManager.Player.PrimaryAttack.ChargeTime;
                percent = -1 * percent + 1;
                WeaponCdOverlay.Opacity = -1 * percent + 1;
                WeaponCdOverlay.Position = new Vector2(0 - 37 * percent, 46);
            }

            if (!master.entityManager.Player.PrimaryAttack.Reloaded)
            {
                percent = (float)master.entityManager.Player.PrimaryAttack.ReloadTimeActive / (float)master.entityManager.Player.PrimaryAttack.ReloadTime;
                WeaponCdOverlay.Opacity = -1 * percent + 1;
                WeaponCdOverlay.Position = new Vector2(0 - 37 * percent, 46);
            }

            WeaponButton.Update(master.MousePos, master.CurrentMouseState.LeftButton);



        }
        public void ReloadWeaponIcons(MasterManager master)
        {
            PriWeaponIcon = new ImagePanel(new Vector2(1, 52), Images.ImageDict[master.entityManager.Player.PrimaryAttack.Name]);

        }
        public bool CheckWeaponButtonClicked(MasterManager master)
        {

            if (master.CurrentMouseState.LeftButton == ButtonState.Pressed)
                if (WeaponButton.ClickArea.Contains(master.MousePos))
                    return true;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, MasterManager master)
        {
            //fixed screen drawing so we gotta start another spritebatch
            spriteBatch.End();

            float percent;
            const float scaleX = 1.775f;//(float)_graphics.PreferredBackBufferWidth / 80;
            const float scaleY = 1.775f;//(float)_graphics.PreferredBackBufferHeight / 48;
            Matrix matrix_scaler = Matrix.CreateScale(scaleX, scaleY, 1.0f);
            Matrix unfixed_matrix = master.cameraManager.Camera.GetViewMatrix();
            Matrix fixed_matrix = master.cameraManager.StaticCamera.GetViewMatrix() + matrix_scaler;
            //unfixed_matrix = matrix_scaler + unfixed_matrix;
            //fixed_matrix = matrix_scaler + fixed_matrix;
            spriteBatch.Begin(transformMatrix: fixed_matrix, samplerState: SamplerState.PointClamp);

            //DebugImage.Draw(spriteBatch, master.cameraManager);
            CoinCounter.Draw(spriteBatch, master.cameraManager);
            SoulCounter.Draw(spriteBatch, master.cameraManager);
            CoinIcon.Draw(spriteBatch, master.cameraManager);
            SoulIcon.Draw(spriteBatch, master.cameraManager);
            //Fps.Draw(spriteBatch, master.cameraManager);
            //DebugVal.Draw(spriteBatch, master.cameraManager);
            if (master.NpcTalking)
            {
                float dialogOpacity = 0;
                if (master.NpcTalkTime < 3)
                    dialogOpacity = (float)master.NpcTalkTime - 2;
                else if (master.NpcTalkTime < 4)
                    dialogOpacity = 1- ((float)master.NpcTalkTime - 3);

                CurrentDialog.Draw(spriteBatch, master.cameraManager, Vector2.Zero, dialogOpacity);
            }
            if (Notifying)
            {
                NotifTime += master.timePassed;
                double yOffset = 0;
                if (NotifTime < 0.5)
                    yOffset = 100 * NotifTime;
                else if (NotifTime > 3.5 && NotifTime <= 4)
                    yOffset = 100 * (0.5 - (NotifTime - 3.5));
                else if (NotifTime > 4)
                {
                    Notifying = false;
                    NotifTime = 0;
                }
                else
                    yOffset = 50;

                NotificationPopup.Draw(spriteBatch, master.cameraManager, new Vector2(0,-(int)yOffset));
                NotificationImage.Draw(spriteBatch, master.cameraManager, new Vector2(0, -(int)yOffset));
            }
            BarUnderlay.Draw(spriteBatch, new Vector2(0, 0), 1, new Vector2(426, 233));
            HpBar.Draw(spriteBatch);
            SeBar.Draw(spriteBatch);
            BarOverlay.Draw(spriteBatch, new Vector2(0, 0), 1, new Vector2(426, 233));

            WeaponButton.Draw(spriteBatch, master.cameraManager);
            PriWeaponIcon.Draw(spriteBatch, master.cameraManager);

            WeaponCdOverlay.Draw(spriteBatch, master.cameraManager);


            if (master.entityManager.Player.PrimaryAttack.Charging && !master.NpcTalking)
            {
                percent = (float)master.entityManager.Player.PrimaryAttack.ChargeTimeActive / (float)master.entityManager.Player.PrimaryAttack.ChargeTime;
                percent = -1 * percent + 1;
                Vector2 size = new Vector2(Math.Max(32 * percent,1), 32 * percent);
                ChargeRing.Draw(spriteBatch, -size / 2 + master.MousePos.ToVector2() + new Vector2(1,-1), 1, size);
            }

            if (SequenceActive)
                Fadeout.Draw(spriteBatch, master.cameraManager);
        }

        public void SetDebugVal(string value)
        {
            DebugVal.ReloadText(value);
        }
        public void ReloadCoinCounter(int coins)
        {
            CoinCounter.ReloadText(": " + coins);
        }
        public void ReloadSoulCounter(int souls)
        {
            SoulCounter.ReloadText(": " + souls);
        }
        public void ReloadFps(string colour, double framerate)
        {

            Fps.ReloadText($"DT: #{colour}#{Math.Round(60 / framerate, 2)}");
        }

        public void NotifyNewItem(string item)
        {
            NotificationImage = new ImagePanel(new Vector2(104, 241), Images.ImageDict[item], new Vector2(24, 24));

            Notifying = true;
        }
    }
}
