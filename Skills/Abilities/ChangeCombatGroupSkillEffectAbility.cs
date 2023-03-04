using System.Collections;
using Abilities;
using Combat;

namespace Skills.Abilities
{
    public class ChangeCombatGroupSkillEffectAbility : Ability
    {
        public ApplySkillEffectAbility applySkillEffectAbility;
        public CombatGroup combatGroup;
        
        protected override IEnumerator Execute()
        {
            if (!applySkillEffectAbility.target.TryGetComponent<CombatGroup>(out var targetCombatGroup))
            {
                successfullyExecuted = false;
                yield break;
            }
            combatGroup.group = targetCombatGroup.group;
        }
    }
}