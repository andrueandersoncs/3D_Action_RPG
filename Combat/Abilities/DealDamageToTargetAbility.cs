using System.Collections;
using Abilities;
using Stats.DamageTypes;
using UnityEngine;

namespace Combat.Abilities
{
    public class DealDamageToTargetAbility : Ability
    {
        public CombatGroup combatGroup;
        public DamageStats damageStats;
        
        public object target { get; set; }

        protected override IEnumerator Execute()
        {
            if (target is not Component component)
            {
                successfullyExecuted = false;
                yield break;
            }

            if (!component.TryGetComponent<CombatGroup>(out var targetCombatGroup))
            {
                successfullyExecuted = false;
                yield break;
            }

            if (!combatGroup.CanDamage(targetCombatGroup))
            {
                successfullyExecuted = false;
                yield break;
            }

            if (!component.TryGetComponent<ReceiveDamageAbility>(out var receiveDamageAbility))
            {
                successfullyExecuted = false;
                yield break;
            }
            
            receiveDamageAbility.damageStatsToReceive = damageStats;
            yield return receiveDamageAbility.Play();

            // Debug.Log("Finished playing receive damage ability: " + receiveDamageAbility.successfullyExecuted);
            // Debug.Log("Receiver: " + receiveDamageAbility.name);
            
            successfullyExecuted = receiveDamageAbility.successfullyExecuted;
        }
    }
}