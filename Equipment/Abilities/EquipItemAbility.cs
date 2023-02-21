using System;
using System.Collections;
using Abilities;
using Inventory;
using Stats;
using Stats.DamageTypes;
using Stats.ResistanceTypes;
using Stats.Vitals;
using UnityEngine;

namespace Equipment.Abilities
{
    public class EquipItemAbility : Ability
    {
        public Equipment equipment;
        public EquipableItem item;
        public Action<EquipmentModule.EquipItemOutput> callback = delegate(EquipmentModule.EquipItemOutput output) {  };

        protected override IEnumerator Execute()
        {
            if (!item.TryGetComponent<ContainableItem>(out _)) yield break;

            var equipItemOutput = EquipmentModule.EquipItem(new EquipmentModule.EquipItemInput
            {
                Item = item,
                Equipment = equipment,
                Slot = item.slot
            });

            if (equipItemOutput is not EquipmentModule.EquipItemOutput.EquippedItem equipped) yield break;

            RepositionGameObject();
            ApplyStatChanges();
            
            callback(equipped);
        }

        private void RepositionGameObject()
        {
            var itemTransform = item.transform;
            var itemHandle = item.handle;
            var itemHandleLocalRotation = itemHandle.localRotation;
            itemTransform.SetParent(equipment.slotTransformParents[item.slot]);
            itemTransform.localRotation = Quaternion.Inverse(itemHandleLocalRotation);
            itemTransform.localPosition = itemHandleLocalRotation * itemHandle.localPosition;
            item.gameObject.SetActive(true);
        }

        private void ApplyStatChanges()
        {
            if (item.TryGetComponent<ActionStats>(out var actionStats))
            {
                var stats = equipment.GetComponent<ActionStats>();
                stats.Add(actionStats);
            }
            if (item.TryGetComponent<AttributeStats>(out var attributeStats))
            {
                var stats = equipment.GetComponent<AttributeStats>();
                stats.Add(attributeStats);
            }
            if (item.TryGetComponent<DamageStats>(out var damageStats))
            {
                var stats = equipment.GetComponent<DamageStats>();
                stats.Add(damageStats);
            }
            if (item.TryGetComponent<ResistanceStats>(out var resistanceStats))
            {
                var stats = equipment.GetComponent<ResistanceStats>();
                stats.Add(resistanceStats);
            }
            if (item.TryGetComponent<VitalStats>(out var vitalStats))
            {
                var stats = equipment.GetComponent<VitalStats>();
                stats.Add(vitalStats);
            }
        }
    }
}