using System;
using System.Collections.Generic;
using System.Text;
using Spextria.Graphics;
using Spextria.Graphics.GUI;
using Spextria.Graphics.GUI.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Spextria.Statics
{
    static class Images
    {
        public static Dictionary<string, MyTexture> ImageDict = new Dictionary<string,  MyTexture>();
        public static Dictionary<char, TextLetter> FontDict = new Dictionary<char, TextLetter>();

        public static void LoadTextureDict(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            string imageData =
//name , frames, speed, ani types
@"
none,1,0,1
spextria_logo,1,0,1

//Characters/player1,4,0.125,12
Characters/player3,4,0.125,6
Characters/player2,4,0.125,4


Sprites/Attacks/charge_ring,1,0,1
Sprites/Attacks/slash_attack,12,0.06,2
Sprites/Attacks/skew_attack,12,0.06,2
Sprites/Attacks/smash_attack,13,0.06,2
Sprites/Attacks/stab_attack,12,0.06,2
Sprites/Attacks/bolt_projectile,1,0,1
Sprites/Attacks/bolt_attack,8,1,2
Sprites/Attacks/torch_attack,8,1,2
Sprites/Attacks/yellow_fire_explosion,8,0.0625,1
Sprites/Attacks/explosion,8,0.0625,1
Sprites/Attacks/steel_fists_attack,12,0.06,2
Sprites/Attacks/gold_fists_attack,23,0.06,2


Sprites/Monsters/beast_lacerta,4,0.125,14
Sprites/Monsters/bloodscale,4,0.125,14

Sprites/Monsters/sand_worm_body,1,0,1
Sprites/Monsters/sand_worm_head,1,0,1
Sprites/Monsters/sand_worm_tail,1,0,1
Sprites/Monsters/tumble_weed,4,0.2,12
Sprites/Monsters/crawl_lacerta,4,0.125,12
Sprites/Monsters/alpha_lacerta,4,0.125,12
Sprites/Monsters/lacerta,4,0.125,12
Sprites/Monsters/arenube,4,0.125,8
Sprites/Monsters/enemy1,4,0.125,12
Sprites/Monsters/amog,4,0.125,8

Sprites/Monsters/gold_tree,1,0,1
Sprites/Monsters/gold_shrub,1,0,1

Sprites/Misc/tornado,4,0.125,1

Story/inter_level_dialog_approving,4,0.25,1
Story/inter_level_dialog,4,0.5,1
Story/stars,1,0,1
Story/spextria_planet,1,0,1
Story/spextria_planet_coloured,1,0,1
Story/house_scene,1,0,1
Story/house_scene_coloured,1,0,1
Story/daemalicus_scene,1,0,1
Story/planet_split_scene,1,0,1
Story/planet_split_scene_before,1,0,1
Story/inanimate_scene,1,0,1
Story/escape_scene,1,0,1
Story/escape_scene_later,1,0,1
Story/luxiar_escape_scene,1,0,1
Story/lab_scene,1,0,1
Story/lab_scene_fade,1,0,1
Story/lab_scene_fade2,1,0,1
Story/lab_scene_fade3,1,0,1
Story/lab_scene_after,1,0,1
Story/lab_scene_final,1,0,1
Story/lab_scene_final2,1,0,1
Story/lab_scene_final3,1,0,1

Menus/torment_category_separator,1,0,1
Menus/shadow_category_separator,1,0,1
Menus/flame_category_separator,1,0,1
Menus/frost_category_separator,1,0,1
Menus/growth_category_separator,1,0,1
Menus/light_category_separator,1,0,1
Menus/neutral_category_separator,1,0,1
Menus/weapon_slot,1,0,1
Menus/weapon_slot_hovered,1,0,1
Menus/half_stars,1,0,1
Menus/weapon_cooldown_overlay,1,0,1
Menus/weapon_button,1,0,1
Menus/weapon_button_hovered,1,0,1
Menus/enemy_bar_underlay,1,0,1
Menus/bar_overlay,1,0,1
Menus/bar_underlay,1,0,1
Menus/enemy_bar_segment,5,0.2,2
Menus/green_player_bar_segment,5,0.2,2
Menus/se_player_bar_segment,5,0.2,2
Menus/planet_select,1,0,1
Menus/selected_luxiar,4,0.1,1
Menus/selected_gramen,4,0.1,1
Menus/selected_freone,4,0.1,1
Menus/selected_umbrac,4,0.1,1
Menus/selected_inferni,4,0.1,1
Menus/selected_ossium,4,0.1,1
Menus/luxiar,1,0,1
Menus/luxiar_landscape,1,0,1
Menus/gramen,1,0,1
Menus/freone,1,0,1
Menus/umbrac,1,0,1
Menus/inferni,1,0,1
Menus/ossium,1,0,1
Menus/locked_luxiar,1,0,1
Menus/locked_gramen,1,0,1
Menus/locked_freone,1,0,1
Menus/locked_umbrac,1,0,1
Menus/locked_inferni,1,0,1
Menus/locked_ossium,1,0,1
Menus/lx_level_locked,1,0,1
Menus/lx_level_done,1,0,1
Menus/lx_level_current,4,0.125,1
Menus/lx_level_selected,4,0.125,1
Menus/level_failed,1,0,1
Menus/level_completed,1,0,1

Collectables/energy_cell,7,0.15,1
Collectables/repair_cell,8,0.15,1
Collectables/coin,14,0.125,1
Collectables/big_coin,8,0.155,1
Collectables/luxiar_soul,8,0.125,1

Particles/player_fragments1,7,0.25,1
Particles/player_fragments2,7,0.25,1
Particles/player_fragments3,7,0.25,1
Particles/sparkle1,4,0.125,1
Particles/sparkle2,4,0.1,1
Particles/sparkle3,4,0.05,1
Particles/white_flecks,4,0.125,1
Particles/white_dust,4,0.125,1
Particles/yellow_dust,4,0.125,1
Particles/sparks,4,0.125,1
Particles/luxiar_soul_particle,4,0.2,1
Particles/gramen_soul_particle,4,0.2,1
Particles/freone_soul_particle,4,0.2,1
Particles/inferni_soul_particle,4,0.2,1
Particles/umbrac_soul_particle,4,0.2,1
Particles/ossium_soul_particle,4,0.2,1

GUI/Panels/text_box,1,0,1
GUI/Panels/metal_box,1,0,1
GUI/Panels/no_box,1,0,1
GUI/Panels/slanted_box,1,0,1
GUI/Panels/tab_button,1,0,1
GUI/Panels/tab_button_hovered,1,0,1
GUI/Panels/tab_button_clicked,1,0,1
GUI/Panels/button,1,0,1
GUI/Panels/button_hovered,1,0,1
GUI/Panels/button_clicked,1,0,1
GUI/Panels/button_grey,1,0,1
GUI/Panels/button_red,1,0,1
GUI/Panels/button_red_hovered,1,0,1
GUI/Panels/button_red_clicked,1,0,1
GUI/Panels/button_yellow,1,0,1
GUI/Panels/button_yellow_hovered,1,0,1
GUI/Panels/button_yellow_clicked,1,0,1
GUI/Panels/button_green,1,0,1
GUI/Panels/button_green_hovered,1,0,1
GUI/Panels/button_green_clicked,1,0,1
GUI/Panels/blackout,1,0,1
GUI/Panels/black,1,0,1
GUI/Panels/speech_box_right,1,0,1
GUI/Panels/speech_box_left,1,0,1

GUI/Icons/arrow_icon,1,0,1
GUI/Icons/plus,1,0,1
GUI/Icons/minus,1,0,1
GUI/Icons/lock,1,0,1
GUI/Icons/solum,1,0,1
GUI/Icons/robur,1,0,1
GUI/Icons/shelby,1,0,1
GUI/Icons/solum_animated,4,0.125,1
GUI/Icons/robur_animated,4,0.125,1
GUI/Icons/shelby_animated,4,0.125,1

SkillTrees/info_node,1,0,1
SkillTrees/neutral_node,1,0,1
SkillTrees/next_neutral_node,1,0,1
SkillTrees/locked_neutral_node,1,0,1
SkillTrees/upgrades_tree,1,0,1
SkillTrees/attacks_tree,1,0,1

SkillTrees/unlockables/torch,1,0,1
SkillTrees/unlockables/bolt,1,0,1
SkillTrees/unlockables/info,1,0,1
SkillTrees/unlockables/stab,1,0,1
SkillTrees/unlockables/skew,1,0,1
SkillTrees/unlockables/smash,1,0,1
SkillTrees/unlockables/slash,1,0,1
SkillTrees/unlockables/gold_fists,1,0,1
SkillTrees/unlockables/steel_fists,1,0,1
SkillTrees/unlockables/scanner,1,0,1
SkillTrees/unlockables/attraction_1,1,0,1
SkillTrees/unlockables/attraction_2,1,0,1
SkillTrees/unlockables/power_1,1,0,1
SkillTrees/unlockables/power_2,1,0,1
SkillTrees/unlockables/power_3,1,0,1
SkillTrees/unlockables/power_4,1,0,1
SkillTrees/unlockables/power_5,1,0,1
SkillTrees/unlockables/resilience_1,1,0,1
SkillTrees/unlockables/resilience_2,1,0,1
SkillTrees/unlockables/resilience_3,1,0,1
SkillTrees/unlockables/resilience_4,1,0,1
SkillTrees/unlockables/resilience_5,1,0,1
SkillTrees/unlockables/soul_collector_1,1,0,1
SkillTrees/unlockables/soul_collector_2,1,0,1
SkillTrees/unlockables/soul_collector_3,1,0,1
SkillTrees/unlockables/soul_collector_4,1,0,1
SkillTrees/unlockables/soul_collector_5,1,0,1
SkillTrees/unlockables/spectral_battery_1,1,0,1
SkillTrees/unlockables/spectral_battery_2,1,0,1
SkillTrees/unlockables/spectral_battery_3,1,0,1
SkillTrees/unlockables/spectral_battery_4,1,0,1
SkillTrees/unlockables/spectral_battery_5,1,0,1
SkillTrees/unlockables/better_repair_cells,1,0,1
SkillTrees/unlockables/better_energy_cells,1,0,1

Sprites/LevelObjects/log_gate,1,0,1
Sprites/LevelObjects/campfire,5,0.25,1
Sprites/LevelObjects/neutral_checkpoint1_inactive,1,0,1
Sprites/LevelObjects/neutral_checkpoint1_active,4,0.18,2
Sprites/LevelObjects/luxiar_checkpoint1_inactive,1,0,1
Sprites/LevelObjects/luxiar_checkpoint1_active,4,0.18,2
Sprites/LevelObjects/goal_arrow1_inactive,1,0,1
Sprites/LevelObjects/goal_arrow1_active,10,0.1,1

Sprites/Npcs/man_by_campfire,2,0.2,2
";
            string[] imageDataList = imageData.Split("\r\n");
            foreach (string image in imageDataList)
            {
                if (image == "")
                    continue;
                string[] splitImage = image.Split(',');
                splitImage[^1].Replace("\n", "");
                string imgName = splitImage[0];
                int aniFrames = int.Parse(splitImage[1]);
                double aniSpeed = double.Parse(splitImage[2]);
                int aniTypes = int.Parse(splitImage[3]);
                ImageDict.Add(imgName.Split("/")[^1], new MyTexture(content.Load<Texture2D>("Images/" + imgName), aniFrames, aniSpeed, aniTypes, imgName.Split("/")[^1]));
            }
        }

        public static void LoadFont(Microsoft.Xna.Framework.Content.ContentManager content, GraphicsDevice graphicsDevice)
        {
            int totalWidth = 0;
            Texture2D fontImage = content.Load<Texture2D>("Images/font");
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ.,:'!?-+1234567890% ";
            for (int i = 0; i < letters.Length; i++)
            {
                char letter = letters[i];
                int currentWidth = 0;
                int spacing = 2;
                if (i < 26)
                {
                    currentWidth = 7;
                }
                else if (i < 31)
                {
                    currentWidth = 2;
                }
                else if (i == 31)
                {
                    currentWidth = 8;
                }
                else if (i < 45)
                {
                    currentWidth = 7;
                }
                else if (i == 45)
                {
                    currentWidth = 5;
                }
                Rectangle sourceRectangle = new Rectangle(totalWidth, 0, currentWidth, 16);
                totalWidth += currentWidth;

                Texture2D letterImage = new Texture2D(graphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
                Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                fontImage.GetData(0, sourceRectangle, data, 0, data.Length);
                letterImage.SetData(data);

                TextLetter textLetter = new TextLetter(letterImage, spacing, letter);
                FontDict.Add(letter, textLetter);
            }
        }

        public static void UpdateTextures(double timePassed)
        {
            foreach (MyTexture texture in ImageDict.Values)
            {
                texture.Update(timePassed);
            }
        }

        public static MyTexture UniqueImage(MyTexture image)
        {
            MyTexture newImage = new MyTexture(image.Texture, image.Frames, image.FrameDuration, image.Types);
            newImage.RandomiseFrame();
            return newImage;
        }
    }
}
