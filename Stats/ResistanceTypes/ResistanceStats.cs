using Combat;
using Stats.DamageTypes;
using UnityEngine;

namespace Stats.ResistanceTypes
{
    public class ResistanceStats : MonoBehaviour, IStats<ResistanceStats>
    {
        public float FireResistance;
        public float ColdResistance;
        public float LightningResistance;
        public float PoisonResistance;
        public float ArcaneResistance;
        public float PhysicalResistance;

        public void Add(ResistanceStats other)
        {
            FireResistance += other.FireResistance;
            ColdResistance += other.ColdResistance;
            LightningResistance += other.LightningResistance;
            PoisonResistance += other.PoisonResistance;
            ArcaneResistance += other.ArcaneResistance;
            PhysicalResistance += other.PhysicalResistance;
        }
        
        public void Subtract(ResistanceStats other)
        {
            FireResistance -= other.FireResistance;
            ColdResistance -= other.ColdResistance;
            LightningResistance -= other.LightningResistance;
            PoisonResistance -= other.PoisonResistance;
            ArcaneResistance -= other.ArcaneResistance;
            PhysicalResistance -= other.PhysicalResistance;
        }
    }
}