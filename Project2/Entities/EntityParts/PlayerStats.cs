using Spextria.Master;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Entities
{
    class PlayerStats
    {
        private float OrigMaxHp;
        public float MaxHp;
        public float Hp;
        private float OrigMaxSe;
        public float MaxSe;
        public float Se;
        private float OrigDamage;
        public float Damage;
        public int Knockback;
        private int OrigKnockback;
        public int KnockbackResistance;
        public float ExtraSoulChance;
        public PlayerStats(float hp = 20, float se = 20, float damage = 2, int knockback = 5, int knockbackRes = 0)
        {
            ExtraSoulChance = 0;
            OrigKnockback = knockback;
            OrigMaxHp = hp;
            MaxHp = hp;
            Hp = hp;
            OrigMaxSe = se;
            MaxSe = se;
            Se = se;
            OrigDamage = damage;
            Damage = damage;
            Knockback = knockback;
            KnockbackResistance = knockbackRes;

        }

        public void Reset()
        {
            ExtraSoulChance = 0;
            MaxHp = OrigMaxHp;
            MaxSe = OrigMaxSe;
            Hp = MaxHp;
            Se = MaxSe;
            Damage = OrigDamage;
            Knockback = OrigKnockback;
            KnockbackResistance = 0;
        }
        public void ApplyBuffs(MasterManager master)
        {
            if (master.storedDataManager.CheckSkillUnlock("power_5"))
                Damage *= 1.30f;
            else if (master.storedDataManager.CheckSkillUnlock("power_4"))
                Damage *= 1.20f;
            else if (master.storedDataManager.CheckSkillUnlock("power_3"))
                Damage *= 1.15f;
            else if (master.storedDataManager.CheckSkillUnlock("power_2"))
                Damage *= 1.10f;
            else if (master.storedDataManager.CheckSkillUnlock("power_1"))
                Damage *= 1.05f;

            if (master.storedDataManager.CheckSkillUnlock("resilience_5"))
                MaxHp += 30;
            else if (master.storedDataManager.CheckSkillUnlock("resilience_4"))
                MaxHp += 20;
            else if (master.storedDataManager.CheckSkillUnlock("resilience_3"))
                MaxHp += 15;
            else if (master.storedDataManager.CheckSkillUnlock("resilience_2"))
                MaxHp += 10;
            else if (master.storedDataManager.CheckSkillUnlock("resilience_1"))
                MaxHp += 5;

            if (master.storedDataManager.CheckSkillUnlock("spectral_battery_5"))
                MaxSe += 30;
            else if (master.storedDataManager.CheckSkillUnlock("spectral_battery_4"))
                MaxSe += 20;
            else if (master.storedDataManager.CheckSkillUnlock("spectral_battery_3"))
                MaxSe += 15;
            else if (master.storedDataManager.CheckSkillUnlock("spectral_battery_2"))
                MaxSe += 10;
            else if (master.storedDataManager.CheckSkillUnlock("spectral_battery_1"))
                MaxSe += 5;

            if (master.storedDataManager.CheckSkillUnlock("soul_collector_5"))
                ExtraSoulChance += 0.5f;
            else if (master.storedDataManager.CheckSkillUnlock("soul_collector_4"))
                ExtraSoulChance += 0.4f;
            else if (master.storedDataManager.CheckSkillUnlock("soul_collector_3"))
                ExtraSoulChance += 0.3f;
            else if (master.storedDataManager.CheckSkillUnlock("soul_collector_2"))
                ExtraSoulChance += 0.2f;
            else if (master.storedDataManager.CheckSkillUnlock("soul_collector_1"))
                ExtraSoulChance += 0.1f;

            Hp = MaxHp;
            Se = MaxSe;
        }
    }
}
