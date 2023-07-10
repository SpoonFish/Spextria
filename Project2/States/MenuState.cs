using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spextria.Entities.AttackObjects;
using Spextria.Graphics.GUI;
using Spextria.Graphics.GUI.Interactions;
using Spextria.Master;
using Spextria.Statics;
using Spextria.StoredData;
using System;
using System.Collections.Generic;

namespace Spextria.States
{
    class MenuState : State
    {
        private bool PendingChange;
        private bool Frozen;
        private string CurrentMenu;
        private GuiContent Components;
        private TextBox Debug;
        private List<string> MenuStack; // remembers what menus you were last on for when you go 'back'

        public MenuState(string menu = "level_select_luxiar", MasterManager master = null)
        {
            PendingChange = true;
            Frozen = false;
            CurrentMenu = menu;
            MenuStack = new List<string>();
            Debug = new TextBox("", new Vector2(268, 108), 250, "basic", true);
            Components = new GuiContent();
            LoadMenu(menu, master);
        }

        private void CheckButtons(MasterManager master, SpextriaGame game, List<IGuiButton> buttons, Point offset = new Point(), GraphicsDevice graphicsDevice = null)
        {

            foreach (IGuiButton button in buttons)
            {
                ButtonSignalEvent signal = button.Update(master.MousePos, master.CurrentMouseState.LeftButton, offset, master.GraphicsPositionScale);

                if (signal.Action != "none")
                {
                    switch (signal.Action)
                    {
                        case "reset_settings":
                            master.storedDataManager.ResetSettings();
                            UnloadMenu();
                            LoadMenu("settings", master, true);
                            return;

                        case "change_soundvol":
                            int soundVolume = master.storedDataManager.Settings.SoundVolume;
                            soundVolume += int.Parse(signal.Subject);
                            if (soundVolume > 100)
                                soundVolume = 100;
                            else if (soundVolume < 0)
                                soundVolume = 0;
                            master.storedDataManager.Settings.SoundVolume = soundVolume;
                            UnloadMenu();
                            LoadMenu("settings", master, true);
                            return;

                        case "change_musicvol":
                            int musicVolume = master.storedDataManager.Settings.MusicVolume;
                            musicVolume += int.Parse(signal.Subject);
                            if (musicVolume > 100)
                                musicVolume = 100;
                            else if (musicVolume < 0)
                                musicVolume = 0;
                            master.storedDataManager.Settings.MusicVolume = musicVolume;
                            UnloadMenu();
                            LoadMenu("settings", master, true);
                            return;

                        case "toggle_attack_type":
                            if (master.storedDataManager.Settings.AttackTowardsMouse)
                                master.storedDataManager.Settings.AttackTowardsMouse = false;
                            else
                                master.storedDataManager.Settings.AttackTowardsMouse = true;

                            UnloadMenu();
                            LoadMenu("settings", master, true);
                            return;

                        case "load_level":
                            string[] loadType = signal.Subject.Split(",");
                            string level = loadType[0];
                            int fromCheckpoint = int.Parse(loadType[1]);
                            game.ChangeState(new GameState());

                            master.mapManager.LoadMap(level, master, graphicsDevice);
                            Vector2 spawnpoint = master.mapManager.CurrentMap.GetSpawnpoint();
                            Vector2 checkpoint = master.mapManager.CurrentMap.GetCheckpoint();
                            master.entityManager.Player.Reset(master);
                            master.CurrentSpriteEffect = null;
                            master.CurrentMapEffect = null;
                            master.playGuiManager.ResetFadeoutSequence();
                            master.cameraManager.Camera.Position = Vector2.Zero;
                            if (fromCheckpoint == 0)
                                master.entityManager.Player.GoTo(spawnpoint);
                            else
                                master.entityManager.Player.GoTo(checkpoint);
                            MenuStack.Clear();
                            break;
                        case "change_state":
                            if (signal.Subject == "game")
                                game.ChangeState(new GameState());
                            MenuStack.Clear();
                            break;

                        case "exit_level":
                            LoadMenu($"level_select_{master.entityManager.Player.CurrentPlanet}", master);
                            CurrentMenu = $"level_select_{master.entityManager.Player.CurrentPlanet}";
                            MenuStack.Clear();
                            MenuStack.Add("planet_select");
                            master.storedDataManager.SaveFile();
                            break;
                        case "play_screen":
                            MenuStack.Insert(0, CurrentMenu);
                            int levelNum = int.Parse(signal.Subject);
                            master.entityManager.Player.CurrentLevel = levelNum;
                            if (levelNum < 10)
                                master.entityManager.Player.CurrentPlanet = "luxiar";
                            master.entityManager.Player.CurrentLevel = int.Parse(signal.Subject);
                            string playMenu = "play_screen";
                            CurrentMenu = playMenu;
                            foreach (FadingImage image in Components.MainScreen.FadingImages)
                            {
                                if (image.FadeOnExit == true)
                                {
                                    image.Fade();
                                    Frozen = true;
                                    PendingChange = true;
                                    return;
                                }
                            }
                            UnloadMenu();
                            LoadMenu(playMenu, master);
                            return;

                        case "change_menu":
                            MenuStack.Insert(0, CurrentMenu);

                            string newMenu = signal.Subject;
                            CurrentMenu = newMenu;
                            foreach (FadingImage image in Components.MainScreen.FadingImages)
                            {
                                if (image.FadeOnExit == true)
                                {
                                    image.Fade();
                                    Frozen = true;
                                    PendingChange = true;
                                    return;
                                }
                            }
                            foreach (ScrollScreen screen in Components.Screens)
                                master.savedScroll = new Point(-(int)screen.CurrentScroll.X, -(int)screen.CurrentScroll.Y);
                            UnloadMenu();
                            LoadMenu(newMenu, master);
                            return;

                        case "change_menu+reset_scroll":
                            MenuStack.Insert(0, CurrentMenu);

                            string newMenu2 = signal.Subject;
                            CurrentMenu = newMenu2;
                            foreach (FadingImage image in Components.MainScreen.FadingImages)
                            {
                                if (image.FadeOnExit == true)
                                {
                                    image.Fade();
                                    Frozen = true;
                                    PendingChange = true;
                                    return;
                                }
                            }
                            master.savedScroll = new Point(0,0);
                            UnloadMenu();
                            LoadMenu(newMenu2, master);
                            return;

                        case "purchase":
                            MenuStack.Insert(0, CurrentMenu);

                            string purchase = signal.Subject.Split("/")[0];
                            int cost = int.Parse(signal.Subject.Split("/")[1]);
                            string currency = signal.Subject.Split("/")[2];

                            int playerCurrencyAvailable = 0;
                            switch (currency)
                            {
                                case "coin":
                                {
                                    playerCurrencyAvailable = master.storedDataManager.CurrentSaveFile.Coins;
                                    break;
                                }
                                case "luxiar_soul":
                                {
                                    playerCurrencyAvailable = master.storedDataManager.CurrentSaveFile.LightSouls;
                                    break;
                                    }
                                case "gramen_soul":
                                    {
                                        playerCurrencyAvailable = master.storedDataManager.CurrentSaveFile.GrowthSouls;
                                        break;
                                    }
                                case "freone_soul":
                                    {
                                        playerCurrencyAvailable = master.storedDataManager.CurrentSaveFile.FrostSouls;
                                        break;
                                    }
                                case "umbrac_soul":
                                    {
                                        playerCurrencyAvailable = master.storedDataManager.CurrentSaveFile.ShadowSouls;
                                        break;
                                    }
                                case "inferni_soul":
                                    {
                                        playerCurrencyAvailable = master.storedDataManager.CurrentSaveFile.FlameSouls;
                                        break;
                                    }
                                case "ossium_soul":
                                    {
                                        playerCurrencyAvailable = master.storedDataManager.CurrentSaveFile.TormentSouls;
                                        break;
                                    }
                            }
                            if (!master.storedDataManager.CurrentSaveFile.Purchases.Contains(purchase) && playerCurrencyAvailable >= cost)
                            {
                                playerCurrencyAvailable -= cost;
                                master.storedDataManager.CurrentSaveFile.Purchases.Add(purchase);
                            }
                            switch (currency)
                            {
                                case "coin":
                                    {
                                        master.playGuiManager.ReloadCoinCounter(playerCurrencyAvailable);
                                        master.storedDataManager.CurrentSaveFile.Coins = playerCurrencyAvailable;
                                        break;
                                    }
                                case "luxiar_soul":
                                    {
                                        master.playGuiManager.ReloadSoulCounter(playerCurrencyAvailable);
                                        master.storedDataManager.CurrentSaveFile.LightSouls = playerCurrencyAvailable;
                                        break;
                                    }
                                case "gramen_soul":
                                    {
                                        master.playGuiManager.ReloadSoulCounter(playerCurrencyAvailable);
                                        master.storedDataManager.CurrentSaveFile.GrowthSouls = playerCurrencyAvailable;
                                        break;
                                    }
                                case "freone_soul":
                                    {
                                        master.playGuiManager.ReloadSoulCounter(playerCurrencyAvailable);
                                        master.storedDataManager.CurrentSaveFile.FrostSouls = playerCurrencyAvailable;
                                        break;
                                    }
                                case "umbrac_soul":
                                    {
                                        master.playGuiManager.ReloadSoulCounter(playerCurrencyAvailable);
                                        master.storedDataManager.CurrentSaveFile.ShadowSouls = playerCurrencyAvailable;
                                        break;
                                    }
                                case "inferni_soul":
                                    {
                                        master.playGuiManager.ReloadSoulCounter(playerCurrencyAvailable);
                                        master.storedDataManager.CurrentSaveFile.FlameSouls = playerCurrencyAvailable;
                                        break;
                                    }
                                case "ossium_soul":
                                    {
                                        master.playGuiManager.ReloadSoulCounter(playerCurrencyAvailable);
                                        master.storedDataManager.CurrentSaveFile.TormentSouls = playerCurrencyAvailable;
                                        break;
                                    }
                            }
                            master.storedDataManager.SaveFile();
                            UnloadMenu();
                            LoadMenu("buy_"+purchase, master, true);
                            return;

                        case "back_menu":
                            if (CurrentMenu == "settings")
                                master.storedDataManager.SaveSettings();
                            string backMenu = MenuStack[0];
                            MenuStack.RemoveAt(0);
                            CurrentMenu = backMenu;
                            foreach (FadingImage image in Components.MainScreen.FadingImages)
                            {
                                if (image.FadeOnExit == true)
                                {
                                    image.Fade();
                                    Frozen = true;
                                    PendingChange = true;
                                    return;
                                }
                            }

                            UnloadMenu();
                            LoadMenu(backMenu, master);
                            return;
                        case "change_weapon":
                            if (Attacks.MeleeOrRanged(signal.Subject) == "melee")
                            {
                                MeleeAttack weapon = Attacks.CreateMeleeAttack(signal.Subject);
                                master.entityManager.Player.PrimaryAttack = weapon;
                            }
                            else
                            {
                                RangedAttack weapon = Attacks.CreateRangeAttack(signal.Subject);
                                master.entityManager.Player.PrimaryAttack = weapon;
                            }
                            master.playGuiManager.ReloadWeaponIcons(master);
                            LoadMenu(CurrentMenu, master, true);
                            return;

                        case "load_save":
                            int loadNumber = int.Parse(signal.Subject);
                            master.storedDataManager.LoadSelectedFile(loadNumber);
                            MenuStack.Clear();

                            UnloadMenu();
                            if (master.storedDataManager.CurrentSaveFile.ShowIntro)
                                LoadMenu("intro1", master);
                            else
                                LoadMenu("planet_select", master);
                            return;

                        case "create_new_save_menu":
                            int createNumber = int.Parse(signal.Subject);
                            master.storedDataManager.CurrentSaveNumber = createNumber;
                            MenuStack.Insert(0, CurrentMenu);

                            CurrentMenu = "new_save_creator";
                            UnloadMenu();
                            LoadMenu(CurrentMenu, master);
                            return;


                        case "create_new_save":
                            if (!Components.MainScreen.TextInputs[0].Valid)
                                break;
                            string saveName = Components.MainScreen.TextInputs[0].Text;
                            master.storedDataManager.CreateFile(saveName);
                            MenuStack.Clear();
                            MenuStack.Add("titlescreen");
                            CurrentMenu = "save_selection";
                            UnloadMenu();
                            LoadMenu(CurrentMenu, master);
                            return;
                        case "exit":
                            game.Exit();
                            break;
                    }
                }
            }
        }

        public override void Update(MasterManager master, GameTime gameTime, SpextriaGame game, GraphicsDevice graphicsDevice)
        {
            foreach (FadingImage image in Components.MainScreen.FadingImages)
            {
                image.Update(gameTime);
                if (image.FreezeMenu && !image.FadeOnExit)
                    Frozen = true;
                if (image.DoneFading)
                {
                    Frozen = false;
                    image.DoneFading = false;
                    if (image.FadeOnExit && PendingChange)
                    {
                        PendingChange = false;
                        UnloadMenu();
                        LoadMenu(CurrentMenu, master);
                        return;
                    }
                }
            }

            if (Frozen)
                return;

            foreach (TextInput input in Components.MainScreen.TextInputs)
            {
                input.Update(master.MousePos, master.CurrentMouseState.LeftButton, gameTime, master.GraphicsPositionScale);
            }

            foreach (ScrollScreen screen in Components.Screens)
            {
                screen.Update(master.MousePos, master.CurrentMouseState);
            }

            List<IGuiButton> mainButtons = Components.MainScreen.Buttons;
            CheckButtons(master, game, mainButtons, graphicsDevice: graphicsDevice);
            CheckButtons(master, game, Components.MainScreen.Nodes);
            for (int i = Components.Screens.Count - 1; i >= 0; i--)
            {
                ScrollScreen screen = (ScrollScreen)Components.Screens[i];
                List<IGuiButton> otherButtons = new List<IGuiButton>();
                otherButtons.AddRange(screen.Buttons);
                otherButtons.AddRange(screen.Nodes);

                Point offset = new Point((int)screen.CurrentScroll.X, (int)screen.CurrentScroll.Y);
                CheckButtons(master, game, otherButtons, offset, graphicsDevice: graphicsDevice);
            }

        }

        public void UnloadMenu()
        {
            Components.MainScreen.TextInputs.Clear();
            Components.MainScreen.Buttons.Clear();
            Components.MainScreen.BasicComponents.Clear();
            Components.MainScreen.FadingImages.Clear();
            Components.Screens.Clear();
        }
        public void LoadMenu(string menu, MasterManager master = null, bool isReload = false)
        {
            Components = Menus.LoadMenu(menu, master, isReload);
        }

        public override void Draw(MasterManager master, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            const int scaleX = 8;//(float)_graphics.PreferredBackBufferWidth / 80;
            const int scaleY = 8;//(float)_graphics.PreferredBackBufferHeight / 48;
            Matrix matrix;

            if (master.GraphicsPositionScale != Vector2.One)
            {
                matrix = master.cameraManager.StaticCamera.GetViewMatrix();

                graphicsDevice.SetRenderTarget(master.MenuTarget);
                //graphicsDevice.SetRenderTarget(normScreen);
                spriteBatch.Begin(transformMatrix: matrix, samplerState: SamplerState.PointClamp);
                //_spriteBatch.Draw(_text.BoxImage, _text.Position, Color.White);

                Components.Draw(spriteBatch, master);

                //Debug.Draw(spriteBatch, master.cameraManager);

                spriteBatch.End();

                graphicsDevice.SetRenderTarget(null);

                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                spriteBatch.Draw(master.MenuTarget, new Rectangle(0, 0, graphicsDevice.DisplayMode.Width, graphicsDevice.DisplayMode.Height), Color.White);
                //spriteBatch.Draw(normScreen, new Rectangle(0, 0, graphicsDevice.DisplayMode.Width, graphicsDevice.DisplayMode.Height), Color.White);
                spriteBatch.End();
            }
            else // normal screen
            {

                Matrix matrix2 = Matrix.CreateScale(scaleX, scaleY, 0f);
                Matrix matrix1 = master.cameraManager.StaticCamera.GetViewMatrix();
                matrix = matrix1 + matrix2;

                graphicsDevice.SetRenderTarget(null);
                //graphicsDevice.SetRenderTarget(normScreen);
                spriteBatch.Begin(transformMatrix: matrix, samplerState: SamplerState.PointClamp);
                //_spriteBatch.Draw(_text.BoxImage, _text.Position, Color.White);

                Components.Draw(spriteBatch, master);
                spriteBatch.End();
            }
        }
    }
}
