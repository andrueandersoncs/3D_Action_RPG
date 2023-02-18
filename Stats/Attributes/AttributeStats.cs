using UnityEngine;

namespace Stats
{
    public class AttributeStats : MonoBehaviour, IStats<AttributeStats>
    {
        public float Strength;
        public float Dexterity;
        public float Vitality;
        public float Energy;
        
        public void Add(AttributeStats other)
        {
            Strength += other.Strength;
            Dexterity += other.Dexterity;
            Vitality += other.Vitality;
            Energy += other.Energy;
        }
        
        public void Subtract(AttributeStats other)
        {
            Strength -= other.Strength;
            Dexterity -= other.Dexterity;
            Vitality -= other.Vitality;
            Energy -= other.Energy;
        }
    }
}