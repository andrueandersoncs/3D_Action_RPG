using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/New", order = 0)]
    public class Skill : ScriptableObject
    {
        public Texture2D icon;
        public string animationLoadTrigger;
        public string animationReleaseTrigger;
        
        public string skillName;
        public string description;
        
        public int levelRequirement;
        public float manaCost;
        
        public float castTime;
        public float duration;
        public float cooldown;

        public GameObject prefab;
    }
}