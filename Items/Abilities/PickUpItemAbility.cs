using System.Collections;
using Abilities;
using Inventory;
using Movement.Abilities;
using UnityEngine;

namespace Items
{
    public class PickUpItemAbility : Ability
    {
        public MoveToGameObjectAbility moveToGameObjectAbility;
        public ItemContainer itemContainer;
        public ContainableItem item;
        
        protected override IEnumerator Execute()
        {
            moveToGameObjectAbility.target = item.gameObject;
            yield return moveToGameObjectAbility.Play();

            var addItemOutput = ItemContainerModule.AddItem(new ItemContainerModule.AddItemInput()
            {
                ContainableItem = item,
                ItemContainer = itemContainer
            });

            if (addItemOutput is not ItemContainerModule.AddItemOutput.ItemAdded added) yield break;
            
            added.Item.transform.SetParent(itemContainer.transform);
            added.Item.gameObject.SetActive(false);
        }
        
        public override void Stop()
        {
            base.Stop();
            moveToGameObjectAbility.Stop();
        }
    }
}