using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stats.ResistanceTypes
{
    public class PlayerResistanceStatsUI : MonoBehaviour
    {
        private void OnEnable()
        {
            var uiDocument = FindObjectOfType<UIDocument>();
            var root = uiDocument.rootVisualElement;
            
            var resistanceStats = GetComponent<ResistanceStats>();
            
            var fireResistanceLabel = root.Q("FireResistance").Children().OfType<Label>().ElementAt(1);
            resistanceStats.ObserveEveryValueChanged(v => v.FireResistance)
                .Subscribe(fireResistance => fireResistanceLabel.text = fireResistance.ToString());
            
            var coldResistanceLabel = root.Q("ColdResistance").Children().OfType<Label>().ElementAt(1);
            resistanceStats.ObserveEveryValueChanged(v => v.ColdResistance)
                .Subscribe(coldResistance => coldResistanceLabel.text = coldResistance.ToString());
            
            var lightningResistanceLabel = root.Q("LightningResistance").Children().OfType<Label>().ElementAt(1);
            resistanceStats.ObserveEveryValueChanged(v => v.LightningResistance)
                .Subscribe(lightningResistance => lightningResistanceLabel.text = lightningResistance.ToString());
            
            var poisonResistanceLabel = root.Q("PoisonResistance").Children().OfType<Label>().ElementAt(1);
            resistanceStats.ObserveEveryValueChanged(v => v.PoisonResistance)
                .Subscribe(poisonResistance => poisonResistanceLabel.text = poisonResistance.ToString());
            
            var physicalResistanceLabel = root.Q("PhysicalResistance").Children().OfType<Label>().ElementAt(1);
            resistanceStats.ObserveEveryValueChanged(v => v.PhysicalResistance)
                .Subscribe(physicalResistance => physicalResistanceLabel.text = physicalResistance.ToString());
            
            var arcaneResistanceLabel = root.Q("ArcaneResistance").Children().OfType<Label>().ElementAt(1);
            resistanceStats.ObserveEveryValueChanged(v => v.ArcaneResistance)
                .Subscribe(arcaneResistance => arcaneResistanceLabel.text = arcaneResistance.ToString());
        }
    }
}