using UniRx;
using UnityEngine;

namespace Inventory
{
    public class ItemContainer : MonoBehaviour
    {
        public Vector2Int dimensions;
        public ReactiveDictionary<Vector2Int, ContainableItem> items = new();
        public ContainableItem[] initialItems;
        
        private void OnEnable()
        {
            foreach (var item in initialItems)
            {
                ItemContainerModule.AddItem(new ItemContainerModule.AddItemInput
                {
                    ContainableItem = item,
                    ItemContainer = this
                });
            }
        }
    }
}