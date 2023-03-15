using System;
using System.Collections;
using Abilities;
using Inventory.Abilities;
using Stats;
using UnityEngine;

namespace Combat.Abilities
{
    public class DieAbility : Ability
    {
        [Header("Dependencies")]
        public Animator animator;
        public EnemyDetector enemyDetector;
        public AttributeStats attributeStats;
        public DropAllItemsAbility dropAllItemsAbility;
        
        [Header("Configuration")]
        public MonoBehaviour[] componentsToDisable;
        public MonoBehaviour[] componentsToDestroy;
        public GameObject[] gameObjectsToDeactivate;
        
        private static readonly int Die = Animator.StringToHash("Die");

        private void Start()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

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
            
            foreach (var component in componentsToDestroy)
            {
                Destroy(component);
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
                ability.Stop();
                Destroy(ability);
            }
        }
    }
}