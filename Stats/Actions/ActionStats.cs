using UnityEngine;

namespace Stats
{
    public class ActionStats : MonoBehaviour, IStats
    {
        public float FasterCastRate;
        public float FasterBlockRate;
        public float FasterHitRecovery;
        public float FasterRunWalk;
        public float MagicFind;
        
        public void Add(IStats stats)
        {
            if (stats is not ActionStats other) return;
            FasterCastRate += other.FasterCastRate;
            FasterBlockRate += other.FasterBlockRate;
            FasterHitRecovery += other.FasterHitRecovery;
            FasterRunWalk += other.FasterRunWalk;
            MagicFind += other.MagicFind;
        }

        public void Subtract(IStats stats)
        {
            if (stats is not ActionStats other) return;
            FasterCastRate -= other.FasterCastRate;
            FasterBlockRate -= other.FasterBlockRate;
            FasterHitRecovery -= other.FasterHitRecovery;
            FasterRunWalk -= other.FasterRunWalk;
            MagicFind -= other.MagicFind;
        }
    }
}