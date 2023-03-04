using System.Collections;
using System.Linq;
using Abilities;
using UnityEngine;

namespace Skills
{
    public class SpendSkillPointAbility : Ability
    {
        [Header("Dependencies")]
        public Skills skills;
        
        [Header("Parameters")]
        public SkillScriptableObject skillScriptableObject;
        
        protected override IEnumerator Execute()
        {
            if (!GetCanAllocateSkillPoint(skillScriptableObject)) yield break;
            skills.skillPointAllocations.Add(skillScriptableObject);
            skills.skillPoints.Value--;
        }
        
        private bool GetCanAllocateSkillPoint(SkillScriptableObject skillScriptableObject)
        {
            var skillIsAvailable = skills.availableSkills.Contains(skillScriptableObject);
            if (!skillIsAvailable) return false;
            
            var hasNoSkillPoints = skills.skillPoints.Value <= 0;
            if (hasNoSkillPoints) return false;
            
            var thisSkillAllocations = skills.skillPointAllocations.Count(skill => skill == skillScriptableObject);
            var hasMaxAllocations = thisSkillAllocations >= skillScriptableObject.maxAllocations;
            if (hasMaxAllocations) return false;
            
            var hasAllocationRequirements = skillScriptableObject.skillAllocationRequirements.All(r =>
                    skills.skillPointAllocations.Count(skill => skill == r.skill) >= r.amount);

            return hasAllocationRequirements;
        }
    }
}