using System;
using UniRx;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public struct KVP<K, V>
    {
        public K Key;
        public V Value;
    }
    
    public class Equipment : MonoBehaviour
    {
        public KVP<EquipmentSlot, EquipableItem>[] initialItems;
        public ReactiveDictionary<EquipmentSlot, EquipableItem> items = new();

        public KVP<EquipmentSlot, Transform>[] initialSlotTransformParents;
        public ReactiveDictionary<EquipmentSlot, Transform> slotTransformParents = new();
        
        private void Start()
        {
            foreach (var kvp in initialItems)
            {
                items.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in initialSlotTransformParents)
            {
                slotTransformParents.Add(kvp.Key, kvp.Value);
            }
        }
    }
}