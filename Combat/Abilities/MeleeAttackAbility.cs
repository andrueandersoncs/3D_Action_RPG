using System.Collections;
using Abilities;
using Equipment;
using Movement.Abilities;
using Stats.DamageTypes;
using UnityEngine;

namespace Combat.Abilities
{
    public class MeleeAttackAbility : Ability
    {
        public Equipment.Equipment equipment;
        public MoveToGameObjectAbility moveToGameObjectAbility;
        public Animator animator;
        public CombatGroup combatGroup;
        public CombatGroup enemy;
        public DamageStats damageStats;
        public TurnTowardGameObjectAbility turnTowardGameObjectAbility;
        
        private static readonly int RightHandAttack = Animator.StringToHash("RightHandAttack");

        private float _lastAttackTime;
        
        protected override IEnumerator Execute()
        {
            if (enemy.group == combatGroup.group) yield break;
                
            MeleeWeapon mainHandWeapon = null;
            var hasMainHand = equipment.items.TryGetValue(EquipmentSlot.MainHand, out var mainHand);
            var hasMainHandWeapon = hasMainHand && mainHand.TryGetComponent(out mainHandWeapon);

            MeleeWeapon offHandWeapon = null;
            var hasOffHand = equipment.items.TryGetValue(EquipmentSlot.OffHand, out var offHand);
            var hasOffHandWeapon = hasOffHand && offHand.TryGetComponent(out offHandWeapon);
            var weapon = hasMainHandWeapon ? mainHandWeapon : offHandWeapon;
            
            if (weapon == null) yield break;

            if (Vector3.Distance(transform.position, enemy.transform.position) > weapon.range) yield break;
            
            var timeSinceLastAttack = Time.time - _lastAttackTime;
            if (timeSinceLastAttack < weapon.delay)
            {
                yield return new WaitForSeconds(weapon.delay - timeSinceLastAttack);
            }

            turnTowardGameObjectAbility.target = enemy.gameObject;
            yield return turnTowardGameObjectAbility.Play();
            
            if (!enemy.TryGetComponent<ReceiveDamageAbility>(out var receiveDamageAbility)) yield break;
            receiveDamageAbility.damageStatsToReceive = damageStats;
            yield return receiveDamageAbility.Play();
            
            animator.SetTrigger(RightHandAttack);
            _lastAttackTime = Time.time;
        }
        
        public override void Stop()
        {
            base.Stop();
            moveToGameObjectAbility.Stop();
        }
    }
}