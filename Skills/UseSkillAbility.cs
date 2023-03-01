using System.Collections;
using Abilities;
using Combat;
using Combat.Abilities;
using Movement.Abilities;
using Stats;
using Stats.Vitals;
using UnityEngine;

namespace Skills
{
    public class UseSkillAbility : Ability
    {
        // Inputs
        public Skill skill;
        public Vector3 target;
        
        // Dependencies
        public Animator animator;
        public AttributeStats attributeStats;
        public VitalStats vitalStats;
        public CombatGroup combatGroup;
        public Transform projectileSpawnPoint;
        public TurnTowardLocationAbility turnTowardLocationAbility;

        // State
        private float _cooldownEndTime;
        
        protected override IEnumerator Execute()
        {
            if (!RequirementsMet()) yield break;
            yield return TurnTowardTarget();
            yield return PlayAnimations();
            SpendResources();
            InstantiateProjectile();
        }
        
        private bool RequirementsMet() =>
            attributeStats.Level >= skill.levelRequirement
            && skill.manaCost <= vitalStats.Mana
            && Time.time >= _cooldownEndTime;

        private IEnumerator TurnTowardTarget()
        {
            turnTowardLocationAbility.objectToTurn = transform;
            turnTowardLocationAbility.location = target;
            yield return turnTowardLocationAbility.Play();
        }
        
        private IEnumerator PlayAnimations()
        {
            animator.SetTrigger(skill.animationLoadTrigger);
            yield return new WaitForSeconds(skill.castTime);
            animator.SetTrigger(skill.animationReleaseTrigger);
            yield return new WaitForSeconds(0.25f);
        }
        
        private void SpendResources()
        {
            vitalStats.Mana -= skill.manaCost;
            _cooldownEndTime = Time.time + skill.cooldown;
        }
        
        private void InstantiateProjectile()
        {
            var transformPosition = transform.position;
            var towardTarget = target - transformPosition;
            towardTarget.y = 0f;
            var rotationTowardTarget = Quaternion.LookRotation(towardTarget);
            var spawnPosition = projectileSpawnPoint.position;
            var instance = Instantiate(skill.prefab, spawnPosition, rotationTowardTarget);
            if (instance.TryGetComponent<DealDamageToTargetAbility>(out var dealDamageToTargetAbility))
            {
                dealDamageToTargetAbility.combatGroup = combatGroup;
            }
        }
    }
}