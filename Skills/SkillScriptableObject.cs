using System;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/New", order = 0)]
    public class SkillScriptableObject : ScriptableObject
    {
        public Texture2D icon;
        public string animationLoadTrigger;
        public string animationReleaseTrigger;
        
        public string skillName;
        public string description;
        
        public int levelRequirement;
        public SkillAllocationRequirementStruct[] skillAllocationRequirements;
        public int maxAllocations;

        // Create Projectile (Direction)
        // - Firebolt
        // - Fireball
        // - Icebolt
        // - Ice Blast
        // - Charged Bolt
        // - Lightning
        // - Chain Lightning

        // Add Effect (Self)
        // - Warmth
        // - Blaze
        // - Enchant
        // - Fire Mastery
        // - Frozen Armor
        // - Shiver Armor
        // - Chilling Armor
        // - Cold Mastery
        // - Energy Shield
        // - Lightning Mastery
        
        // Create Channel (Direction)
        // - Inferno
        
        // Create Object (Location)
        // - Firewall
        // - Meteor
        // - Hydra
        // - Frost Nova
        // - Glacial Spike
        // - Blizzard
        // - Frozen Orb
        // - Telekinesis
        // - Static Field
        // - Nova
        // - Teleport
        // - Thunderstorm

        // Maximum range from the player that the prefab can be spawned
        public float range;
        public float manaCost;
        public float castTime;
        public float cooldown;

        public GameObject prefab;
    }
}