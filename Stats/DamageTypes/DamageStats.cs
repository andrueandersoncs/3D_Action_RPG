using Combat;
using Stats.ResistanceTypes;
using Stats.Vitals;
using UnityEngine;

namespace Stats.DamageTypes
{
    public class DamageStats : MonoBehaviour, IStats<DamageStats>
    {
        public float FireDamage;
        public float ColdDamage;
        public float LightningDamage;
        public float PoisonDamage;
        public float ArcaneDamage;
        public float PhysicalDamage;
        
        public float AggregateDamage() =>
            FireDamage + ColdDamage + LightningDamage + PoisonDamage + ArcaneDamage + PhysicalDamage;

        public void Add(DamageStats other)
        {
            FireDamage += other.FireDamage;
            ColdDamage += other.ColdDamage;
            LightningDamage += other.LightningDamage;
            PoisonDamage += other.PoisonDamage;
            ArcaneDamage += other.ArcaneDamage;
            PhysicalDamage += other.PhysicalDamage;
        }
        
        public void Subtract(DamageStats other)
        {
            FireDamage -= other.FireDamage;
            ColdDamage -= other.ColdDamage;
            LightningDamage -= other.LightningDamage;
            PoisonDamage -= other.PoisonDamage;
            ArcaneDamage -= other.ArcaneDamage;
            PhysicalDamage -= other.PhysicalDamage;
        }

        public void DealDamage(GameObject target)
        {
            if (!target.TryGetComponent<VitalStats>(out var vitalStats)) return;
            CombatModule.ReceiveDamage(new CombatModule.ReceiveDamageInput
            {
                DamageStats = this,
                VitalStats = vitalStats,
                ResistanceStats = vitalStats.GetComponent<ResistanceStats>()
            });
        }
    }
}