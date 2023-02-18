using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stats
{
    public class PlayerActionStatsUI : MonoBehaviour
    {
        private void OnEnable()
        {
            var uiDocument = FindObjectOfType<UIDocument>();
            var root = uiDocument.rootVisualElement;
            
            var actionStats = GetComponent<ActionStats>();

            var fasterCastRateLabel = root.Q("FasterCastRate").Children().OfType<Label>().ElementAt(1);
            actionStats.ObserveEveryValueChanged(v => v.FasterCastRate)
                .Subscribe(fasterCastRate => fasterCastRateLabel.text = fasterCastRate.ToString());
            
            var fasterBlockRateLabel = root.Q("FasterBlockRate").Children().OfType<Label>().ElementAt(1);
            actionStats.ObserveEveryValueChanged(v => v.FasterBlockRate)
                .Subscribe(fasterBlockRate => fasterBlockRateLabel.text = fasterBlockRate.ToString());
            
            var fasterHitRecoveryLabel = root.Q("FasterHitRecovery").Children().OfType<Label>().ElementAt(1);
            actionStats.ObserveEveryValueChanged(v => v.FasterHitRecovery)
                .Subscribe(fasterHitRecovery => fasterHitRecoveryLabel.text = fasterHitRecovery.ToString());
            
            var fasterRunWalkLabel = root.Q("FasterRunWalk").Children().OfType<Label>().ElementAt(1);
            actionStats.ObserveEveryValueChanged(v => v.FasterRunWalk)
                .Subscribe(fasterRunWalk => fasterRunWalkLabel.text = fasterRunWalk.ToString());
            
            var magicFindLabel = root.Q("MagicFind").Children().OfType<Label>().ElementAt(1);
            actionStats.ObserveEveryValueChanged(v => v.MagicFind)
                .Subscribe(magicFind => magicFindLabel.text = magicFind.ToString());
        }
    }
}