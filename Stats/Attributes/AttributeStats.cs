using UnityEngine;

namespace Stats
{
    public class AttributeStats : MonoBehaviour, IStats
    {
        public int Level;
        public int Experience;
        public int MaxExperience;
        
        public float Strength;
        public float Dexterity;
        public float Vitality;
        public float Energy;
        
        public void Add(IStats stats)
        {
            if (stats is not AttributeStats other) return;
            Level += other.Level;
            Experience += other.Experience;
            MaxExperience += other.MaxExperience;
            Strength += other.Strength;
            Dexterity += other.Dexterity;
            Vitality += other.Vitality;
            Energy += other.Energy;
        }
        
        public void Subtract(IStats stats)
        {
            if (stats is not AttributeStats other) return;
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