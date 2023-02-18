using Stats.DamageTypes;
using Stats.ResistanceTypes;
using Stats.Vitals;

namespace Combat
{
    public static class CombatModule
    {
        public struct ReceiveDamageInput
        {
            public DamageStats DamageStats;
            public ResistanceStats ResistanceStats;
            public VitalStats VitalStats;
        }
        
        public static void ReceiveDamage(ReceiveDamageInput input)
        {
            var damage = input.ResistanceStats == null
                ? AggregateDamage(input.DamageStats)
                : CalculateDamage(input.DamageStats, input.ResistanceStats);
            
            input.VitalStats.Health -= damage;
        }
        
        public static float AggregateDamage(DamageStats damageStats)
        {
            return
                damageStats.FireDamage +
                damageStats.ColdDamage +
                damageStats.LightningDamage +
                damageStats.PoisonDamage +
                damageStats.ArcaneDamage +
                damageStats.PhysicalDamage;
        }
        
        public static float CalculateDamage(DamageStats damageStats, ResistanceStats resistanceStats)
        {
            return
                damageStats.FireDamage * (1 - resistanceStats.FireResistance / 100f) +
                damageStats.ColdDamage * (1 - resistanceStats.ColdResistance / 100f) +
                damageStats.LightningDamage * (1 - resistanceStats.LightningResistance / 100f) +
                damageStats.PoisonDamage * (1 - resistanceStats.PoisonResistance / 100f) +
                damageStats.ArcaneDamage * (1 - resistanceStats.ArcaneResistance / 100f) +
                damageStats.PhysicalDamage * (1 - resistanceStats.PhysicalResistance / 100f);
        }
    }
}