using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Stats.Vitals
{
    public class OverheadHealthBarUI : MonoBehaviour
    {
        public RawImage healthBar;
        public VitalStats vitalStats;

        private void OnEnable()
        {
            vitalStats
                .ObserveEveryValueChanged(v => v.Health)
                .CombineLatest(vitalStats.ObserveEveryValueChanged(v => v.MaxHealth), (health, maxHealth) => (health, maxHealth))
                .Subscribe(tuple =>
                {
                    var (health, maxHealth) = tuple;
                    healthBar.rectTransform
                        .SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, health / maxHealth);
                });
        }
    }
}