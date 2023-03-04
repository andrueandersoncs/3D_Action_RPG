using System.Collections;
using Abilities;
using Skills;
using Stats;
using UnityEngine;

namespace Combat.Abilities
{
    public class ReceiveExperienceAbility : Ability
    {
        // Dependencies
        public AttributeStats attributeStats;
        public ReceiveSkillPointsAbility receiveSkillPointsAbility;
        
        // Parameters
        public int experience;
        
        protected override IEnumerator Execute()
        {
            attributeStats.Experience += experience;
            
            while (attributeStats.Experience >= attributeStats.MaxExperience)
            {
                LevelUp();
            }

            yield break;
        }
        
        private void LevelUp()
        {
            attributeStats.Experience -= attributeStats.MaxExperience;
            attributeStats.Level += 1;
            attributeStats.MaxExperience =
                Mathf.RoundToInt((attributeStats.Level + 500) + (Mathf.Pow(attributeStats.Level, 2) * 250));
            
            receiveSkillPointsAbility.amount = 1;
            receiveSkillPointsAbility.FireAndForget();
        }
    }
}