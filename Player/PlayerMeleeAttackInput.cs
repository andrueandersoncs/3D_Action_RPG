using Combat;
using Combat.Abilities;
using Mouse;
using Movement.Abilities;
using Stats.Vitals;
using UnityEngine;

namespace Player
{
    public class PlayerMeleeAttackInput : MonoBehaviour, IMouseInputConsumer
    {
        public KeyCode stationaryKey = KeyCode.LeftShift;
        public MeleeAttackAbility meleeAttackAbility;
        public MoveToGameObjectAbility moveToGameObjectAbility;
        
        public bool OnMouseInput(MouseInput mouseInput)
        {
            foreach (var hit in mouseInput.hits)
            {
                var hasCombatGroup = hit.collider.TryGetComponent<CombatGroup>(out var enemy);
                var hasVitalStats = hit.collider.TryGetComponent<VitalStats>(out _);
                if (!hasCombatGroup || !hasVitalStats) continue;
                
                if (Input.GetKey(stationaryKey))
                {
                    meleeAttackAbility.enemy = enemy;
                    meleeAttackAbility.Play();    
                }
                else
                {
                    moveToGameObjectAbility.target = enemy.gameObject;
                    meleeAttackAbility.enemy = enemy;
                    moveToGameObjectAbility.nextAbilities = new[] {meleeAttackAbility};
                    moveToGameObjectAbility.PlayAll();
                }
                
                return true;
            }
            
            return false;
        }
    }
}