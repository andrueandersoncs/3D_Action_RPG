using UnityEngine;

namespace Stats
{
    public class ActionStats : MonoBehaviour, IStats<ActionStats>
    {
        public float FasterCastRate;
        public float FasterBlockRate;
        public float FasterHitRecovery;
        public float FasterRunWalk;
        public float MagicFind;
        
        public void Add(ActionStats other)
        {
            FasterCastRate += other.FasterCastRate;
            FasterBlockRate += other.FasterBlockRate;
            FasterHitRecovery += other.FasterHitRecovery;
            FasterRunWalk += other.FasterRunWalk;
            MagicFind += other.MagicFind;
        }

        public void Subtract(ActionStats other)
        {
            FasterCastRate -= other.FasterCastRate;
            FasterBlockRate -= other.FasterBlockRate;
            FasterHitRecovery -= other.FasterHitRecovery;
            FasterRunWalk -= other.FasterRunWalk;
            MagicFind -= other.MagicFind;
        }
    }
}