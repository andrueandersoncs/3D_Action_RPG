using System.Collections;
using Abilities;
using UnityEngine;

namespace Inventory.Abilities
{
    public class DropItemAbility : Ability
    {
        public ItemContainer itemContainer;
        public ContainableItem item;
        
        protected override IEnumerator Execute()
        {
            var removeItemOutput = ItemContainerModule.RemoveItem(new ItemContainerModule.RemoveItemInput()
            {
                ContainableItem = item,
                ItemContainer = itemContainer
            });

            if (removeItemOutput is not ItemContainerModule.RemoveItemOutput.RemovedItem removed)
                yield break;
            
            var dropPosition = transform.position + Random.onUnitSphere * 2f;
            dropPosition.y = transform.position.y;
            
            removed.Item.transform.SetParent(null);
            removed.Item.transform.position = dropPosition;
            removed.Item.gameObject.SetActive(true);
        }
    }
}