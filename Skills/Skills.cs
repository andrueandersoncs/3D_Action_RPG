using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;

namespace Skills
{
    public class Skills : MonoBehaviour
    {
        public Skill[] initialSkillPointAllocations;
        public ReactiveCollection<Skill> skillPointAllocations = new();
        
        public IEnumerable<Skill> DistinctSkills => skillPointAllocations.Distinct();
        

        private void Start()
        {
            skillPointAllocations.AddRange(initialSkillPointAllocations);
        }

        // locked skills
        // unlocked skills
        // skill points
        // skill dependency graph
    }
}