using Stats;
using UnityEngine;

namespace Skills.Effects
{
    public class IncreaseStatsEffect : SkillEffect
    {
        public MonoBehaviour[] statsToApply;
        
        public override void Apply(GameObject target)
        {
            var receivers = target.GetComponents<IStats>();
            foreach (var statToApply in statsToApply)
            {
                if (statToApply is not IStats iStat) continue;
                foreach (var receiver in receivers)
                {
                    receiver.Add(iStat);
                }
            }
        }

        public override void Unapply(GameObject target)
        {
            var receivers = target.GetComponents<IStats>();
            foreach (var statToApply in statsToApply)
            {
                if (statToApply is not IStats iStat) continue;
                foreach (var receiver in receivers)
                {
                    receiver.Subtract(iStat);
                }
            }
        }

        public override string GetName() => "IncreaseStatsEffect";
    }
}