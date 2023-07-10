using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Entities
{
    class MonsterStats
    {
        private float OrigMaxHp;
        public float MaxHp;
        public float Hp;
        public float MaxTorpor;
        public float Torpor;
        public bool HasTorpor;
        public int AttackRange;
        private float OrigDamage;
        public float Damage;
        public float ContactDamage;
        public double StunLength;
        public int Knockback;
        public float KnockbackResistance;
        public int SoulReward;
        public int CoinReward;
        public float NoneRes;
        public float LightRes;
        public float GrowthRes;
        public float FrostRes;
        public float ShadowRes;
        public float FlameRes;
        public float TormentRes;
        public float EnvironmentRes;
        public double ContactDmgHitTime;
        public MonsterStats(float hp, float torpor, int attackRange, float damage, double stunLength = 0, float contactDamage = 0, double contactDmgHitTime = 0.8, int knockback = 5, float knockbackRes = 0, int soulReward = 0, int coinReward = 0, float noneRes=0, float lightRes=0, float growthRes=0, float frostRes=0, float shadowRes=0, float flameRes =0 , float tormentRes = 0, float environmentRes = 0)
        {
            OrigMaxHp = hp;
            MaxHp = hp;
            Hp = hp;
            StunLength = stunLength;
            HasTorpor = false; // torpor like a health bar for knockback for bigger enemies, once it recieves enough normal knockback it gets stunned
            if (torpor > 0)
                HasTorpor = true;
            MaxTorpor = torpor;
            Torpor = torpor;
            AttackRange = attackRange;
            OrigDamage = damage;
            Damage = damage;
            ContactDamage = contactDamage;
            ContactDmgHitTime = contactDmgHitTime;
            Knockback = knockback;
            KnockbackResistance = knockbackRes;
            SoulReward = soulReward;
            CoinReward = coinReward;

            NoneRes = noneRes;
            LightRes = lightRes;
            GrowthRes = growthRes;
            FrostRes = frostRes;
            ShadowRes = shadowRes;
            FlameRes = flameRes;
            TormentRes = tormentRes;
            EnvironmentRes = environmentRes;
        }
    }
}
