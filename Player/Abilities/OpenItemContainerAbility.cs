using System.Collections;
using Abilities;
using Inventory;
using Inventory.Abilities;
using Movement.Abilities;

namespace Player.Abilities
{
    public class OpenItemContainerAbility : Ability
    {
        public MoveToGameObjectAbility moveToGameObjectAbility;
        public ItemContainer itemContainer;

        protected override IEnumerator Execute()
        {
            if (!itemContainer.TryGetComponent<DropAllItemsAbility>(out var dropAllItems)) yield break;
            moveToGameObjectAbility.target = itemContainer.gameObject;
            yield return moveToGameObjectAbility.Play();
            dropAllItems.Play();
        }
        
        public override void Stop()
        {
            base.Stop();
            moveToGameObjectAbility.Stop();
        }
    }
}