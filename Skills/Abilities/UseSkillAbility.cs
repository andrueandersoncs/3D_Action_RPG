using System.Collections;
using Abilities;
using Combat;
using Combat.Abilities;
using Movement.Abilities;
using Stats;
using Stats.Vitals;
using UnityEngine;
using UnityEngine.Serialization;

namespace Skills
{
    public class UseSkillAbility : Ability
    {
        // Inputs
        [Header("Parameters")]
        [FormerlySerializedAs("skill")] public SkillScriptableObject skillScriptableObject;
        public Vector3 target;
        
        // Dependencies
        [Header("Dependencies")]
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
            attributeStats.Level >= skillScriptableObject.levelRequirement
            && skillScriptableObject.manaCost <= vitalStats.Mana
            && Time.time >= _cooldownEndTime;

        private IEnumerator TurnTowardTarget()
        {
            turnTowardLocationAbility.objectToTurn = transform;
            turnTowardLocationAbility.location = target;
            yield return turnTowardLocationAbility.Play();
        }
        
        private IEnumerator PlayAnimations()
        {
            if (skillScriptableObject.animationLoadTrigger != "")
            {
                animator.SetTrigger(skillScriptableObject.animationLoadTrigger);
                yield return new WaitForSeconds(skillScriptableObject.castTime);    
            }

            if (skillScriptableObject.animationReleaseTrigger != "")
            {
                animator.SetTrigger(skillScriptableObject.animationReleaseTrigger);
                yield return new WaitForSeconds(0.25f);    
            }
        }
        
        private void SpendResources()
        {
            vitalStats.Mana -= skillScriptableObject.manaCost;
            _cooldownEndTime = Time.time + skillScriptableObject.cooldown;
        }
        
        private void InstantiateProjectile()
        {
            var towardTarget = target - transform.position;
            towardTarget.y = 0f;
            
            var rotationTowardTarget = Quaternion.LookRotation(towardTarget);
            
            var spawnPosition = projectileSpawnPoint.position + Vector3.ClampMagnitude(towardTarget, skillScriptableObject.range);
            
            var instance = Instantiate(skillScriptableObject.prefab, spawnPosition, rotationTowardTarget);
            
            if (!instance.TryGetComponent<ApplySkillEffectAbility>(out var applySkillEffectAbility)) return;
            applySkillEffectAbility.target = gameObject;
            applySkillEffectAbility.PlayAll();
        }
    }
}