using Abilities;
using Combat.Abilities;
using Mouse;
using Player.Abilities;
using UnityEngine;

namespace Player
{
    // Decide what the player character should be doing at any point in time
    public class PlayerController : MonoBehaviour
    {
        // Handle movement via Abilities
        // Handle attacking via Abilities
        // Handle opening chests/item containers via Abilities
        
        public PlayerMouseInput playerMouseInput;
        public MeleeAttackAbility meleeAttackAbility;
        public OpenItemContainerAbility openItemContainerAbility;
        
        private void OnEnable()
        {
            
        }
    }
}