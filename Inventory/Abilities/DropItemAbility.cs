using System.Collections;
using Abilities;
using Movement;
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

            var pos = transform.position;
            var dropPosition = pos + PositioningExtensions.StandardNeighboringPositions
                [Mathf.FloorToInt(Random.value * PositioningExtensions.StandardNeighboringPositions.Length)];

            removed.Item.transform.SetParent(null);
            removed.Item.transform.position = dropPosition.RoundToPosition();
            removed.Item.gameObject.SetActive(true);
        }
    }
}