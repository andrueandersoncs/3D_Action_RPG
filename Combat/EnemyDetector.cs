using Stats.Vitals;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Combat
{
    public class EnemyDetector : MonoBehaviour
    {
        public ReactiveCollection<GameObject> detectedEnemies = new();
        public Collider detectionTrigger;
        public CombatGroup combatGroup;
        
        private void OnEnable()
        {
            detectionTrigger
                .OnTriggerEnterAsObservable()
                .Where(collider =>
                    collider.TryGetComponent<CombatGroup>(out var otherCombatGroup)
                    && otherCombatGroup.group != combatGroup.group
                    && collider.TryGetComponent<VitalStats>(out _)
                )
                .Subscribe(collider => detectedEnemies.Add(collider.gameObject))
                .AddTo(this);

            detectionTrigger
                .OnTriggerExitAsObservable()
                .Where(collider => detectedEnemies.Contains(collider.gameObject))
                .Subscribe(collider => detectedEnemies.Remove(collider.gameObject))
                .AddTo(this);
        }
    }
}