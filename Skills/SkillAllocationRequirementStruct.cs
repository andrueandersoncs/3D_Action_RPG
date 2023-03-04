using System;

namespace Skills
{
    [Serializable]
    public struct SkillAllocationRequirementStruct
    {
        public int amount;
        public SkillScriptableObject skill;
    }
}