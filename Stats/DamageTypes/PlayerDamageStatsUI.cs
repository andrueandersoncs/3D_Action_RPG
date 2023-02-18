using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stats.DamageTypes
{
    public class PlayerDamageStatsUI : MonoBehaviour
    {
        private void OnEnable()
        {
            var uiDocument = FindObjectOfType<UIDocument>();
            var root = uiDocument.rootVisualElement;
            
            var damageStats = GetComponent<DamageStats>();
            
            var fireDamageLabel = root.Q("FireDamage").Children().OfType<Label>().ElementAt(1);
            damageStats.ObserveEveryValueChanged(v => v.FireDamage)
                .Subscribe(fireDamage => fireDamageLabel.text = fireDamage.ToString());
            
            var coldDamageLabel = root.Q("ColdDamage").Children().OfType<Label>().ElementAt(1);
            damageStats.ObserveEveryValueChanged(v => v.ColdDamage)
                .Subscribe(coldDamage => coldDamageLabel.text = coldDamage.ToString());
            
            var lightningDamageLabel = root.Q("LightningDamage").Children().OfType<Label>().ElementAt(1);
            damageStats.ObserveEveryValueChanged(v => v.LightningDamage)
                .Subscribe(lightningDamage => lightningDamageLabel.text = lightningDamage.ToString());
            
            var poisonDamageLabel = root.Q("PoisonDamage").Children().OfType<Label>().ElementAt(1);
            damageStats.ObserveEveryValueChanged(v => v.PoisonDamage)
                .Subscribe(poisonDamage => poisonDamageLabel.text = poisonDamage.ToString());
            
            var arcaneDamageLabel = root.Q("ArcaneDamage").Children().OfType<Label>().ElementAt(1);
            damageStats.ObserveEveryValueChanged(v => v.ArcaneDamage)
                .Subscribe(arcaneDamage => arcaneDamageLabel.text = arcaneDamage.ToString());
            
            var physicalDamageLabel = root.Q("PhysicalDamage").Children().OfType<Label>().ElementAt(1);
            damageStats.ObserveEveryValueChanged(v => v.PhysicalDamage)
                .Subscribe(physicalDamage => physicalDamageLabel.text = physicalDamage.ToString());
        }
    }
}