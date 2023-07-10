
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
using Spextria.Maps.LevelObjectTypes;

namespace Spextria.Statics
{
    static class Monsters
    {
        public static MonsterEntity CreateMonster(string name, Vector2 pos)
        {

            //return new SolidMonsterSprite(Images.UniqueImage(Images.ImageDict["player2"]), pos, 1.4f, 3.8f, 2.4f, CreateHitbox(new Size(8, 24), pos, new Point(0, 32)));
            List<Attack> attacks = new List<Attack>(); 
            MonsterStats stats = new MonsterStats(0, 0, 0, 0); 
            switch (name)
            {
                case "tutorial_lacerta":
                    attacks = new List<Attack>(){
                        Attacks.CreateMeleeAttack("none"),
                    };

                    stats = new MonsterStats(
                        hp: 15,
                        torpor: 0,
                        attackRange: 0,
                        damage: 0,
                        contactDamage: 1,
                        knockback: 2,
                        soulReward: 1,
                        coinReward: 1
                    );
                    return new SolidMonsterEntity(Images.UniqueImage(Images.ImageDict["lacerta"]), pos, stats, 1.2f, 2f, 1.8f, attacks, null, DataTypes.Intelligence.Sidetoside);

                case "tier1_slasher_lacerta":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("tier1_slash")
                    };

                    stats = new MonsterStats(
                        hp: 18,
                        torpor: 0,
                        attackRange: 30,
                        damage: 0,
                        contactDamage: 0,
                        knockback: 2,
                        soulReward: 1,
                        coinReward: 2
                    );
                    return new SolidMonsterEntity(Images.UniqueImage(Images.ImageDict["lacerta"]), pos, stats, 1.5f, 2f, 1.8f, attacks, null, DataTypes.Intelligence.Sidetoside);

                case "tier1_sand_worm":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("none")
                    };

                    stats = new MonsterStats(
                        hp: 50,
                        torpor: 0,
                        attackRange: 110,
                        damage: 0,
                        contactDamage: 2,
                        knockback: 2,
                        soulReward: 3,
                        coinReward: 8,
                        environmentRes: 1
                    );
                    return new WormEntity(Images.UniqueImage(Images.ImageDict["sand_worm_head"]), pos, stats, 1.2f, 2f, 1.7f, attacks, 7, Images.ImageDict["sand_worm_body"], Images.ImageDict["sand_worm_tail"], null, DataTypes.Intelligence.Worm1);

                case "tier2_sand_worm":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("none")
                    };

                    stats = new MonsterStats(
                        hp: 80,
                        torpor: 0,
                        attackRange: 210,
                        damage: 0,
                        contactDamage: 3,
                        knockback: 4,
                        soulReward: 4,
                        coinReward: 11,
                        environmentRes: 1
                    );
                    return new WormEntity(Images.UniqueImage(Images.ImageDict["sand_worm_head"]), pos, stats, 1.05f, 2f, 2.3f, attacks, 11, Images.ImageDict["sand_worm_body"], Images.ImageDict["sand_worm_tail"], null, DataTypes.Intelligence.Worm1);

                case "tier1_ranger_lacerta":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateRangeAttack("tier1_bolt", false)
                    };

                    stats = new MonsterStats(
                        hp: 18,
                        torpor: 0,
                        attackRange: 90,
                        damage: 0,
                        contactDamage: 0,
                        knockback: 2,
                        soulReward: 1,
                        coinReward: 2
                    );
                    return new SolidMonsterEntity(Images.UniqueImage(Images.ImageDict["lacerta"]), pos, stats, 1.2f, 2f, 1.8f, attacks, null, DataTypes.Intelligence.Sidetoside);

                case "tier2_lacerta":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("dash")
                    };

                    stats = new MonsterStats(
                        hp: 28,
                        torpor: 0,
                        attackRange: 15,
                        damage: 0,
                        contactDamage: 2.5f,
                        knockback: 3,
                        soulReward: 2,
                        coinReward: 3
                    );
                    return new SolidMonsterEntity(Images.UniqueImage(Images.ImageDict["crawl_lacerta"]), pos, stats, 1.8f, 2f, 2f, attacks, null, DataTypes.Intelligence.Scout, DataTypes.Hostility.Hostile);

                case "tier2_ranger_lacerta":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateRangeAttack("tier2_bolt", false)
                    };

                    stats = new MonsterStats(
                        hp: 25,
                        torpor: 0,
                        attackRange: 140,
                        damage: 0,
                        contactDamage: 0,
                        knockback: 2,
                        soulReward: 1,
                        coinReward: 4
                    );
                    return new SolidMonsterEntity(Images.UniqueImage(Images.ImageDict["alpha_lacerta"]), pos, stats, 1.7f, 2f, 2f, attacks, null, DataTypes.Intelligence.Sidetoside, DataTypes.Hostility.Hostile);

                case "gold_shrub":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("none"),
                    };

                    stats = new MonsterStats(
                        knockbackRes:1,
                        hp: 3,
                        torpor: 0,
                        attackRange: 0,
                        damage: 0,
                        contactDmgHitTime: 0.2,
                        contactDamage: 2,
                        knockback: 5,
                        soulReward: 0,
                        coinReward: 0,
                        noneRes: 1,
                        lightRes: 1,
                        growthRes: 1,
                        frostRes: 1,
                        shadowRes: 1

                    );
                    return new SolidMonsterEntity(Images.ImageDict["gold_shrub"], pos, stats, 0, 0, 0, attacks, null, DataTypes.Intelligence.Stationary);

                case "fast_arenube":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("none"),
                    };

                    stats = new MonsterStats(
                        knockbackRes: 1,
                        hp: 1000,
                        torpor: 1000,
                        attackRange: 0,
                        damage: 0,
                        contactDmgHitTime: 0,
                        contactDamage: 2,
                        knockback: 5,
                        soulReward: 0,
                        coinReward: 0,
                        noneRes: 1,
                        flameRes: 1,
                        lightRes: 1,
                        growthRes: 1,
                        frostRes: 1,
                        shadowRes: 1

                    );
                    return new Arenube(Images.UniqueImage(Images.ImageDict["arenube"]), pos, stats, 0, 0, 0, attacks, null, 2);

                case "arenube_right":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("none"),
                    };

                    stats = new MonsterStats(
                        knockbackRes: 1,
                        hp: 1000,
                        torpor: 1000,
                        attackRange: 0,
                        damage: 0,
                        contactDmgHitTime: 0,
                        contactDamage: 2,
                        knockback: 5,
                        soulReward: 0,
                        coinReward: 0,
                        noneRes: 1,
                        flameRes: 1,
                        lightRes: 1,
                        growthRes: 1,
                        frostRes: 1,
                        shadowRes: 1

                    );
                    return new Arenube(Images.UniqueImage(Images.ImageDict["arenube"]), pos, stats, 0, 0, 0, attacks, null, 1.5f,DataTypes.Directions.Right);

                case "arenube":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("none"),
                    };

                    stats = new MonsterStats(
                        knockbackRes: 1,
                        hp: 1000,
                        torpor: 1000,
                        attackRange: 0,
                        damage: 0,
                        contactDmgHitTime: 0,
                        contactDamage: 2,
                        knockback: 5,
                        soulReward: 0,
                        coinReward: 0,
                        noneRes: 1,
                        flameRes: 1,
                        lightRes: 1,
                        growthRes: 1,
                        frostRes: 1,
                        shadowRes: 1

                    );
                    return new Arenube(Images.UniqueImage(Images.ImageDict["arenube"]), pos, stats, 0,0,0, attacks);

                case "bloodscale":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("bloodscale_skew"),
                        Attacks.CreateMeleeAttack("skew"),
                        Attacks.CreateRangeAttack("spit", false),
                    };

                    stats = new MonsterStats(
                        knockbackRes: 0.25f,
                        hp: 50,
                        torpor: 14,
                        attackRange: 65,
                        damage: 0,
                        contactDamage: 2,
                        stunLength: 1.6,
                        knockback: 5,
                        soulReward: 5,
                        coinReward: 25,
                        lightRes: 0.3f

                    );
                    return new Bloodscale(Images.UniqueImage(Images.ImageDict["bloodscale"]), pos, stats, 1.6f, 2, 2.5f, attacks);

                case "gold_tree":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("none"),
                    };

                    stats = new MonsterStats(
                        knockbackRes: 1,
                        hp: 3,
                        torpor: 0,
                        attackRange: 0,
                        damage: 0,
                        contactDmgHitTime: 0.2,
                        contactDamage: 2,
                        knockback: 5,
                        soulReward: 0,
                        coinReward: 0,
                        noneRes: 1,
                        lightRes: 1,
                        growthRes: 1,
                        frostRes: 1,
                        shadowRes: 1

                    );
                    return new SolidMonsterEntity(Images.ImageDict["gold_tree"], pos, stats, 0, 0, 0, attacks, null, DataTypes.Intelligence.Stationary);

                case "tumble_weed":
                    attacks = new List<Attack>()
                    {
                        Attacks.CreateMeleeAttack("none"),
                    };

                    stats = new MonsterStats(
                        hp: 5,
                        torpor: 0,
                        attackRange: 0,
                        damage: 0,
                        contactDmgHitTime: 0.5,
                        contactDamage: 1.2f,
                        knockback: 3,
                        soulReward: 0,
                        coinReward: 1,
                        environmentRes: 1,
                        noneRes: 0.5f,
                        lightRes:0.5f,
                        growthRes:0.5f,
                        frostRes: 0.5f,
                        shadowRes: 0.5f

                    );
                    return new SolidMonsterEntity(Images.ImageDict["tumble_weed"], pos, stats, 1.5f, 2f, 0.4f, attacks, null, DataTypes.Intelligence.Leap);

                case "enemy2":
                    attacks = new List<Attack>(){
                        Attacks.CreateMeleeAttack("beast_dash"),
                        Attacks.CreateRangeAttack("lizard_beast_fire", false)
                    };

                    stats = new MonsterStats(
                        hp: 200,
                        torpor: 30,
                        stunLength: 4,
                        attackRange: 48,
                        damage: 0,
                        contactDamage: 4,
                        knockback: 2,
                        soulReward: 1,
                        coinReward: 200
                    );
                    return new SolidMonsterEntity(Images.UniqueImage(Images.ImageDict["beast_lacerta"]), pos, stats, 2.1f, 2.5f, 2.3f, attacks, null, DataTypes.Intelligence.Scout, DataTypes.Hostility.Hostile);

            }
            return null;
        }

        private static HitboxObject CreateHitbox(Size size, Vector2 vecPosition, Point posOffset = new Point())
        {
            Point position = new Point((int)vecPosition.X,(int)vecPosition.Y) + posOffset;

            Rectangle full = new Rectangle(position.X, position.Y, size.Width, size.Height);
            Rectangle body = new Rectangle(position.X + 1, position.Y + 1, size.Width - 2, size.Height - 1);
            Rectangle leftRight = new Rectangle(position.X, position.Y + 2, size.Width, size.Height - 4);
            Rectangle upDown = new Rectangle(position.X + 4, position.Y, size.Width - 8, size.Height);
            HitboxObject hitbox = new HitboxObject(body, upDown, leftRight, full, posOffset);
            return hitbox;
        }
    }
}