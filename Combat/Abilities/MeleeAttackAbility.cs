using System.Collections;
using Abilities;
using Equipment;
using Movement.Abilities;
using Stats.DamageTypes;
using Stats.Vitals;
using UnityEngine;
using UnityEngine.Serialization;

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
        [FormerlySerializedAs("turnTowardGameObjectAbility")] public TurnTowardTargetAbility turnTowardTargetAbility;
        
        private static readonly int RightHandAttack = Animator.StringToHash("RightHandAttack");

        private float _lastAttackTime;
        
        protected override IEnumerator Execute()
        {
            if (enemy.group == combatGroup.group) yield break;

            if (!enemy.TryGetComponent<VitalStats>(out var enemyVitals)) yield break;
            if (enemyVitals.Health <= 0f) yield break;

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

            turnTowardTargetAbility.target = enemy.gameObject;
            yield return turnTowardTargetAbility.Play();
            
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