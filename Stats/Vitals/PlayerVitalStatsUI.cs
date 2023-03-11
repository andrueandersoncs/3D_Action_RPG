using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stats.Vitals
{
    public class PlayerVitalStatsUI : MonoBehaviour
    {
        private void OnEnable()
        {
            var uiDocument = FindObjectOfType<UIDocument>();
            var root = uiDocument.rootVisualElement;
            
            var vitalStats = GetComponent<VitalStats>();

            var healthGlobe = root.Q("HealthGlobe");
            var healthGlobeLabel = root.Q<Label>("HealthGlobeLabel");
            var healthLabel = root.Q("Health").Children().OfType<Label>().ElementAt(1);
            var maxHealthLabel = root.Q("MaxHealth").Children().OfType<Label>().ElementAt(1);
            
            vitalStats.ObserveEveryValueChanged(v => v.Health)
                .CombineLatest(vitalStats.ObserveEveryValueChanged(v => v.MaxHealth), (health, maxHealth) => (health, maxHealth))
                .Subscribe(tuple =>
                {
                    var (health, maxHealth) = tuple;
                    healthGlobe.style.height = Length.Percent(health / maxHealth * 100f);
                    healthGlobeLabel.text = $"Health: {Mathf.Round(health)} / {Mathf.Round(maxHealth)}";
                    healthLabel.text = Mathf.Round(health).ToString();
                    maxHealthLabel.text = Mathf.Round(maxHealth).ToString();
                });
            
            var manaGlobe = root.Q("ManaGlobe");
            var manaGlobeLabel = root.Q<Label>("ManaGlobeLabel");
            var manaLabel = root.Q("Mana").Children().OfType<Label>().ElementAt(1);
            var maxManaLabel = root.Q("MaxMana").Children().OfType<Label>().ElementAt(1);
            
            vitalStats.ObserveEveryValueChanged(v => v.Mana)
                .CombineLatest(vitalStats.ObserveEveryValueChanged(v => v.MaxMana), (mana, maxMana) => (mana, maxMana))
                .Subscribe(tuple =>
                {
                    var (mana, maxMana) = tuple;
                    manaGlobe.style.height = Length.Percent(mana / maxMana * 100f);
                    manaGlobeLabel.text = $"Mana: {Mathf.Round(mana)} / {Mathf.Round(maxMana)}";
                    manaLabel.text = Mathf.Round(mana).ToString();
                    maxManaLabel.text = Mathf.Round(maxMana).ToString();
                });
            
            var staminaLabel = root.Q("Stamina").Children().OfType<Label>().ElementAt(1);
            vitalStats.ObserveEveryValueChanged(v => v.Stamina)
                .Subscribe(stamina => staminaLabel.text = Mathf.Round(stamina).ToString());
            
            var maxStaminaLabel = root.Q("MaxStamina").Children().OfType<Label>().ElementAt(1);
            vitalStats.ObserveEveryValueChanged(v => v.MaxStamina)
                .Subscribe(maxStamina => maxStaminaLabel.text = Mathf.Round(maxStamina).ToString());
        }
    }
}