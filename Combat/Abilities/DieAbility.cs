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
        public Canvas healthBarUI;
        
        private static readonly int Die = Animator.StringToHash("Die");

        protected override IEnumerator Execute()
        {
            animator.SetTrigger(Die);

            if (healthBarUI != null)
            {
                healthBarUI.enabled = false;    
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