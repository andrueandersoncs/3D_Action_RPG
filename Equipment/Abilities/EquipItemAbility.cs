using System;
using Inventory;
using UnityEngine;

namespace Equipment.Abilities
{
    public class EquipItemAbility : MonoBehaviour
    {
        public Equipment equipment;
        
        public void Execute(EquipableItem item, Action<EquipmentModule.EquipItemOutput> callback)
        {
            if (!item.TryGetComponent<ContainableItem>(out _)) return;

            var equipItemOutput = EquipmentModule.EquipItem(new EquipmentModule.EquipItemInput
            {
                Item = item,
                Equipment = equipment,
                Slot = item.slot
            });
            
            if (equipItemOutput is not EquipmentModule.EquipItemOutput.EquippedItem equipped) return;

            var itemTransform = item.transform;
            var itemHandle = item.handle;
            var itemHandleLocalRotation = itemHandle.localRotation;
            itemTransform.SetParent(equipment.slotTransformParents[item.slot]);
            itemTransform.localRotation = Quaternion.Inverse(itemHandleLocalRotation);
            itemTransform.localPosition = itemHandleLocalRotation * itemHandle.localPosition;
            item.gameObject.SetActive(true);
            
            callback(equipped);
        }
    }
}