using System.Collections;
using Abilities;
using Stats.DamageTypes;
using UnityEngine;

namespace Combat.Abilities
{
    public class DealDamageToTargetAbility : Ability
    {
        [Header("Dependencies")]
        public CombatGroup combatGroup;
        public DamageStats damageStats;
        
        public object target { get; set; }

        protected override IEnumerator Execute()
        {
            // Debug.Log("Dealing damage to target!");
            
            if (target is not Component component)
            {
                successfullyExecuted = false;
                yield break;
            }
            // Debug.Log("Target is component");

            if (!component.TryGetComponent<CombatGroup>(out var targetCombatGroup))
            {
                successfullyExecuted = false;
                yield break;
            }
            // Debug.Log("Target has combat group");

            if (!combatGroup.CanDamage(targetCombatGroup))
            {
                successfullyExecuted = false;
                yield break;
            }
            // Debug.Log("Can damage target");

            if (!component.TryGetComponent<ReceiveDamageAbility>(out var receiveDamageAbility))
            {
                successfullyExecuted = false;
                yield break;
            }

            // Debug.Log("Target has receive damage ability");
            
            receiveDamageAbility.damageStatsToReceive = damageStats;
            receiveDamageAbility.FireAndForget();

            Debug.Log("Finished playing receive damage ability: " + receiveDamageAbility.successfullyExecuted);
            Debug.Log("Receiver: " + receiveDamageAbility.name);
            
            // successfullyExecuted = receiveDamageAbility.successfullyExecuted;
            successfullyExecuted = true;
        }
    }
}