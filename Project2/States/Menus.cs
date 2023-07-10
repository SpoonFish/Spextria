using Spextria.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using Spextria.Master;
using Microsoft.Xna.Framework;
using Spextria.Statics;
using Spextria.Graphics.GUI.Interactions;
using Spextria.StoredData;
using Spextria.Entities;
using System.ComponentModel.Design;

namespace Spextria.States
{
    static class Menus
    {
        // Different object for fade in/out because if you have both in one menu then their opacities would override each other constantly
        static private ImagePanel totalFadein = new ImagePanel(new Vector2(0, 0), Images.ImageDict["blackout"], new Vector2(426, 233));
        static private ImagePanel totalFadeout = new ImagePanel(new Vector2(0, 0), Images.ImageDict["blackout"], new Vector2(426, 234));

        static public GuiContent LoadMenu(string menuName, MasterManager master = null, bool isReload = false)
        {
            GuiContent components = new GuiContent();
            switch (menuName)
            {
                case "titlescreen": //Maybe preload these in statics?
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["stars"], new Vector2(426, 233)));
                    ImagePanel logo = new ImagePanel(new Vector2(0, 0), Images.ImageDict["spextria_logo"], new Vector2(426, 233));
                    components.MainScreen.BasicComponents.Add(logo);
                    //components.FadingImages.Add(new FadingImage(logo, "in", 1.5));
                    components.MainScreen.FadingImages.Add(new FadingImage(totalFadeout, "out", 2.5));
                    components.MainScreen.FadingImages.Add(new FadingImage(totalFadein, "in", 0.5, false, 0, true));
                    components.MainScreen.Buttons.Add(new Button("#white#PLAY", "#yellow#PLAY", new Vector2(306, 120), new Vector2(80, 25), new ButtonSignalEvent("change_menu", "save_selection")));
                    components.MainScreen.Buttons.Add(new Button("#white#settings", "#blue#settings", new Vector2(306, 155), new Vector2(80, 25), new ButtonSignalEvent("change_menu", "settings")));
                    components.MainScreen.Buttons.Add(new Button("#white#EXIT", "#red#EXIT", new Vector2(306, 190), new Vector2(80, 25), new ButtonSignalEvent("exit")));
                    break;

                case "settings":
                    components = AddFade(components, isReload);

                    components.MainScreen.BasicComponents.Add(new TextBox("Settings", new Vector2(176, 16), 200, "slanted", true));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("back_menu")));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));

                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(15, 55), new Vector2(190, 30), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"Music Vol: #yellow#{master.storedDataManager.Settings.MusicVolume}", new Vector2(20, 61), 200, "none", true));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(175, 60), new Vector2(20, 20), new ButtonSignalEvent("change_musicvol", "-10")));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(145, 60), new Vector2(20, 20), new ButtonSignalEvent("change_musicvol", "10")));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(174, 59), Images.ImageDict["minus"]));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(144, 59), Images.ImageDict["plus"]));

                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(220, 55), new Vector2(190, 30), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"Sound Vol: #yellow#{master.storedDataManager.Settings.SoundVolume}", new Vector2(225, 61), 200, "none", true));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(380, 60), new Vector2(20, 20), new ButtonSignalEvent("change_soundvol", "-10")));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(350, 60), new Vector2(20, 20), new ButtonSignalEvent("change_soundvol", "10")));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(379, 59), Images.ImageDict["minus"]));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(349, 59), Images.ImageDict["plus"]));

                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(15, 100), new Vector2(395, 30), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"ATTACK TOWARDS:", new Vector2(20, 106), 200, "none", true));
                    if (!master.storedDataManager.Settings.AttackTowardsMouse)
                        components.MainScreen.Buttons.Add(new Button("PLAYER DIRECTION", "#grey#PLAYER DIRECTION", new Vector2(165, 105), new Vector2(170, 20), new ButtonSignalEvent("toggle_attack_type")));
                    else
                        components.MainScreen.Buttons.Add(new Button("MOUSE POINTER", "#grey#MOUSE POINTER", new Vector2(165, 105), new Vector2(170, 20), new ButtonSignalEvent("toggle_attack_type")));

                    components.MainScreen.Buttons.Add(new Button("RESET ALL", "#red#RESET ALL", new Vector2(300, 10), new Vector2(100, 30), new ButtonSignalEvent("reset_settings"), "button_red", "button_red_hovered", "button_red_clicked"));
                    break;

                case "save_selection":
                    components = AddFade(components, isReload);

                    components.MainScreen.BasicComponents.Add(new TextBox("Select a #yellow#save file #white#to play", new Vector2(100, 16), 200, "slanted", true));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("change_menu", "titlescreen")));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));

                    int i = 0;
                    foreach (SaveFile save in master.storedDataManager.SaveFiles)
                    {
                        components.MainScreen.BasicComponents.Add(new TextBox($"Slot {i + 1}", new Vector2(50 + i * 137, 57), 200, "none", true));
                        if (save.New)
                            components.MainScreen.Buttons.Add(new Button("! EMPTY !", "#yellow#CREATE NEW", new Vector2(17 + i * 137, 55), new Vector2(120, 170), new ButtonSignalEvent("create_new_save_menu", $"{i + 1}"), "button", "button_hovered", "button_clicked", true, new Vector2(0, 40)));
                        else
                        {
                            components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(60 + i * 137, 85), Images.ImageDict[save.Character + "_animated"], new Vector2(16 * 2, 32 * 2)));
                            components.MainScreen.Buttons.Add(new Button(save.Name, "#yellow#" + save.Name, new Vector2(17 + i * 137, 55), new Vector2(120, 170), new ButtonSignalEvent("load_save", $"{i + 1}"), "button", "button_hovered", "button_clicked", true, new Vector2(0, 40)));
                        }
                        i++;
                    }
                    //components.MainScreen.Buttons.Add(new Button(save.Name, "#yellow#"+ save1.Name, new Vector2(17, 55), new Vector2(120, 170), new ButtonSignalEvent("", "")));
                    //components.MainScreen.Buttons.Add(new Button(save.Name, "#yellow#" + save2.Name, new Vector2(152, 55), new Vector2(120, 170), new ButtonSignalEvent("", "")));
                    //components.MainScreen.Buttons.Add(new Button(save.Name, "#yellow#" + save3.Name, new Vector2(292, 55), new Vector2(120, 170), new ButtonSignalEvent("", "")));
                    break;

                case "new_save_creator":
                    components.MainScreen.TextInputs.Add(new TextInput("ENTER NAME...", new Vector2(213 - 75, 100), 150, 12, 2));
                    components.MainScreen.BasicComponents.Add(new TextBox("Create new #yellow# save file#white# !", new Vector2(120, 16), 200, "slanted", true));
                    components.MainScreen.Buttons.Add(new Button("CREATE", "#green#CREATE", new Vector2(213 - 50, 180), new Vector2(100, 40), new ButtonSignalEvent("create_new_save", "")));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("back_menu", "")));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));
                    break;

                case "intro1":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["stars"], new Vector2(426, 233)));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "intro2"), "none", "none", "none"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"Spextria was once a planet of great {Rainbowify("colour")}", new Vector2(250, 30), 120, "black", true));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["spextria_planet"], new Vector2(426, 233)));
                    components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["spextria_planet_coloured"], new Vector2(426, 233)), "in", 5, false, 2));

                    components = AddFade(components, isReload);
                    break;

                case "intro2":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["stars"], new Vector2(426, 233)));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "intro3"), "none", "none", "none"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"One day a strange star appeared in the sky and the {Rainbowify("colour")} began to #grey#fade", new Vector2(15, 30), 200, "black", true));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["house_scene_coloured"], new Vector2(426, 233)));
                    components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["house_scene"], new Vector2(426, 233)), "in", 9, false, 1));

                    components = AddFade(components, isReload);
                    break;

                case "intro3":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["stars"], new Vector2(426, 233)));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "intro4"), "none", "none", "none"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"This was no normal star, it was an ancient cosmic entity only heard of in legends...                   #red#malum#white#, a corrupted demigod of {Rainbowify("colour")}", new Vector2(15, 30), 120, "black", true));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["daemalicus_scene"], new Vector2(426, 233)));

                    components = AddFade(components, isReload);
                    break;

                case "intro4":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["stars"], new Vector2(426, 233)));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "intro5"), "none", "none", "none"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"The colours of Spextria divided into 6 new planets, each with its own nature: #yellow#light #green#growth #blue#frost #purple#shadow #orange#flame #red#torment", new Vector2(278, 30), 80, "black", true));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["planet_split_scene_before"], new Vector2(426, 233)));
                    components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["planet_split_scene"], new Vector2(426, 233)), "in", 5, false, 3));

                    components = AddFade(components, isReload);
                    break;

                case "intro5":
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "intro6"), "none", "none", "none"));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["inanimate_scene"], new Vector2(426, 233)));
                    components.MainScreen.BasicComponents.Add(new TextBox($"Those who did not leave spextria in time or protect themselves became frozen in time, #grey#inanimate...", new Vector2(15, 30), 120, "none", true));


                    components = AddFade(components, isReload);
                    break;

                case "intro6":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["stars"], new Vector2(426, 233)));
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["luxiar_escape_scene"], new Vector2(426, 233)));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "intro7"), "none", "none", "none"));
                    components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["escape_scene_later"], new Vector2(426, 233)), "in", 1, false, 3));
                    components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["escape_scene"], new Vector2(426, 233)), "out", 1, false, 3));
                    components.MainScreen.BasicComponents.Add(new TextBox($"The few that escaped fled to #yellow#luxiar#white#, the planet of #yellow#light#white#. It is the safest planet to live in out of the 6.                   But life for them had completely changed...", new Vector2(15, 18), 120, "black", true));


                    components = AddFade(components, isReload);
                    break;

                case "intro7":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["lab_scene"], new Vector2(426, 233)));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "intro8"), "none", "none", "none"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"Over time, scientists designed a weapon that could absorb and manipulate {Rainbowify("colour")} in hope that they could undo the wrath of #red#daemalicus", new Vector2(5, 5), 330, "none", true));

                    components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["lab_scene_fade"], new Vector2(426, 233)), "in", 0.5, false, 0, true));
                    if (!isReload)
                        components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["blackout"], new Vector2(426, 233)), "out", 0.5, true));
                    break;

                case "intro8":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["lab_scene_after"], new Vector2(426, 233)));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "intro9"), "none", "none", "none"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"This experimental weapon is called     #yellow#''EXPERIMENT: S.O.L.U.M'' #white#and is designed to travel to and survive in the other planets with its unique ability and obtain power to fix spextria", new Vector2(5, 5), 330, "none", true));

                    components = AddFade(components, isReload, 0.5, "lab_scene_fade");
                    break;

                case "intro9":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["lab_scene_final"], new Vector2(426, 233)));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "intro10"), "none", "none", "none"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"The #yellow#guardians #white#of each planet hold enough {Rainbowify("spectral power")} to revert spextria back to normal", new Vector2(5, 5), 330, "none", true));


                    components = AddFade(components, isReload, 0.5, "lab_scene_fade2");
                    break;

                case "intro10":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["lab_scene_final2"], new Vector2(426, 233)));
                    master.storedDataManager.CurrentSaveFile.ShowIntro = false;
                    master.storedDataManager.SaveFile();
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "planet_select"), "none", "none", "none"));
                    components.MainScreen.BasicComponents.Add(new TextBox($"But whether this mission fails or succeeds will be up to #yellow#you#white#!", new Vector2(4, 5), 330, "none", true));

                    components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["lab_scene_final3"], new Vector2(426, 233)), "in", 2, true, 5));
                    components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["lab_scene_fade2"], new Vector2(426, 233)), "in", 2, true, 8));
                    components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["blackout"], new Vector2(426, 233)), "in", 3, false, 0, true));
                    if (!isReload)
                        components.MainScreen.FadingImages.Add(new FadingImage(new ImagePanel(new Vector2(0, 0), Images.ImageDict["lab_scene_fade2"], new Vector2(426, 233)), "out", 0.5, true));
                    break;

                case "planet_select":
                    master.savedScroll = new Point();
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("change_menu", "titlescreen")));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(320, 10), new Vector2(100, 30), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["stars"], new Vector2(426, 233)));
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["planet_select"], new Vector2(426, 233)));

                    Vector2 planet_pos = new Vector2(194, 66);
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(planet_pos, Images.ImageDict["locked_luxiar"], new Vector2(57, 57)));
                    components.MainScreen.Buttons.Add(new Button("", "#yellow#LUXIAR", new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent(), "none", "none", "none", true, new Vector2(-planet_pos.X - 10 + 348, -planet_pos.Y - 10 + 5)));
                    if (master.storedDataManager.CurrentSaveFile.PlanetsUnlocked >= 1)
                        components.MainScreen.Buttons.Add(new ButtonImage(new ImagePanel(planet_pos, Images.ImageDict["luxiar"], new Vector2(57, 57)), new ImagePanel(planet_pos, Images.ImageDict["selected_luxiar"], new Vector2(57, 57)), new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent("change_menu", "level_select_luxiar")));

                    planet_pos = new Vector2(167, 153);
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(planet_pos, Images.ImageDict["locked_gramen"], new Vector2(57, 57)));
                    components.MainScreen.Buttons.Add(new Button("", "#green#GRAMEN", new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent(), "none", "none", "none", true, new Vector2(-planet_pos.X - 10 + 348, -planet_pos.Y - 10 + 5)));
                    if (master.storedDataManager.CurrentSaveFile.PlanetsUnlocked >= 2)
                        components.MainScreen.Buttons.Add(new ButtonImage(new ImagePanel(planet_pos, Images.ImageDict["gramen"], new Vector2(57, 57)), new ImagePanel(planet_pos, Images.ImageDict["selected_gramen"], new Vector2(57, 57)), new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent("change_menu", "level_select_gramen")));

                    planet_pos = new Vector2(78, 170);
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(planet_pos, Images.ImageDict["locked_freone"], new Vector2(57, 57)));
                    components.MainScreen.Buttons.Add(new Button("", "#blue#FREONE", new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent(), "none", "none", "none", true, new Vector2(-planet_pos.X - 10 + 348, -planet_pos.Y - 10 + 5)));
                    if (master.storedDataManager.CurrentSaveFile.PlanetsUnlocked >= 3)
                        components.MainScreen.Buttons.Add(new ButtonImage(new ImagePanel(planet_pos, Images.ImageDict["freone"], new Vector2(57, 57)), new ImagePanel(planet_pos, Images.ImageDict["selected_freone"], new Vector2(57, 57)), new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent("change_menu", "level_select_freone")));

                    planet_pos = new Vector2(16, 108);
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(planet_pos, Images.ImageDict["locked_umbrac"], new Vector2(57, 57)));
                    components.MainScreen.Buttons.Add(new Button("", "#purple#UMBRAC", new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent(), "none", "none", "none", true, new Vector2(-planet_pos.X - 10 + 348, -planet_pos.Y - 10 + 5)));
                    if (master.storedDataManager.CurrentSaveFile.PlanetsUnlocked >= 4)
                        components.MainScreen.Buttons.Add(new ButtonImage(new ImagePanel(planet_pos, Images.ImageDict["umbrac"], new Vector2(57, 57)), new ImagePanel(planet_pos, Images.ImageDict["selected_umbrac"], new Vector2(57, 57)), new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent("change_menu", "level_select_umbrac")));

                    planet_pos = new Vector2(41, 23);
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(planet_pos, Images.ImageDict["locked_inferni"], new Vector2(57, 57)));
                    components.MainScreen.Buttons.Add(new Button("", "#orange#INFERNI", new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent(), "none", "none", "none", true, new Vector2(-planet_pos.X - 10 + 346, -planet_pos.Y - 10 + 5)));
                    if (master.storedDataManager.CurrentSaveFile.PlanetsUnlocked >= 5)
                        components.MainScreen.Buttons.Add(new ButtonImage(new ImagePanel(planet_pos, Images.ImageDict["inferni"], new Vector2(57, 57)), new ImagePanel(planet_pos, Images.ImageDict["selected_inferni"], new Vector2(57, 57)), new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent("change_menu", "level_select_inferni")));

                    planet_pos = new Vector2(135, 5);
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(planet_pos, Images.ImageDict["locked_ossium"], new Vector2(57, 57)));
                    components.MainScreen.Buttons.Add(new Button("", "#red#OSSIUM", new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent(), "none", "none", "none", true, new Vector2(-planet_pos.X - 10 + 348, -planet_pos.Y - 10 + 5)));
                    if (master.storedDataManager.CurrentSaveFile.PlanetsUnlocked >= 6)
                        components.MainScreen.Buttons.Add(new ButtonImage(new ImagePanel(planet_pos, Images.ImageDict["ossium"], new Vector2(57, 57)), new ImagePanel(planet_pos, Images.ImageDict["selected_ossium"], new Vector2(57, 57)), new Vector2(planet_pos.X + 10, planet_pos.Y + 10), new Vector2(40, 40), new ButtonSignalEvent("change_menu", "level_select_ossium")));

                    components = AddFade(components, isReload);
                    break;

                case "play_screen":
                    components.MainScreen.BasicComponents.Add(new TextBox($"LEVEL {master.entityManager.Player.CurrentLevel}", new Vector2(213 - 34, 11), 100, "none", true));
                    components.MainScreen.BasicComponents.Add(new TextBox("#grey#"+LevelInfo.LevelNames[master.entityManager.Player.CurrentLevel.ToString()], new Vector2(213 - (int)(LevelInfo.LevelNames[master.entityManager.Player.CurrentLevel.ToString()].Length*3.5)-18, 38), 400, "none", true));
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["stars"], new Vector2(426, 233)));
                    components.MainScreen.Buttons.Add(new Button("START FROM:", "START FROM:", new Vector2(213 - 100, 116 - 50), new Vector2(200, 125), new ButtonSignalEvent(), "button", "button", "button", true, new Vector2(0, -40)));

                    if (master.storedDataManager.CurrentSaveFile.Checkpoint > 0 || master.storedDataManager.CurrentSaveFile.CurrentLevel > master.entityManager.Player.CurrentLevel)
                        components.MainScreen.Buttons.Add(new Button("#white#CHECKPOINT", "#yellow#CHECKPOINT", new Vector2(213 - 60, 116), new Vector2(120, 20), new ButtonSignalEvent("load_level", $"{master.entityManager.Player.CurrentLevel},1"), "button_yellow", "button_yellow_hovered", "button_yellow_clicked"));
                    else
                    {
                        components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(213-58, 116), Images.ImageDict["lock"]));
                        components.MainScreen.Buttons.Add(new Button("#grey#CHECKPOINT", "#grey#CHECKPOINT", new Vector2(213 - 60, 116), new Vector2(120, 20), new ButtonSignalEvent(), "button_grey", "button_grey", "button", true, new Vector2(13, 0)));
                    }
                    components.MainScreen.Buttons.Add(new Button("#white#START", "#green#START", new Vector2(213 - 60, 116 + 35), new Vector2(120, 20), new ButtonSignalEvent("load_level", $"{master.entityManager.Player.CurrentLevel},0"), "button_green", "button_green_hovered", "button_green_clicked"));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("back_menu", "")));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));
                    components = AddFade(components, isReload, time: 0.25);
                    break;

                case "level_select_luxiar":
                    components.Screens.Add(new ScrollScreen(new Vector2(0,0), new Vector2(426,233), new Vector2(928-404,480-211), new Vector2(-22,-22), master.savedScroll));
                    components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(0,0), Images.ImageDict["luxiar_landscape"], new Vector2(928, 480)));
                    //components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(50, 50), Images.ImageDict["lx_level_current"], new Vector2(40, 30)));
                    components = AddLevel(components, 1, new Vector2(176,170), master);
                    components = AddLevel(components, 2, new Vector2(304, 122), master);
                    components = AddLevel(components, 3, new Vector2(428, 126), master);
                    components = AddLevel(components, 4, new Vector2(498, 126), master);
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(-10, -8), new Vector2(426+20, 30), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(-8 , -10), new Vector2(30, 233+20), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(426-20, -10), new Vector2(30, 233+20), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(-10, 233 - 20), new Vector2(426 + 20, 30), new ButtonSignalEvent(), "button", "button", "button"));

                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(-8, -8), new Vector2(80, 45), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(426-70, -8), new Vector2(80, 45), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(-8, 233-35), new Vector2(80, 45), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(426 - 70, 233-35), new Vector2(80, 45), new ButtonSignalEvent(), "button", "button", "button"));

                    components.MainScreen.Buttons.Add(new Button("JOURNAL", Rainbowify("JOURNAL"), new Vector2(330, 213-15), new Vector2(104, 24), new ButtonSignalEvent("change_menu", "journal_menu")));

                    components.MainScreen.Buttons.Add(new Button("UPGRADES", Rainbowify("UPGRADES"), new Vector2(330, 13), new Vector2(104, 24), new ButtonSignalEvent("change_menu", "upgrade_menu")));

                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("change_menu", "planet_select")));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));

                    components.MainScreen.BasicComponents.Add(new TextBox("#yellow#LUXIAR", new Vector2(213 -25, 13), 100, "slanted", true));
                    components = AddFade(components, isReload);
                    break;

                case "journal_menu":
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("change_menu+reset_scroll", "level_select_" + master.entityManager.Player.CurrentPlanet)));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));

                    components.MainScreen.Buttons.Add(new Button("LIZARD ISSUE", "#yellow#LIZARD ISSUE", new Vector2(213-75, 10), new Vector2(150, 30), new ButtonSignalEvent("change_menu", "lizard_issue-1")));
                    break;

                case "upgrade_menu":
                    master.savedScroll = new Point(500 - 213, 500 - 146);
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("change_menu+reset_scroll", "level_select_"+master.entityManager.Player.CurrentPlanet)));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));

                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(-4, 36), new Vector2(434, 233), new ButtonSignalEvent(), "button", "button", "button"));
                    components.MainScreen.Buttons.Add(new Button("SHOP", "SHOP", new Vector2(54, 10), new Vector2(130, 22), new ButtonSignalEvent(), "tab_button", "tab_button_hovered", "tab_button_clicked"));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(99, 60), Images.ImageDict["power_4"], new Vector2(60,60)));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(99, 150), Images.ImageDict["scanner"], new Vector2(60, 60)));
                    components.MainScreen.Buttons.Add(new Button("#grey#SKILLS", "SKILLS", new Vector2(426-54-130, 10), new Vector2(130, 19), new ButtonSignalEvent("change_menu", "skill_select_menu"), "tab_button", "tab_button_hovered", "tab_button_clicked"));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(426 - 64 + 35- 130, 60), Images.ImageDict["slash"], new Vector2(60, 60)));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(426 -64 + 35 - 130, 150), Images.ImageDict["smash"], new Vector2(60, 60)));
                    components.MainScreen.Buttons.Add(new Button("TECH", "#blue#TECH", new Vector2(64, 50), new Vector2(130, 170), new ButtonSignalEvent("change_menu", "upgrades_tree")));
                    components.MainScreen.Buttons.Add(new Button("WEAPONS", "#yellow#WEAPONS", new Vector2(426 - 64 - 130, 50), new Vector2(130, 170), new ButtonSignalEvent("change_menu", "attacks_tree")));

                    break;
                case "skill_select_menu":
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("change_menu", "level_select_" + master.entityManager.Player.CurrentPlanet)));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));

                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(-4, 36), new Vector2(434, 233), new ButtonSignalEvent("change_menu", "planet_select"), "button", "button", "button"));
                    components.MainScreen.Buttons.Add(new Button("#grey#SHOP", "SHOP", new Vector2(54, 10), new Vector2(130, 19), new ButtonSignalEvent("change_menu", "upgrade_menu"), "tab_button", "tab_button_hovered", "tab_button_clicked"));
                    components.MainScreen.Buttons.Add(new Button("SKILLS", "SKILLS", new Vector2(426 - 54 - 130, 10), new Vector2(130, 22), new ButtonSignalEvent(), "tab_button", "tab_button_hovered", "tab_button_clicked"));
                    break;

                case "upgrades_tree":
                    
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("change_menu", "upgrade_menu")));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));

                    components.Screens.Add(new ScrollScreen(new Vector2(0, 0), new Vector2(426, 233), new Vector2(1000 - 426, 1000 - 233), new Vector2(0, 0), master.savedScroll));//, new Point(500 - 213, 500 - 116)));

                    components.Screens[0].BasicComponents.Add(new TextBox("#grey#USE WASD OR ARROW KEYS TO NAVIGATE", new Vector2(500 - 70, 500 - 130), 100, "none", true));

                    components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["upgrades_tree"], new Vector2(1000, 1000)));
                    
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "scanner", new List<string>() { "" }, new Vector2(481, 482), new ButtonSignalEvent("change_menu", "buy_scanner"), master));

                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "attraction_1", new List<string>() { "" }, new Vector2(372, 482), new ButtonSignalEvent("change_menu", "buy_attraction_1"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "attraction_2", new List<string>() { "attraction_1" }, new Vector2(309, 482), new ButtonSignalEvent("change_menu", "buy_attraction_2"), master));

                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "power_1", new List<string>() { "scanner" }, new Vector2(543, 419), new ButtonSignalEvent("change_menu", "buy_power_1"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "spectral_battery_1", new List<string>() { "scanner" }, new Vector2(543, 545), new ButtonSignalEvent("change_menu", "buy_spectral_battery_1"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "soul_collector_1", new List<string>() { "scanner" }, new Vector2(419, 545), new ButtonSignalEvent("change_menu", "buy_soul_collector_1"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "resilience_1", new List<string>() { "scanner" }, new Vector2(419, 419), new ButtonSignalEvent("change_menu", "buy_resilience_1"), master));
                    
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "power_2", new List<string>() { "power_1" }, new Vector2(586, 376), new ButtonSignalEvent("change_menu", "buy_power_2"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "spectral_battery_2", new List<string>() { "spectral_battery_1" }, new Vector2(586, 588), new ButtonSignalEvent("change_menu", "buy_spectral_battery_2"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "soul_collector_2", new List<string>() { "soul_collector_1" }, new Vector2(376, 588), new ButtonSignalEvent("change_menu", "buy_soul_collector_2"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "resilience_2", new List<string>() { "resilience_1" }, new Vector2(376, 376), new ButtonSignalEvent("change_menu", "buy_resilience_2"), master));

                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "power_3", new List<string>() { "power_2" }, new Vector2(629, 333), new ButtonSignalEvent("change_menu", "buy_power_3"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "spectral_battery_3", new List<string>() { "spectral_battery_2" }, new Vector2(629, 631), new ButtonSignalEvent("change_menu", "buy_spectral_battery_3"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "soul_collector_3", new List<string>() { "soul_collector_2" }, new Vector2(333, 631), new ButtonSignalEvent("change_menu", "buy_soul_collector_3"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "resilience_3", new List<string>() { "resilience_2" }, new Vector2(333, 333), new ButtonSignalEvent("change_menu", "buy_resilience_3"), master));

                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "power_4", new List<string>() { "power_3" }, new Vector2(672, 290), new ButtonSignalEvent("change_menu", "buy_power_4"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "spectral_battery_4", new List<string>() { "spectral_battery_3" }, new Vector2(672, 674), new ButtonSignalEvent("change_menu", "buy_spectral_battery_4"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "soul_collector_4", new List<string>() { "soul_collector_3" }, new Vector2(290, 674), new ButtonSignalEvent("change_menu", "buy_soul_collector_4"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "resilience_4", new List<string>() { "resilience_3" }, new Vector2(290, 290), new ButtonSignalEvent("change_menu", "buy_resilience_4"), master));

                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "power_5", new List<string>() { "power_4" }, new Vector2(715, 247), new ButtonSignalEvent("change_menu", "buy_power_5"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "spectral_battery_5", new List<string>() { "spectral_battery_4" }, new Vector2(715, 717), new ButtonSignalEvent("change_menu", "buy_spectral_battery_5"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "soul_collector_5", new List<string>() { "soul_collector_4" }, new Vector2(247, 717), new ButtonSignalEvent("change_menu", "buy_soul_collector_5"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "resilience_5", new List<string>() { "resilience_4" }, new Vector2(247, 247), new ButtonSignalEvent("change_menu", "buy_resilience_5"), master));

                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "better_repair_cells", new List<string>() { "resilience_3" }, new Vector2(375, 294), new ButtonSignalEvent("change_menu", "buy_better_repair_cells"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "better_energy_cells", new List<string>() { "spectral_battery_2" }, new Vector2(544, 627), new ButtonSignalEvent("change_menu", "buy_better_energy_cells"), master));


                    break;



                case "attacks_tree":

                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("change_menu", "upgrade_menu")));
                    components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));

                    components.Screens.Add(new ScrollScreen(new Vector2(0, 0), new Vector2(426, 233), new Vector2(1000 - 426, 1000 - 233), new Vector2(0, 0), master.savedScroll));//, new Point(500 - 213, 500 - 116)));

                    components.Screens[0].BasicComponents.Add(new TextBox("#grey#USE WASD OR ARROW KEYS TO NAVIGATE", new Vector2(500 + 70, 500 - 130), 80, "none", true));
                    components.Screens[0].BasicComponents.Add(new TextBox("#grey#CLICK THE #yellow#?#grey# NODES FOR INFORMATION", new Vector2(500 - 170, 500 - 130), 70, "none", true));

                    components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["attacks_tree"], new Vector2(1000, 1000)));

                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "steel_fists", new List<string>() { "" }, new Vector2(481, 482), new ButtonSignalEvent("change_menu", "buy_steel_fists"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "slash", new List<string>() { "steel_fists" }, new Vector2(521,441), new ButtonSignalEvent("change_menu", "buy_slash"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "smash", new List<string>() { "steel_fists" }, new Vector2(521,523), new ButtonSignalEvent("change_menu", "buy_smash"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "stab", new List<string>() { "steel_fists" }, new Vector2(441,523), new ButtonSignalEvent("change_menu", "buy_stab"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "skew", new List<string>() { "steel_fists" }, new Vector2(441,441), new ButtonSignalEvent("change_menu", "buy_skew"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("neutral", "bolt", new List<string>() { "steel_fists" }, new Vector2(285, 482), new ButtonSignalEvent("change_menu", "buy_bolt"), master));

                    components.Screens[0].Nodes.Add(new SkillNode("info", "info", new List<string>() { "" }, new Vector2(570, 441), new ButtonSignalEvent("change_menu", "slash_info"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("info", "info", new List<string>() { "" }, new Vector2(570, 523), new ButtonSignalEvent("change_menu", "smash_info"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("info", "info", new List<string>() { "" }, new Vector2(392, 441), new ButtonSignalEvent("change_menu", "skew_info"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("info", "info", new List<string>() { "" }, new Vector2(392, 523), new ButtonSignalEvent("change_menu", "stab_info"), master));
                    components.Screens[0].Nodes.Add(new SkillNode("info", "info", new List<string>() { "" }, new Vector2(234, 482), new ButtonSignalEvent("change_menu", "range_info"), master));

                    break;

                case "pause_menu":
                    components.MainScreen.BasicComponents.Add(new TextBox($"LEVEL {master.entityManager.Player.CurrentLevel}", new Vector2(213 - 34, 38), 100, "none", true));
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["stars"], new Vector2(426, 233)));
                    
                    components.MainScreen.Buttons.Add(new Button("#white#RESUME", "#yellow#RESUME", new Vector2(213 - 60, 116), new Vector2(120, 20), new ButtonSignalEvent("change_state", "game"), "button_yellow", "button_yellow_hovered", "button_yellow_clicked"));
                    
                    components.MainScreen.Buttons.Add(new Button("#white#EXIT", "#red#EXIT", new Vector2(213 - 60, 116+35), new Vector2(120, 20), new ButtonSignalEvent("exit_level"), "button_red", "button_red_hovered", "button_red_clicked"));
                    
                    break;

                case "pri_weapon_select":

                    components.Screens.Add(new ScrollScreen(new Vector2(213, 0), new Vector2(213, 233), new Vector2(213, 250), new Vector2(0, -3), new Point(0, -3), false, true, true));
                    components = AddWeaponSlots(isReload, master, components);
                    components.MainScreen.BasicComponents.Add(new TextBox($"#grey#i n f o", new Vector2(213 - 170, 61), 200, "black", true));
                    components.MainScreen.BasicComponents.Add(new TextBox($"SELECT PRIMARY WEAPON", new Vector2(213 - 200, 11), 200, "none", true));
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["half_stars"], new Vector2(213, 233)));
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 47), Images.ImageDict["weapon_button"], new Vector2(41,38)));
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(1, 53), Images.ImageDict[master.entityManager.Player.PrimaryAttack.Name], new Vector2(24, 25)));
                    components.MainScreen.Buttons.Add(new Button("#white#OK", "#yellow#OK", new Vector2(213 - 165, 206), new Vector2(120, 20), new ButtonSignalEvent("change_state", "game"), "button_yellow", "button_yellow_hovered", "button_yellow_clicked"));

                    break;

                case "level_failed":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["level_failed"], new Vector2(426, 233)));
                    //components.MainScreen.BasicComponents.Add(new TextBox($"LEVEL FAILED", new Vector2(213 - 54, 38), 100, "none", true));
                    string checkpoint = master.storedDataManager.CurrentSaveFile.Checkpoint.ToString();
                    if (master.storedDataManager.CurrentSaveFile.CurrentLevel > master.entityManager.Player.CurrentLevel)
                        checkpoint = "1";
                    components.MainScreen.Buttons.Add(new Button("#white#RESPAWN", "#yellow#RESPAWN", new Vector2(213 - 40, 142), new Vector2(80, 30), new ButtonSignalEvent("load_level", master.entityManager.Player.CurrentLevel.ToString()+","+checkpoint), "button_yellow", "button_yellow_hovered", "button_yellow_clicked"));


                    components.MainScreen.Buttons.Add(new Button("#white#EXIT TO LEVEL SELECT", "#red#EXIT TO LEVEL SELECT", new Vector2(213 - 90, 190), new Vector2(180, 30), new ButtonSignalEvent("exit_level"), "button_red", "button_red_hovered", "button_red_clicked"));
                    components = AddFade(components, isReload, 1f);
                    break;

                case "level_won":
                    string nextMenu = AddCutsceneTrigger(master, 2, "lizard_issue");

                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["level_completed"], new Vector2(426, 233)));
                    //components.MainScreen.BasicComponents.Add(new TextBox($"LEVEL FAILED", new Vector2(213 - 54, 38), 100, "none", true));

                    if (nextMenu == "exit")
                        components.MainScreen.Buttons.Add(new Button("#white#EXIT TO LEVEL SELECT", "#yellow#EXIT TO LEVEL SELECT", new Vector2(213 - 110, 156), new Vector2(220, 30), new ButtonSignalEvent("exit_level"), "button_yellow", "button_yellow_hovered", "button_yellow_clicked"));
                    else
                        components.MainScreen.Buttons.Add(new Button("#white#EXIT TO LEVEL SELECT", "#green#EXIT TO LEVEL SELECT", new Vector2(213 - 110, 156), new Vector2(220, 30), new ButtonSignalEvent("change_menu",nextMenu), "button_green", "button_green_hovered", "button_green_clicked"));

                    components = AddFade(components, isReload, 1f);
                    break;



                case "lizard_issue-1":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["inter_level_dialog"], new Vector2(426, 233)));
                    components.MainScreen.BasicComponents.Add(new TextBox($"#blue#! #grey#INCOMING MESSAGE #blue#!", new Vector2(160, 10), 330, "none", true));
                    components.MainScreen.BasicComponents.Add(new TextBox($"GREAT WORK OUT THERE #yellow#S.O.L.U.M#white#. THAT WAS YOUR FIRST AND FINAL TEST MISSION OUTSIDE THE WALLS OF THE CITY. ALL SYSTEMS AND CONTROLS WORKED SUCCESSFULLY.", new Vector2(140, 40), 230, "none", true));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "lizard_issue-2"), "none", "none", "none"));


                    components = AddFade(components, isReload, 0.5, "blackout");
                    break;

                case "lizard_issue-2":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["inter_level_dialog"], new Vector2(426, 233)));
                    components.MainScreen.BasicComponents.Add(new TextBox($"NOW OUR MAIN GOAL IS TO SAVE {Rainbowify("SPEXTRIA")}, OBVIOUSLY, BUT THERES A LONG JOURNEY AHEAD OF US UNTIL THAT HAPPENS. \\ \\FOR NOW, YOUR NEXT MISSION IS TO HELP US OUT WITH A DANGEROUS LIZARD COLONY NEARBY THREATENING THE SAFETY OF THE CITY.", new Vector2(140, 10), 230, "none", true));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("change_menu", "lizard_issue-3"), "none", "none", "none"));


                    components = AddFade(components, isReload, 0.5, "blackout");
                    break;
                case "lizard_issue-3":
                    components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["inter_level_dialog_approving"], new Vector2(426, 233)));
                    components.MainScreen.BasicComponents.Add(new TextBox($"ALL WE KNOW IS THAT THEIR MAIN SETTLEMENT IS SOMEWHERE TO THE EAST. NONE OF OUR SCOUTS HAVE MADE IT FAR ENOUGH TO GATHER MUCH INFORMATION AS THE AREA HAS A HIGH ENEMY PRESENCE.\\ \\YOU SHOULD BE STRONG ENOUGH TO FIGHT OFF THE LIZARDS, UNLIKE US HUMANS...", new Vector2(140, 10), 230, "none", true));
                    components.MainScreen.Buttons.Add(new Button("", "", new Vector2(0, 0), new Vector2(426, 233), new ButtonSignalEvent("exit_level"), "none", "none", "none"));


                    components = AddFade(components, isReload, 0.5, "blackout");
                    break;


                case "skew_info":
                    AddBuyLayout(isReload, master, components, "info", "SKEW ATTACKS ARE EFFECTIVE DUE TO THEIR LONG RANGE AND PIERCING ATTRIBUTES ALLOWING YOU TO FIGHT ENEMIES FROM A SAFER DISTANCE", 99999, "coin", "attacks_tree");
                    break;
                case "stab_info":
                    AddBuyLayout(isReload, master, components, "info", "STAB ATTACKS RELY ON CLOSE-UP ACTION TO DAMAGE VITAL PARTS OF ENEMIES CAUSING HIGH BUT UNPREDICTABLE DAMAGE", 99999, "coin", "attacks_tree");
                    break;
                case "smash_info":
                    AddBuyLayout(isReload, master, components, "info", "SMASH ATTACKS FOCUS ON POWER AND AREA OF EFFECT AT THE COST OF SPEED. THEY WORK GREAT ON GROUPS OF ENEMIES", 99999, "coin", "attacks_tree");
                    break;
                case "slash_info":
                    AddBuyLayout(isReload, master, components, "info", "SLASH ATTACKS ARE THE SAFEST FORM OF ATTACKS, THEIR ABILITIES ARE WELL BALANCED AND HAVE MEDIUM STATS", 99999, "coin", "attacks_tree");
                    break;
                case "range_info":
                    AddBuyLayout(isReload, master, components, "info", "RANGE WEAPONS ARE WEAK BUT ALLOW YOU TO HIT ENEMIES FROM AFAR", 99999, "coin", "attacks_tree");
                    break;

                case "buy_bolt":
                    AddBuyLayout(isReload, master, components, "bolt", "SHOOT METAL BOLTS FROM A DISTANCE\\ \\#grey#DAMAGE: 4\\SPEED: LOW", 100, "coin", "attacks_tree");
                    break;
                case "buy_smash":
                    AddBuyLayout(isReload, master, components, "smash", "A SLOW BUT DEVASTATING HAMMER\\ \\#grey#DAMAGE: 5\\SPEED: LOW\\RANGE: MEDIUM", 50, "coin", "attacks_tree");
                    break;
                case "buy_stab":
                    AddBuyLayout(isReload, master, components, "stab", "THIS SHORT BLADE IS MADE FOR FAST AND STEALTHY ACTION\\ \\#grey#DAMAGE: 5\\SPEED: FAST\\RANGE: LOW", 50, "coin", "attacks_tree");
                    break;
                case "buy_skew":
                    AddBuyLayout(isReload, master, components, "skew", "LUNGE FORWARD, EXTENDING THIS FAR-REACHING BUT NARROW BLADE\\ \\#grey#DAMAGE: 3\\SPEED: MEDIUM\\RANGE: HIGH", 50, "coin", "attacks_tree");
                    break;
                case "buy_slash":
                    AddBuyLayout(isReload, master, components, "slash", "A BLADE FOR SIMPLE BUT BALANCED ATTACKS\\ \\#grey#DAMAGE: 4\\SPEED: MEDIUM\\RANGE: MEDIUM", 50, "coin", "attacks_tree");
                    break;
                case "buy_steel_fists":
                    AddBuyLayout(isReload, master, components, "steel fists", "THE DEFAULT ATTACK. EVEN THE FORCE OF YOUR METAL HANDS CAN BE EFFECTIVE\\#grey#DAMAGE: 2\\SPEED: MEDIUM\\RANGE: LOW", 50, "coin", "attacks_tree");
                    break;

                case "buy_scanner":
                    AddBuyLayout(isReload, master, components, "scanner", "THIS DEVICE SHOWS THE REMAINING HEALTH OF ENEMIES AROUND YOU", 50, "coin", "upgrades_tree");
                    break;
                case "buy_attraction_1":
                    AddBuyLayout(isReload, master, components, "attraction 1", "COLLECT COINS AND POWER UPS FROM A DISTANCE", 500, "coin", "upgrades_tree");
                    break;
                case "buy_attraction_2":
                    AddBuyLayout(isReload, master, components, "attraction 2", "INCREASED ATTRACTION STRENGTH AND RANGE", 2500, "coin", "upgrades_tree");
                    break;
                case "buy_power_1":
                    AddBuyLayout(isReload, master, components, "power 1", "INCREASES DAMAGE OUTPUT BY #green#5% #white#TOTAL", 120, "coin", "upgrades_tree");
                    break;
                case "buy_power_2":
                    AddBuyLayout(isReload, master, components, "power 2", "INCREASES DAMAGE OUTPUT BY #green#10% #white#TOTAL", 520, "coin", "upgrades_tree");
                    break;
                case "buy_power_3":
                    AddBuyLayout(isReload, master, components, "power 3", "INCREASES DAMAGE OUTPUT BY #green#15% #white#TOTAL", 1200, "coin", "upgrades_tree");
                    break;
                case "buy_power_4":
                    AddBuyLayout(isReload, master, components, "power 4", "INCREASES DAMAGE OUTPUT BY #green#20% #white#TOTAL", 5200, "coin", "upgrades_tree");
                    break;
                case "buy_power_5":
                    AddBuyLayout(isReload, master, components, "power 5", "INCREASES DAMAGE OUTPUT BY #green#30%#white# TOTAL", 12000, "coin", "upgrades_tree");
                    break;
                case "buy_resilience_1":
                    AddBuyLayout(isReload, master, components, "resilience 1", "INCREASES MAXIMUM HEALTH BY #green#5 #white#TOTAL", 120, "coin", "upgrades_tree");
                    break;
                case "buy_resilience_2":
                    AddBuyLayout(isReload, master, components, "resilience 2", "INCREASES MAXIMUM HEALTH BY #green#10 #white#TOTAL", 520, "coin", "upgrades_tree");
                    break;
                case "buy_resilience_3":
                    AddBuyLayout(isReload, master, components, "resilience 3", "INCREASES MAXIMUM HEALTH BY #green#15 #white#TOTAL", 1200, "coin", "upgrades_tree");
                    break;
                case "buy_resilience_4":
                    AddBuyLayout(isReload, master, components, "resilience 4", "INCREASES MAXIMUM HEALTH BY #green#20 #white#TOTAL", 5200, "coin", "upgrades_tree");
                    break;
                case "buy_resilience_5":
                    AddBuyLayout(isReload, master, components, "resilience 5", "INCREASES MAXIMUM HEALTH BY #green#30 #white#TOTAL", 12000, "coin", "upgrades_tree");
                    break;
                case "buy_soul_collector_1":
                    AddBuyLayout(isReload, master, components, "soul collector 1", "#green#10% #white#CHANCE TO DROP EXTRA SOULS", 120, "luxiar_soul", "upgrades_tree");
                    break;
                case "buy_soul_collector_2":
                    AddBuyLayout(isReload, master, components, "soul collector 2", "#green#20% #white#CHANCE TO DROP EXTRA SOULS", 520, "coin", "upgrades_tree");
                    break;
                case "buy_soul_collector_3":
                    AddBuyLayout(isReload, master, components, "soul collector 3", "#green#30%#white# CHANCE TO DROP EXTRA SOULS", 1200, "coin", "upgrades_tree");
                    break;
                case "buy_soul_collector_4":
                    AddBuyLayout(isReload, master, components, "soul collector 4", "#green#40% #white#CHANCE TO DROP EXTRA SOULS", 5200, "coin", "upgrades_tree");
                    break;
                case "buy_soul_collector_5":
                    AddBuyLayout(isReload, master, components, "soul collector 5", "#green#50% #white#CHANCE TO DROP EXTRA SOULS", 12000, "coin", "upgrades_tree");
                    break;
                case "buy_spectral_battery_1":
                    AddBuyLayout(isReload, master, components, "spectral battery 1", "INCREASES ENERGY CAPACITY BY #green#5 #white#TOTAL", 120, "coin", "upgrades_tree");
                    break;
                case "buy_spectral_battery_2":
                    AddBuyLayout(isReload, master, components, "spectral battery 2", "INCREASES ENERGY CAPACITY BY #green#10 #white#TOTAL", 520, "coin", "upgrades_tree");
                    break;
                case "buy_spectral_battery_3":
                    AddBuyLayout(isReload, master, components, "spectral battery 3", "INCREASES ENERGY CAPACITY BY #green#15 #white#TOTAL", 1200, "coin", "upgrades_tree");
                    break;
                case "buy_spectral_battery_4":
                    AddBuyLayout(isReload, master, components, "spectral battery 4", "INCREASES ENERGY CAPACITY BY #green#20 #white#TOTAL", 5200, "coin", "upgrades_tree");
                    break;
                case "buy_spectral_battery_5":
                    AddBuyLayout(isReload, master, components, "spectral battery 5", "INCREASES ENERGY CAPACITY BY #green#30 #white#TOTAL", 12000, "coin", "upgrades_tree");
                    break;
                case "buy_better_repair_cells":
                    AddBuyLayout(isReload, master, components, "better repair cells", "INCREASES CHANCE TO FIND REPAIR CELLS. INCREASES AMOUNT HEALED BY REPAIR CELLS FROM #yellow#10 #white#HP #white#TO #green#18 #white#HP", 5200, "coin", "upgrades_tree");
                    break;
                case "buy_better_energy_cells":
                    AddBuyLayout(isReload, master, components, "better energy cells", "INCREASES CHANCE TO FIND ENERGY CELLS. INCREASES AMOUNT HEALED BY ENERGY CELLS FROM #yellow#15 #white#SE #white#TO #green#25 #white#SE", 12000, "coin", "upgrades_tree");
                    break;
            }
            return components;
        }
        static private List<string> CheckWeaponUnlocked(List<string> list, string weaponName, MasterManager master)
        {
            if (master.storedDataManager.CheckSkillUnlock(weaponName))
                list.Add(weaponName);
            return list;

        }
        static private GuiContent AddWeaponSlots(bool isReload, MasterManager master, GuiContent components)
        {
            int x = 0;
            int y = 0;
            List<string> availableWeapons = new List<string>();
            availableWeapons = CheckWeaponUnlocked(availableWeapons, "slash", master);
            availableWeapons = CheckWeaponUnlocked(availableWeapons, "smash", master);
            availableWeapons = CheckWeaponUnlocked(availableWeapons, "skew", master);
            availableWeapons = CheckWeaponUnlocked(availableWeapons, "stab", master);
            availableWeapons = CheckWeaponUnlocked(availableWeapons, "steel_fists", master);
            availableWeapons = CheckWeaponUnlocked(availableWeapons, "bolt", master);

            components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(213, y+1), Images.ImageDict["neutral_category_separator"], new Vector2(213, 32)));
            components.Screens[0].BackgroundComponents.Add(new TextBox("#white#NEUTRAL", new Vector2(213+10, y+10), 213, "none"));
            y += 1;
            foreach(string weapon in availableWeapons)
            {
                components.Screens[0].Buttons.Add(new ButtonImage(new ImagePanel(new Vector2(213 + 3 + x * 35, 0 + 3 + y * 35), Images.ImageDict["weapon_slot"]), new ImagePanel(new Vector2(213 + 3 + x * 35, 0 + 3 + y * 35), Images.ImageDict["weapon_slot_hovered"]), new Vector2(213 + 3 + x * 35, 0 + 3 + y * 35), new Vector2(32, 32), new ButtonSignalEvent("change_weapon", weapon)));
                components.Screens[0].BasicComponents.Add(new ImagePanel(new Vector2(213 + 7 + x * 35, 0 + 7 + y * 35), Images.ImageDict[weapon]));
                x += 1;
                if (x > 6)
                {
                    y += 1;
                    x = 0;
                }
            }
            availableWeapons.Clear();
            availableWeapons = CheckWeaponUnlocked(availableWeapons, "gold_fists", master);
            components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(213, (y+1)*35), Images.ImageDict["light_category_separator"], new Vector2(213,32)));
            components.Screens[0].BackgroundComponents.Add(new TextBox("#yellow#LIGHT", new Vector2(213 + 10, y*35 + 43), 213, "none"));
            y += 1;
            x = 0;
            if (availableWeapons.Count != 0)
                y += 1;
            foreach (string weapon in availableWeapons)
            {
                components.Screens[0].Buttons.Add(new ButtonImage(new ImagePanel(new Vector2(213 + 3 + x * 35, 0 + 3 + y * 35), Images.ImageDict["weapon_slot"]), new ImagePanel(new Vector2(213 + 3 + x * 35, 0 + 3 + y * 35), Images.ImageDict["weapon_slot_hovered"]), new Vector2(213 + 3 + x * 35, 0 + 3 + y * 35), new Vector2(32, 32), new ButtonSignalEvent("change_weapon", weapon)));
                components.Screens[0].BasicComponents.Add(new ImagePanel(new Vector2(213 + 7 + x * 35, 0 + 7 + y * 35), Images.ImageDict[weapon]));
                x += 1;
                if (x > 6)
                {
                    y += 1;
                    x = 0;
                }
            }

            availableWeapons.Clear();
            availableWeapons = CheckWeaponUnlocked(availableWeapons, "torch", master);
            components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(213, (y + 1) * 35), Images.ImageDict["flame_category_separator"], new Vector2(213, 32)));
            components.Screens[0].BackgroundComponents.Add(new TextBox("#orange#FLAME", new Vector2(213 + 10, y * 35 + 43), 213, "none"));
            y += 1;
            x = 0;
            if (availableWeapons.Count != 0)
                y += 1;
            foreach (string weapon in availableWeapons)
            {
                components.Screens[0].Buttons.Add(new ButtonImage(new ImagePanel(new Vector2(213 + 3 + x * 35, 0 + 3 + y * 35), Images.ImageDict["weapon_slot"]), new ImagePanel(new Vector2(213 + 3 + x * 35, 0 + 3 + y * 35), Images.ImageDict["weapon_slot_hovered"]), new Vector2(213 + 3 + x * 35, 0 + 3 + y * 35), new Vector2(32, 32), new ButtonSignalEvent("change_weapon", weapon)));
                components.Screens[0].BasicComponents.Add(new ImagePanel(new Vector2(213 + 7 + x * 35, 0 + 7 + y * 35), Images.ImageDict[weapon]));
                x += 1;
                if (x > 6)
                {
                    y += 1;
                    x = 0;
                }
            }
            availableWeapons.Clear();

            components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(213, (y + 1) * 35), Images.ImageDict["growth_category_separator"], new Vector2(213, 32)));
            components.Screens[0].BackgroundComponents.Add(new TextBox("#green#GROWTH", new Vector2(213 + 10, y * 35 + 43), 213, "none"));
            y += 1;
            x = 0;
            if (availableWeapons.Count != 0)
                y += 1;
            components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(213, (y + 1) * 35), Images.ImageDict["frost_category_separator"], new Vector2(213, 32)));
            components.Screens[0].BackgroundComponents.Add(new TextBox("#blue#FROST", new Vector2(213 + 10, y * 35 + 43), 213, "none"));
            y += 1;
            x = 0;
            if (availableWeapons.Count != 0)
                y += 1;
            components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(213, (y + 1) * 35), Images.ImageDict["shadow_category_separator"], new Vector2(213, 32)));
            components.Screens[0].BackgroundComponents.Add(new TextBox("#purple#SHADOW", new Vector2(213 + 10, y * 35 + 43), 213, "none"));
            y += 1;
            x = 0;
            if (availableWeapons.Count != 0)
                y += 1;
            components.Screens[0].BackgroundComponents.Add(new ImagePanel(new Vector2(213, (y + 1) * 35), Images.ImageDict["torment_category_separator"], new Vector2(213, 32)));
            components.Screens[0].BackgroundComponents.Add(new TextBox("#red#TORMENT", new Vector2(213 + 10, y * 35 + 43), 213, "none"));
            y += 1;
            x = 0;
            if (availableWeapons.Count != 0)
                y += 1;
            return components;
        }
        static private string AddCutsceneTrigger(MasterManager master, int level, string cutsceneName)
        {

            if (master.storedDataManager.CurrentSaveFile.CurrentLevel == level && !master.storedDataManager.CurrentSaveFile.Cutscenes.Contains(cutsceneName))
            {
                master.storedDataManager.CurrentSaveFile.Cutscenes.Add(cutsceneName);
                return cutsceneName + "-1";
            }
            else
                return "exit";
        }
        static private GuiContent AddBuyLayout(bool isReload, MasterManager master, GuiContent components, string name, string desc, int cost, string currency, string backMenu)
        {
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

            components.MainScreen.BasicComponents.Add(new TextBox($": {playerCurrencyAvailable}", new Vector2(380, 4), 100, "none"));
            components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(360, 4), Images.ImageDict[currency], new Vector2(16, 16)));

            components.MainScreen.Buttons.Add(new Button("", "", new Vector2(8, 8), new Vector2(32, 16), new ButtonSignalEvent("change_menu", backMenu)));

            components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(7, 7), Images.ImageDict["arrow_icon"]));
            components.MainScreen.BasicComponents.Add(new TextBox(name, new Vector2(60, 12), 300, "black", true));
            components.MainScreen.BasicComponents.Add(new TextBox(desc, new Vector2(20, 48), 200, "black", true));
            components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(0, 0), Images.ImageDict["stars"], new Vector2(426, 233)));
            components.MainScreen.BackgroundComponents.Add(new ImagePanel(new Vector2(287, 50), Images.ImageDict[name.Replace(' ','_')], new Vector2(120, 120)));



            //check if item is already bought
            if (!master.storedDataManager.CurrentSaveFile.Purchases.Contains(name.Replace(' ', '_')))
            {
                Vector2 offset = new Vector2(-5, 0);
                if (cost < 100 && cost > 9)
                    offset = new Vector2(0, 0);
                else if (cost < 1000)
                    offset = new Vector2(5, 0);
                else if (cost < 10000)
                    offset = new Vector2(10, 0);
                else if (cost < 100000)
                    offset = new Vector2(15, 0);
                //maybe possibility for 2 currencies to purchase later
                if (isReload)
                {
                    components.MainScreen.Buttons.Add(new Button($"CANT AFFORD", $"#red#CANT AFFORD", new Vector2(60, 185), new Vector2(140, 32), new ButtonSignalEvent(), "button_red", "button_red_hovered", "button_red_clicked"));
                }
                else
                {
                    if (name != "info")
                    {
                        components.MainScreen.Buttons.Add(new Button($"#white#BUY: {cost}", $"#yellow#BUY: {cost}", new Vector2(60, 185), new Vector2(140, 32), new ButtonSignalEvent("purchase", name.Replace(' ', '_') + $"/{cost}/{currency}"), "button_yellow", "button_yellow_hovered", "button_yellow_clicked", true, new Vector2(-10, 0)));
                        components.MainScreen.BasicComponents.Add(new ImagePanel(new Vector2(152, 192) + offset, Images.ImageDict[currency], new Vector2(16, 16)));
                    }
                }
            }
            else
            {
                components.MainScreen.Buttons.Add(new Button($"#grey#BOUGHT", $"#grey#BOUGHT", new Vector2(60, 185), new Vector2(140, 32), new ButtonSignalEvent()));
            }
            
            return components;
        }

        static private GuiContent AddLevel(GuiContent components, int levelNumber, Vector2 position, MasterManager master, int secretPassage = 0)
        {
            ButtonImage levelButton = new ButtonImage(new ImagePanel(position, Images.ImageDict["lx_level_current"], new Vector2(40, 30)), new ImagePanel(position, Images.ImageDict["lx_level_selected"], new Vector2(40, 30)), position, new Vector2(40, 30), new ButtonSignalEvent("play_screen", $"{levelNumber}"));
            int currentLevel = master.storedDataManager.CurrentSaveFile.CurrentLevel;
            if (currentLevel > levelNumber)
                levelButton = new ButtonImage(new ImagePanel(position, Images.ImageDict["lx_level_done"], new Vector2(40, 30)), new ImagePanel(position, Images.ImageDict["lx_level_selected"], new Vector2(40, 30)), position, new Vector2(40, 30), new ButtonSignalEvent("play_screen", $"{levelNumber}"));
            else if (currentLevel < levelNumber)
                levelButton = new ButtonImage(new ImagePanel(position, Images.ImageDict["lx_level_locked"], new Vector2(40, 30)), new ImagePanel(position, Images.ImageDict["lx_level_locked"], new Vector2(40, 30)), position, new Vector2(40, 30), new ButtonSignalEvent());
            components.Screens[0].Buttons.Add(levelButton);
            return components;
        }
        static private GuiContent AddFade(GuiContent components, bool isReload, double time = 0.3, string image = "blackout")
        {
            ImagePanel totalFadein = new ImagePanel(new Vector2(0, 0), Images.ImageDict[image], new Vector2(426, 233));
            ImagePanel totalFadeout = new ImagePanel(new Vector2(0, 0), Images.ImageDict[image], new Vector2(426, 233));
            components.MainScreen.FadingImages.Add(new FadingImage(totalFadein, "in", time, false, 0, true));
            if (!isReload)
                components.MainScreen.FadingImages.Add(new FadingImage(totalFadeout, "out", time, true));
            return components;
        }
        static private string Rainbowify(string text)
        {
            List<string> colours = new List<string> {"#red#", "#orange#", "#yellow#" , "#green#", "#blue#", "#purple#" };
            string rainbowText = "";
            for (int i = 0; i < text.Length; i++)
            {
                rainbowText += colours[i%6] + text[i];
            }
            rainbowText += "#white#";
            return rainbowText;
        }
    }
}
