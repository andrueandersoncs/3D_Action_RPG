using UnityEngine;

namespace Stats.Vitals
{
    public class VitalStats : MonoBehaviour, IStats
    {
        public float Health;
        public float MaxHealth;
        
        public float Mana;
        public float MaxMana;
        
        public float Stamina;
        public float MaxStamina;

        public void Add(IStats stats)
        {
            if (stats is not VitalStats other) return;
            Health += other.Health;
            MaxHealth += other.MaxHealth;
            Mana += other.Mana;
            MaxMana += other.MaxMana;
            Stamina += other.Stamina;
            MaxStamina += other.MaxStamina;
        }
        
        public void Subtract(IStats stats)
        {
            if (stats is not VitalStats other) return;
            Health -= other.Health;
            MaxHealth -= other.MaxHealth;
            Mana -= other.Mana;
            MaxMana -= other.MaxMana;
            Stamina -= other.Stamina;
            MaxStamina -= other.MaxStamina;
        }
    }
}