using System.Collections;
using Abilities;
using Stats;
using UnityEngine;

namespace Combat.Abilities
{
    public class ReceiveExperienceAbility : Ability
    {
        public AttributeStats attributeStats;
        public int experience;
        
        protected override IEnumerator Execute()
        {
            attributeStats.Experience += experience;
            
            while (attributeStats.Experience >= attributeStats.MaxExperience)
            {
                attributeStats.Experience -= attributeStats.MaxExperience;
                attributeStats.Level += 1;
                attributeStats.MaxExperience =
                    Mathf.RoundToInt((attributeStats.Level + 500) + (Mathf.Pow(attributeStats.Level, 2) * 250));
            }

            yield break;
        }
    }
}