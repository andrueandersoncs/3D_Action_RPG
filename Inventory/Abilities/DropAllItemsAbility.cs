using System.Collections;
using System.Linq;
using Abilities;

namespace Inventory.Abilities
{
    public class DropAllItemsAbility : Ability
    {
        public ItemContainer itemContainer;
        public DropItemAbility dropItemAbility;

        protected override IEnumerator Execute()
        {
            var items = itemContainer.items.Values.ToList();
            foreach (var item in items)
            {
                dropItemAbility.item = item;
                yield return dropItemAbility.Play();
            }
        }
    }
}