using System;
using System.Collections;
using Abilities;
using Stats;
using Stats.DamageTypes;
using Stats.ResistanceTypes;
using Stats.Vitals;

namespace Equipment.Abilities
{
    public class UnequipItemAbility : Ability
    {
        public Equipment equipment;
        public EquipmentSlot slot;
        public Action<EquipmentModule.UnequipItemOutput> callback = delegate(EquipmentModule.UnequipItemOutput output) {  };
        
        protected override IEnumerator Execute()
        {
            var output = EquipmentModule.UnequipItem(new EquipmentModule.UnequipItemInput { Equipment = equipment, Slot = slot });
            
            if (output is not EquipmentModule.UnequipItemOutput.UnequippedItem unequippedItem) yield break;

            RepositionGameObject(unequippedItem.Item);
            ApplyStatChanges(unequippedItem.Item);
            
            callback(unequippedItem);
        }
        
        private void RepositionGameObject(EquipableItem item)
        {
            item.transform.SetParent(null);
            item.gameObject.SetActive(false);
        }
        
        private void ApplyStatChanges(EquipableItem item)
        {
            if (item.TryGetComponent<ActionStats>(out var actionStats))
            {
                var stats = equipment.GetComponent<ActionStats>();
                stats.Subtract(actionStats);
            }
            if (item.TryGetComponent<AttributeStats>(out var attributeStats))
            {
                var stats = equipment.GetComponent<AttributeStats>();
                stats.Subtract(attributeStats);
            }
            if (item.TryGetComponent<DamageStats>(out var damageStats))
            {
                var stats = equipment.GetComponent<DamageStats>();
                stats.Subtract(damageStats);
            }
            if (item.TryGetComponent<ResistanceStats>(out var resistanceStats))
            {
                var stats = equipment.GetComponent<ResistanceStats>();
                stats.Subtract(resistanceStats);
            }
            if (item.TryGetComponent<VitalStats>(out var vitalStats))
            {
                var stats = equipment.GetComponent<VitalStats>();
                stats.Subtract(vitalStats);
            }
        }
    }
}