using System.Collections;
using Abilities;
using UnityEngine;

namespace Skills
{
    public class ReceiveSkillPointsAbility : Ability
    {
        [Header("Dependencies")]
        public Skills skills;

        [Header("Parameters")]
        public int amount;
        
        protected override IEnumerator Execute()
        {
            skills.skillPoints.Value += amount;
            yield break;
        }
    }
}