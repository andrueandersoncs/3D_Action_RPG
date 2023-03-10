using System.Collections;
using Abilities;
using Movement.Abilities;
using Stats;
using Stats.Vitals;
using UnityEngine;
using UnityEngine.Serialization;

namespace Skills.Abilities
{
    public class UseSkillAbility : Ability
    {
        [Header("Parameters")]
        [FormerlySerializedAs("skill")] public SkillScriptableObject skillScriptableObject;
        public Vector3 target;
        
        [Header("Dependencies")]
        public Animator animator;
        public AttributeStats attributeStats;
        public VitalStats vitalStats;
        public Transform projectileSpawnPoint;
        public TurnTowardLocationAbility turnTowardLocationAbility;
        
        private float _cooldownEndTime;
        
        protected override IEnumerator Execute()
        {
            if (!RequirementsMet()) yield break;
            yield return TurnTowardTarget();
            yield return PlayAnimations();
            SpendResources();
            InstantiatePrefab();
        }
        
        private bool RequirementsMet() =>
            attributeStats.Level >= skillScriptableObject.levelRequirement
            && skillScriptableObject.manaCost <= vitalStats.Mana
            && Time.time >= _cooldownEndTime;

        private IEnumerator TurnTowardTarget()
        {
            if (skillScriptableObject.prefab == null) yield break;
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
        
        private void InstantiatePrefab()
        {
            if (skillScriptableObject.prefab == null) return;
            
            var towardTarget = target - transform.position;
            towardTarget.y = 0f;
            var rotationTowardTarget = Quaternion.LookRotation(towardTarget);
            var spawnPosition = projectileSpawnPoint.position + Vector3.ClampMagnitude(towardTarget, skillScriptableObject.range);
            var instance = Instantiate(skillScriptableObject.prefab, spawnPosition, rotationTowardTarget);
            
            if (!instance.TryGetComponent<ApplySkillEffectsAbility>(out var applySkillEffectsAbility)) return;
            applySkillEffectsAbility.target = gameObject;
            applySkillEffectsAbility.skillName = skillScriptableObject.skillName;
            applySkillEffectsAbility.FireAndForget();
        }
    }
}