using System.Collections;
using Abilities;
using Inventory.Abilities;
using UnityEngine;

namespace Combat.Abilities
{
    public class DieAbility : Ability
    {
        public Animator animator;
        public DropAllItemsAbility dropAllItemsAbility;
        public MonoBehaviour[] componentsToDisable;
        public GameObject[] gameObjectsToDeactivate;
        
        private static readonly int Die = Animator.StringToHash("Die");

        protected override IEnumerator Execute()
        {
            animator.SetTrigger(Die);

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