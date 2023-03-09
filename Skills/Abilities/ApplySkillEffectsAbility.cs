using System.Collections;
using Abilities;
using Skills.Effects;
using UnityEngine;

namespace Skills.Abilities
{
    public class ApplySkillEffectsAbility : Ability
    {
        [Header("Dependencies")]
        public SkillEffect[] skillEffects;
        
        [Header("Parameters")]
        public GameObject target;
        public string skillName;
        
        // keep track of previously applied skill effects,
        // there are no skill effects that can be applied twice
        // actually, the effects could be applied twice but they have to come from different skills
        
        // how do we identify a skill effect?
        // skill name + skill effect name?
        
        // how do we "unapply" a skill effect?
        // require each skill effect to implement an unapply method?
        
        protected override IEnumerator Execute()
        {
            // when applying skill effects,
            // for each skill effect,
            // check if the skill name + skill effect already exists
            // if it does, unapply it
            // then apply the new skill effect
            var skills = target.GetComponent<Skills>();
            
            foreach (var skillEffect in skillEffects)
            {
                var skillEffectKey = $"{skillName}{skillEffect.GetName()}";
                if (skills.skillEffects.TryGetValue(skillEffectKey, out var oldSkillEffect))
                {
                    oldSkillEffect.Unapply(target);
                    skills.skillEffects.Remove(skillEffectKey);
                    Destroy(oldSkillEffect.gameObject);
                }
                skillEffect.Apply(target);
                skills.skillEffects.Add(skillEffectKey, skillEffect);
            }

            yield break;
        }
    }
}