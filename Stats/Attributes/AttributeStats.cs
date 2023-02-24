using UnityEngine;

namespace Stats
{
    public class AttributeStats : MonoBehaviour, IStats<AttributeStats>
    {
        public int Level;
        public int Experience;
        public int MaxExperience;
        
        public float Strength;
        public float Dexterity;
        public float Vitality;
        public float Energy;
        
        public void Add(AttributeStats other)
        {
            Level += other.Level;
            Experience += other.Experience;
            MaxExperience += other.MaxExperience;
            Strength += other.Strength;
            Dexterity += other.Dexterity;
            Vitality += other.Vitality;
            Energy += other.Energy;
        }
        
        public void Subtract(AttributeStats other)
        {
            Level -= other.Level;
            Experience -= other.Experience;
            MaxExperience -= other.MaxExperience;
            Strength -= other.Strength;
            Dexterity -= other.Dexterity;
            Vitality -= other.Vitality;
            Energy -= other.Energy;
        }
    }
}