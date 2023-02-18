using Inventory;
using Mouse;
using Player.Abilities;
using UnityEngine;

namespace Player
{
    public class PlayerOpenItemContainerInput : MonoBehaviour, IMouseInputConsumer
    {
        public OpenItemContainerAbility openItemContainerAbility;

        public bool OnMouseInput(MouseInput mouseInput)
        {
            foreach (var hit in mouseInput.hits)
            {
                if (!hit.collider.TryGetComponent<ItemContainer>(out var itemContainer)) continue;
                openItemContainerAbility.itemContainer = itemContainer;
                openItemContainerAbility.Play();
                return true;
            }

            return false;
        }
    }
}