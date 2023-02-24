using System;
using System.Collections;
using Abilities;
using Inventory.Abilities;
using Stats;
using UniRx;
using UnityEngine;

namespace Combat.Abilities
{
    public class DieAbility : Ability
    {
        public Animator animator;
        public DropAllItemsAbility dropAllItemsAbility;
        public MonoBehaviour[] componentsToDisable;
        public GameObject[] gameObjectsToDeactivate;
        public EnemyDetector enemyDetector;
        public AttributeStats attributeStats;
        
        private static readonly int Die = Animator.StringToHash("Die");

        protected override IEnumerator Execute()
        {
            animator.SetTrigger(Die);

            // Reward experience to all detected enemies
            foreach (var enemy in enemyDetector.detectedEnemies)
            {
                if (!enemy.TryGetComponent<ReceiveExperienceAbility>(out var receiveExperienceAbility)) continue;
                receiveExperienceAbility.experience = attributeStats.Experience;
                receiveExperienceAbility.Play();
            }
            
            foreach (var component in componentsToDisable)
            {
                component.enabled = false;
            }

            foreach (var go in gameObjectsToDeactivate)
            {
                go.SetActive(false);
            }

            if (dropAllItemsAbility != null)
            {
                yield return dropAllItemsAbility.Play();
            }

            foreach (var ability in GetComponents<Ability>())
            {
                ability.Disable();
            }
        }
    }
}