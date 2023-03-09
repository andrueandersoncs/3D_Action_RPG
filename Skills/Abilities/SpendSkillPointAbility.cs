using System.Collections;
using System.Linq;
using Abilities;
using UnityEngine;

namespace Skills.Abilities
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
        
        private bool GetCanAllocateSkillPoint(SkillScriptableObject skillInput)
        {
            var skillIsAvailable = skills.availableSkills.Contains(skillInput);
            if (!skillIsAvailable) return false;
            
            var hasNoSkillPoints = skills.skillPoints.Value <= 0;
            if (hasNoSkillPoints) return false;
            
            var thisSkillAllocations = skills.skillPointAllocations.Count(skill => skill == skillInput);
            var hasMaxAllocations = thisSkillAllocations >= skillInput.maxAllocations;
            if (hasMaxAllocations) return false;
            
            var hasAllocationRequirements = skillInput.skillAllocationRequirements.All(r =>
                    skills.skillPointAllocations.Count(skill => skill == r.skill) >= r.amount);

            return hasAllocationRequirements;
        }
    }
}