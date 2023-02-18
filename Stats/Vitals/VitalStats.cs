using UnityEngine;

namespace Stats.Vitals
{
    public class VitalStats : MonoBehaviour, IStats<VitalStats>
    {
        public float Health;
        public float MaxHealth;
        
        public float Mana;
        public float MaxMana;
        
        public float Stamina;
        public float MaxStamina;

        public void Add(VitalStats other)
        {
            Health += other.Health;
            MaxHealth += other.MaxHealth;
            Mana += other.Mana;
            MaxMana += other.MaxMana;
            Stamina += other.Stamina;
            MaxStamina += other.MaxStamina;
        }
        
        public void Subtract(VitalStats other)
        {
            Health -= other.Health;
            MaxHealth -= other.MaxHealth;
            Mana -= other.Mana;
            MaxMana -= other.MaxMana;
            Stamina -= other.Stamina;
            MaxStamina -= other.MaxStamina;
        }
    }
}