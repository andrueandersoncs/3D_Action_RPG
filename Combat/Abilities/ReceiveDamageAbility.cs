using System.Collections;
using Abilities;
using Stats.DamageTypes;
using Stats.ResistanceTypes;
using Stats.Vitals;
using UnityEngine;

namespace Combat.Abilities
{
    public class ReceiveDamageAbility : Ability
    {
        public VitalStats vitalStats;
        public ResistanceStats resistanceStats;
        public Animator animator;
        public DieAbility dieAbility;

        public DamageStats damageStatsToReceive;
        
        private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");

        protected override IEnumerator Execute()
        {
            if (vitalStats.Health <= 0)
            {
                Debug.Log("Did not successfully execute receive damage ability");
                successfullyExecuted = false;
                yield break;
            }
            
            var damage = resistanceStats == null ? AggregateDamage() : CalculateDamage();
            
            Debug.Log("Damage dealt:" + damage);
            
            vitalStats.Health -= damage;
            
            Debug.Log("Health:" + vitalStats.Health);

            if (vitalStats.Health <= 0)
            {
                yield return dieAbility.Play();
            }
            else
            {
                animator.SetTrigger(TakeDamage);
            }

            successfullyExecuted = true;
        }
        
        private float AggregateDamage()
        {
            return
                damageStatsToReceive.FireDamage +
                damageStatsToReceive.ColdDamage +
                damageStatsToReceive.LightningDamage +
                damageStatsToReceive.PoisonDamage +
                damageStatsToReceive.ArcaneDamage +
                damageStatsToReceive.PhysicalDamage;
        }
        
        private float CalculateDamage()
        {
            return
                damageStatsToReceive.FireDamage * (1 - resistanceStats.FireResistance / 100f) +
                damageStatsToReceive.ColdDamage * (1 - resistanceStats.ColdResistance / 100f) +
                damageStatsToReceive.LightningDamage * (1 - resistanceStats.LightningResistance / 100f) +
                damageStatsToReceive.PoisonDamage * (1 - resistanceStats.PoisonResistance / 100f) +
                damageStatsToReceive.ArcaneDamage * (1 - resistanceStats.ArcaneResistance / 100f) +
                damageStatsToReceive.PhysicalDamage * (1 - resistanceStats.PhysicalResistance / 100f);
        }
    }
}