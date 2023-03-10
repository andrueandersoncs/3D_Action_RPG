using DefaultNamespace;
using Items;
using Stats.DamageTypes;
using Stats.ResistanceTypes;
using Stats.Vitals;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stats
{
    public class ItemStatsUI : MonoBehaviour, IUserInterface
    {
        [Header("Dependencies")]
        public ActionStats actionStats;
        public AttributeStats attributeStats;
        public DamageStats damageStats;
        public ResistanceStats resistanceStats;
        public VitalStats vitalStats;
        public ContainableItemUI containableItemUI;
        
        private VisualElement _visualElement;

        private void Start()
        {
            _visualElement = new VisualElement
            {
                style =
                {
                    display = DisplayStyle.None,
                    position = Position.Absolute,
                    backgroundColor = new Color(0.2666f, 0.2666f, 0.2666f),
                    color = Color.white,
                    borderBottomWidth = 1,
                    borderBottomColor = Color.white,
                    borderLeftWidth = 1,
                    borderLeftColor = Color.white,
                    borderRightWidth = 1,
                    borderRightColor = Color.white,
                    borderTopWidth = 1,
                    borderTopColor = Color.white,
                }
            };
            
            if (vitalStats != null)
            {
                AddVitalStats();
            }
            
            if (attributeStats != null)
            {
                AddAttributeStats();
            }
            
            if (damageStats != null)
            {
                AddDamageStats();
            }
            
            if (resistanceStats != null)
            {
                AddResistanceStats();
            }
            
            if (actionStats != null)
            {
                AddActionStats();
            }

            var containableItemUIVisualElement = containableItemUI.GetVisualElement();
            
            containableItemUIVisualElement.RegisterCallback<MouseEnterEvent, VisualElement>((evt, visualElement) =>
            {
                visualElement.style.display = DisplayStyle.Flex;
                visualElement.style.left = evt.mousePosition.x + 5;
                visualElement.style.top = evt.mousePosition.y + 5;
            }, _visualElement);
            
            containableItemUIVisualElement.RegisterCallback<MouseMoveEvent, VisualElement>((evt, visualElement) =>
            {
                visualElement.style.left = evt.mousePosition.x + 5;
                visualElement.style.top = evt.mousePosition.y + 5;
            }, _visualElement);
            
            containableItemUIVisualElement.RegisterCallback<MouseLeaveEvent, VisualElement>((_, visualElement) =>
            {
                visualElement.style.display = DisplayStyle.None;
            }, _visualElement);
            
            UserInterface.Root.Add(_visualElement);
        }

        private void AddVitalStats()
        {
            if (vitalStats.MaxHealth != 0f)
            {
                var maxHealthLabelContainer = new VisualElement();
                maxHealthLabelContainer.Add(new Label {text = "Max Health: " + vitalStats.MaxHealth});
                _visualElement.Add(maxHealthLabelContainer);
            }
            if (vitalStats.MaxMana != 0f)
            {
                var maxManaLabelContainer = new VisualElement();
                maxManaLabelContainer.Add(new Label {text = "Max Mana: " + vitalStats.MaxMana});
                _visualElement.Add(maxManaLabelContainer);
            }
            if (vitalStats.MaxStamina != 0f)
            {
                var maxStaminaLabelContainer = new VisualElement();
                maxStaminaLabelContainer.Add(new Label {text = "Max Stamina: " + vitalStats.MaxStamina});
                _visualElement.Add(maxStaminaLabelContainer);
            }
        }

        private void AddResistanceStats()
        {
            if (resistanceStats.ArcaneResistance != 0f)
            {
                var arcaneResistanceLabelContainer = new VisualElement();
                arcaneResistanceLabelContainer.Add(new Label {text = "Arcane Resistance: " + resistanceStats.ArcaneResistance});
                _visualElement.Add(arcaneResistanceLabelContainer);
            }
            if (resistanceStats.ColdResistance != 0f)
            {
                var coldResistanceLabelContainer = new VisualElement();
                coldResistanceLabelContainer.Add(new Label {text = "Cold Resistance: " + resistanceStats.ColdResistance});
                _visualElement.Add(coldResistanceLabelContainer);
            }
            if (resistanceStats.FireResistance != 0f)
            {
                var fireResistanceLabelContainer = new VisualElement();
                fireResistanceLabelContainer.Add(new Label {text = "Fire Resistance: " + resistanceStats.FireResistance});
                _visualElement.Add(fireResistanceLabelContainer);
            }
            if (resistanceStats.LightningResistance != 0f)
            {
                var lightningResistanceLabelContainer = new VisualElement();
                lightningResistanceLabelContainer.Add(new Label {text = "Lightning Resistance: " + resistanceStats.LightningResistance});
                _visualElement.Add(lightningResistanceLabelContainer);
            }
            if (resistanceStats.PoisonResistance != 0f)
            {
                var poisonResistanceLabelContainer = new VisualElement();
                poisonResistanceLabelContainer.Add(new Label {text = "Poison Resistance: " + resistanceStats.PoisonResistance});
                _visualElement.Add(poisonResistanceLabelContainer);
            }
            if (resistanceStats.PhysicalResistance != 0f)
            {
                var physicalResistanceLabelContainer = new VisualElement();
                physicalResistanceLabelContainer.Add(new Label {text = "Physical Resistance: " + resistanceStats.PhysicalResistance});
                _visualElement.Add(physicalResistanceLabelContainer);
            }
        }

        private void AddDamageStats()
        {
            if (damageStats.ArcaneDamage != 0f)
            {
                var arcaneDamageLabelContainer = new VisualElement();
                arcaneDamageLabelContainer.Add(new Label {text = "Arcane Damage: " + damageStats.ArcaneDamage});
                _visualElement.Add(arcaneDamageLabelContainer);
            }
            if (damageStats.ColdDamage != 0f)
            {
                var coldDamageLabelContainer = new VisualElement();
                coldDamageLabelContainer.Add(new Label {text = "Cold Damage: " + damageStats.ColdDamage});
                _visualElement.Add(coldDamageLabelContainer);
            }
            if (damageStats.FireDamage != 0f)
            {
                var fireDamageLabelContainer = new VisualElement();
                fireDamageLabelContainer.Add(new Label {text = "Fire Damage: " + damageStats.FireDamage});
                _visualElement.Add(fireDamageLabelContainer);
            }
            if (damageStats.LightningDamage != 0f)
            {
                var lightningDamageLabelContainer = new VisualElement();
                lightningDamageLabelContainer.Add(new Label {text = "Lightning Damage: " + damageStats.LightningDamage});
                _visualElement.Add(lightningDamageLabelContainer);
            }
            if (damageStats.PoisonDamage != 0f)
            {
                var poisonDamageLabelContainer = new VisualElement();
                poisonDamageLabelContainer.Add(new Label {text = "Poison Damage: " + damageStats.PoisonDamage});
                _visualElement.Add(poisonDamageLabelContainer);
            }
            if (damageStats.PhysicalDamage != 0f)
            {
                var physicalDamageLabelContainer = new VisualElement();
                physicalDamageLabelContainer.Add(new Label {text = "Physical Damage: " + damageStats.PhysicalDamage});
                _visualElement.Add(physicalDamageLabelContainer);
            }
        }

        private void AddAttributeStats()
        {
            if (attributeStats.Strength != 0f)
            {
                var strengthLabelContainer = new VisualElement();
                strengthLabelContainer.Add(new Label {text = "Strength: " + attributeStats.Strength});
                _visualElement.Add(strengthLabelContainer);
            }
            if (attributeStats.Dexterity != 0f)
            {
                var dexterityLabelContainer = new VisualElement();
                dexterityLabelContainer.Add(new Label {text = "Dexterity: " + attributeStats.Dexterity});
                _visualElement.Add(dexterityLabelContainer);
            }
            if (attributeStats.Energy != 0f)
            {
                var energyLabelContainer = new VisualElement();
                energyLabelContainer.Add(new Label {text = "Energy: " + attributeStats.Energy});
                _visualElement.Add(energyLabelContainer);
            }
            if (attributeStats.Vitality != 0f)
            {
                var vitalityLabelContainer = new VisualElement();
                vitalityLabelContainer.Add(new Label {text = "Vitality: " + attributeStats.Vitality});
                _visualElement.Add(vitalityLabelContainer);
            }
        }

        private void OnDestroy()
        {
            UserInterface.Root.Remove(_visualElement);
        }

        public VisualElement GetVisualElement()
        {
            return _visualElement;
        }

        private void AddActionStats()
        {
            if (actionStats.FasterCastRate != 0f)
            {
                var fasterCastRateLabelContainer = new VisualElement();
                fasterCastRateLabelContainer.Add(new Label {text = "Faster Cast Rate: " + actionStats.FasterCastRate});
                _visualElement.Add(fasterCastRateLabelContainer);
            }
            
            if (actionStats.FasterBlockRate != 0f)
            {
                var fasterBlockRateLabelContainer = new VisualElement();
                fasterBlockRateLabelContainer.Add(new Label {text = "Faster Block Rate: " + actionStats.FasterBlockRate});
                _visualElement.Add(fasterBlockRateLabelContainer);
            }
            
            if (actionStats.FasterHitRecovery != 0f)
            {
                var fasterHitRecoveryLabelContainer = new VisualElement();
                fasterHitRecoveryLabelContainer.Add(new Label {text = "Faster Hit Recovery: " + actionStats.FasterHitRecovery});
                _visualElement.Add(fasterHitRecoveryLabelContainer);
            }
            
            if (actionStats.FasterRunWalk != 0f)
            {
                var fasterRunWalkLabelContainer = new VisualElement();
                fasterRunWalkLabelContainer.Add(new Label {text = "Faster Run/Walk: " + actionStats.FasterRunWalk});
                _visualElement.Add(fasterRunWalkLabelContainer);
            }
            
            if (actionStats.MagicFind != 0f)
            {
                var magicFindLabelContainer = new VisualElement();
                magicFindLabelContainer.Add(new Label {text = "Magic Find: " + actionStats.MagicFind});
                _visualElement.Add(magicFindLabelContainer);
            }
        }
    }
}