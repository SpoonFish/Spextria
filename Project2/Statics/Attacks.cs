
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using Spextria.Master;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.Entities;
using Spextria.Entities.AttackObjects;

namespace Spextria.Statics
{
    static class Attacks
    {
        public static MeleeAttack CreateMeleeAttack(string name)
        {

            //return new SolidMonsterSprite(Images.UniqueImage(Images.ImageDict["player2"]), pos, 1.4f, 3.8f, 2.4f, CreateHitbox(new Size(8, 24), pos, new Point(0, 32)));

            MeleeAttack attack = null;
            switch (name)
            {
                case "none":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { });
                        Vector2 offset = new Vector2(500, 500);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["slash_attack"]), "none", "none", "slash", 3, 5, 0.01d, 12, 0.4, 0, 0, hurtbox, offset);
                        break;
                    }
                case "tier1_slash":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(7, -10, 32, 24), new Rectangle(7, -1, 38, 13) });
                        Vector2 offset = new Vector2(2, -13);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["slash_attack"]), "slash", "slash", "slash", 1.5f, 2, 0.4d, 12, 0.41d, 0.77d, 0.2d, hurtbox, offset, new Vector2(48, 30), false, 0.8f);
                        break;
                    }
                case "slash":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(7, -10, 42, 26), new Rectangle(7, -1, 52, 16) });
                        Vector2 offset = new Vector2(2, -13);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["slash_attack"]), "slash", "none", "slash", 3, 3, 0.41d, 12, 0.5d, 0.7d, 0.2d, hurtbox, offset, new Vector2(64, 32), false, 0.8f);
                        break;
                    }
                case "torch":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(7, -12, 34, 26) });
                        Vector2 offset = new Vector2(1, -18);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["torch_attack"]), "torch", "flame", "none", 3, 1, 0.71d, 8, 0.9d, 1d, 0.1d, hurtbox, offset, new Vector2(42, 32), false, 0.9f);
                        break;
                    }
                case "gold_fists":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(7, -12, 22, 26), new Rectangle(7, -8, 24, 16) });
                        Vector2 offset = new Vector2(-2, -11);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["gold_fists_attack"]), "gold_fists", "none", "none", 1.65f, 2, 0.3d, 23, 0.22d, 0.38d, 0.01d, hurtbox, offset, new Vector2(32, 26), false, 4.1f);
                        break;
                    }
                case "steel_fists":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(7, -12, 22, 26), new Rectangle(7, -8, 24, 16) });
                        Vector2 offset = new Vector2(4, -11);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["steel_fists_attack"]), "steel_fists", "none", "none", 2.5f, 3, 0.41d, 12, 0.4d, 0.58d, 0.1d, hurtbox, offset, new Vector2(26, 26), false, 0.9f);
                        break;
                    }
                case "smash":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(7, -16, 30, 20), new Rectangle(7, -2, 47, 22) });
                        Vector2 offset = new Vector2(2, -30);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["smash_attack"]), "smash", "none", "smash", 5, 6, 0.81d, 13, 1d, 0.9d, 0.4d, hurtbox, offset, new Vector2(58, 48), true, 0.5f);
                        break;
                    }
                case "monster_skew":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(4, -2, 60, 18) });
                        Vector2 offset = new Vector2(2, -8);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["skew_attack"]), "skew", "none", "skew", 3, 3, 0.51d, 12, 0.5d, 0.9d, 0.3d, hurtbox, offset, new Vector2(62, 22), false, 1.2f);
                        break;
                    }
                case "skew":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(4, -2, 69, 18) });
                        Vector2 offset = new Vector2(2, -6);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["skew_attack"]), "skew", "none", "skew", 3, 3, 0.51d, 12, 0.5d, 0.9d, 0.3d, hurtbox, offset, new Vector2(74, 24), false, 1.2f);
                        break;
                    }
                case "bloodscale_skew":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(4, -2, 69, 18) });
                        Vector2 offset = new Vector2(2, -6);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["skew_attack"]), "skew", "none", "skew", 4, 6, 0.71d, 12, 0.8d, 1.9d, 0.5d, hurtbox, offset, new Vector2(74, 24), false, 2.2f);
                        break;
                    }
                case "beast_dash":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(0, -9, 42, 32) });
                        Vector2 offset = new Vector2(0, 0);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["none"]), "skew", "none", "none", 6, 4, 0.71d, 12, 0.6d, 1.3d, 0.85d, hurtbox, offset, new Vector2(1, 1), false, 1.6f);
                        break;
                    }
                case "dash":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(0, -9, 7, 12) });
                        Vector2 offset = new Vector2(0, 0);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["none"]), "none", "none", "none", 4, 4, 0.91d, 12, 0.6d, 0.3d, 0.2d, hurtbox, offset, new Vector2(1, 1), false, 1.8f);
                        break;
                    }
                case "stab":
                    {
                        AttackHurtbox hurtbox = new AttackHurtbox(new Rectangle[] { new Rectangle(7, -2, 30, 14) });
                        Vector2 offset = new Vector2(2, -6);
                        attack = new MeleeAttack(Images.UniqueImage(Images.ImageDict["stab_attack"]), "stab", "none", "stab", 5, 2, 0.31d, 12, 0.3d, 0.4d, 0.05d, hurtbox, offset, new Vector2(34, 20), false, 0.95f);
                        break;
                    }
            }
            return attack;
        }

        public static RangedAttack CreateRangeAttack(string name, bool player = true)
        {

            //return new SolidMonsterSprite(Images.UniqueImage(Images.ImageDict["player2"]), pos, 1.4f, 3.8f, 2.4f, CreateHitbox(new Size(8, 24), pos, new Point(0, 32)));

            RangedAttack attack = null;
            switch (name)
            {
                case "spit":
                    {
                        Vector2 offset = new Vector2(-2, -8);
                        attack = new RangedAttack(Images.UniqueImage(Images.ImageDict["none"]), "spit", "torment", "none", 1, 2, 1d, 10, 0.2d, 1.2d, 2.3d, player, 1.6f, true, 4.5f, 0, new Rectangle(0, 0, 6, 6), Images.ImageDict["button_red"], offset, new Vector2(24, 24), new Vector2(5, -4), false, 0.1f, 2, "sparkles", 9, 20, 3200, false);//, 3, Images.ImageDict["explosion"], new Rectangle(0,0,32,32));
                        break;
                    }
                case "bolt":
                    {
                        Vector2 offset = new Vector2(-2, -8);
                        attack = new RangedAttack(Images.UniqueImage(Images.ImageDict["bolt_attack"]), "bolt", "none", "none", 30, 2, 0.1d, 10, 0.5d, 0.3d, 0.9d, player, 1f, true, 6.5f, 4, new Rectangle(0, 0, 16, 8), Images.ImageDict["bolt_projectile"], offset, new Vector2(24, 24), new Vector2(5, 0), false, 0.4f, 1, "sparkles", 1, 1, 3200, false);//, 3, Images.ImageDict["explosion"], new Rectangle(0,0,32,32));
                        break;
                    }
                case "tier2_bolt":
                    {
                        Vector2 offset = new Vector2(-2, -8);
                        attack = new RangedAttack(Images.UniqueImage(Images.ImageDict["bolt_attack"]), "bolt", "none", "none", 2, 1, 0.3d, 10, 0.2d, 0.4d, 01.1d, player, 1f, true, 5.8f, 0, new Rectangle(0, 0, 32, 8), Images.ImageDict["bolt_projectile"], offset, new Vector2(24, 24), new Vector2(10, 0), false, 0.1f, 1, "sparkles", 2, 5, 3200, false);//, 3, Images.ImageDict["explosion"], new Rectangle(0,0,32,32));
                        break;
                    }
                case "tier1_bolt":
                    {
                        Vector2 offset = new Vector2(-2, -8);
                        attack = new RangedAttack(Images.UniqueImage(Images.ImageDict["bolt_attack"]), "bolt", "none", "none", 1, 1, 0.1d, 10, 0.5d, 0.3d, 0.9d, player, 1f, true, 4.5f, 0, new Rectangle(0, 0, 16, 8), Images.ImageDict["bolt_projectile"], offset, new Vector2(24, 24), new Vector2(10, 0), false, 0.4f, 1, "sparkles", 1, 5, 3200, false);//, 3, Images.ImageDict["explosion"], new Rectangle(0,0,32,32));
                        break;
                    }
                case "lizard_beast_fire":
                    {
                        Vector2 offset = new Vector2(-2, -8);
                        attack = new RangedAttack(Images.UniqueImage(Images.ImageDict["none"]), "bolt", "light", "none", 2, 0, 2, 2, 0.1d, 5.5, 1.6, player, 0.1f, true, 3, 1, new Rectangle(0, 0, 8, 8), Images.ImageDict["yellow_fire_explosion"], offset, new Vector2(1, 1), new Vector2(30,-4), true, 0.1f, 2, "", 80, 15, 80, true); 
                        break;
                    }
            }
            return attack;
        }
        public static string MeleeOrRanged(string name)
        {
            List<string> rangeList = new List<string>() { "bolt" };
            if (rangeList.Contains(name))
                return "range";
            return "melee";
        }
    }
}