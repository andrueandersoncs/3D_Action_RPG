using Stats.Vitals;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class DeathDetector : MonoBehaviour
    {
        public UnityEvent onDeathDetected;
        public UnityEvent<GameObject> onDeathDetectedOther;
        public VitalStats vitalStats;

        private void OnEnable()
        {
            vitalStats
                .ObserveEveryValueChanged(v => v.Health)
                .Where(health => health <= 0)
                .Subscribe(_ =>
                {
                    onDeathDetected.Invoke();
                    onDeathDetectedOther.Invoke(vitalStats.gameObject);
                });
        }
    }
}