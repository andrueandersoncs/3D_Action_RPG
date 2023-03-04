using System.Collections;
using Abilities;

namespace Skills.Abilities
{
    public class ChangeTransformParentSkillEffectAbility : Ability
    {
        public ApplySkillEffectAbility applySkillEffectAbility;
        
        protected override IEnumerator Execute()
        {
            transform.SetParent(applySkillEffectAbility.target.transform);
            yield break;
        }
    }
}