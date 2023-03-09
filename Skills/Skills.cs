using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Skills.Effects;
using UniRx;
using UnityEngine;

namespace Skills
{
    public class Skills : MonoBehaviour
    {
        public SkillScriptableObject[] initialSkillPointAllocations;
        public ReactiveCollection<SkillScriptableObject> skillPointAllocations = new();
        
        public ReactiveProperty<int> skillPoints = new(0);
        
        public SkillScriptableObject[] availableSkills;
        
        [ShowInInspector]
        public Dictionary<string, SkillEffect> skillEffects = new();

        public IEnumerable<SkillScriptableObject> DistinctSkills => skillPointAllocations.Distinct();

        private void Start()
        {
            skillPointAllocations.AddRange(initialSkillPointAllocations);
        }
    }
}