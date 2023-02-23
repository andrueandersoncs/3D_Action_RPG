using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stats
{
    public class PlayerAttributeStatsUI : MonoBehaviour
    {
        private void OnEnable()
        {
            var uiDocument = FindObjectOfType<UIDocument>();
            var root = uiDocument.rootVisualElement;
            
            var attributeStats = GetComponent<AttributeStats>();
            
            var levelLabel = root.Q("Level").Children().OfType<Label>().ElementAt(1);
            attributeStats.ObserveEveryValueChanged(v => v.Level)
                .Subscribe(level => levelLabel.text = level.ToString());
            
            var experienceLabel = root.Q("Experience").Children().OfType<Label>().ElementAt(1);
            attributeStats.ObserveEveryValueChanged(v => v.Experience)
                .Subscribe(experience => experienceLabel.text = experience.ToString());
            
            var strengthLabel = root.Q("Strength").Children().OfType<Label>().ElementAt(1);
            attributeStats.ObserveEveryValueChanged(v => v.Strength)
                .Subscribe(strength => strengthLabel.text = strength.ToString());
            
            var dexterityLabel = root.Q("Dexterity").Children().OfType<Label>().ElementAt(1);
            attributeStats.ObserveEveryValueChanged(v => v.Dexterity)
                .Subscribe(dexterity => dexterityLabel.text = dexterity.ToString());
            
            var vitalityLabel = root.Q("Vitality").Children().OfType<Label>().ElementAt(1);
            attributeStats.ObserveEveryValueChanged(v => v.Vitality)
                .Subscribe(vitality => vitalityLabel.text = vitality.ToString());
            
            var energyLabel = root.Q("Energy").Children().OfType<Label>().ElementAt(1);
            attributeStats.ObserveEveryValueChanged(v => v.Energy)
                .Subscribe(energy => energyLabel.text = energy.ToString());
        }
    }
}