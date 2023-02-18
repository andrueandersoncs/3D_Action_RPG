using Stats;
using UniRx;
using UnityEngine;

namespace Equipment
{
    public abstract class BaseEquipmentStatsModifier<T> : MonoBehaviour where T : IStats<T>
    {
        private void OnEnable()
        {
            var equipment = GetComponent<Equipment>();
            var stats = GetComponent<T>();

            equipment
                .items
                .ObserveAdd()
                .Subscribe(addEvent =>
                {
                    var item = addEvent.Value;
                    var itemStats = item.GetComponent<T>();
                    if (itemStats == null) return;
                    stats.Add(itemStats);
                });
            
            equipment
                .items
                .ObserveRemove()
                .Subscribe(removeEvent =>
                {
                    var item = removeEvent.Value;
                    var itemStats = item.GetComponent<T>();
                    if (itemStats == null) return;
                    stats.Subtract(itemStats);
                });
        }
    }
}