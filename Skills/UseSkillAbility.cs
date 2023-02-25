using System.Collections;
using Abilities;
using Stats;
using Stats.Vitals;
using UnityEngine;

namespace Skills
{
    public class UseSkillAbility : Ability
    {
        public Skill skill;
        public Transform target;
        public Animator animator;
        
        public AttributeStats attributeStats;
        public VitalStats vitalStats;

        private float _cooldownEndTime;
        
        protected override IEnumerator Execute()
        {
            // check level requirement
            if (skill.levelRequirement > attributeStats.Level)
            {
                Debug.Log("Level requirement not met");
                yield break;
            }
            
            // check mana cost
            if (skill.manaCost > vitalStats.Mana)
            {
                Debug.Log("Not enough mana");
                yield break;
            }
            
            vitalStats.Mana -= skill.manaCost;

            // check cooldown
            if (Time.time < _cooldownEndTime)
            {
                Debug.Log("Skill on cooldown");
                yield break;
            }
            
            _cooldownEndTime = Time.time + skill.cooldown;
            
            // play animation
            animator.SetTrigger(skill.animationTrigger);
            
            // wait cast time
            yield return new WaitForSeconds(skill.castTime);
            
            // spawn prefab
            Instantiate(skill.prefab, target.position, target.rotation, target);
        }
    }
}