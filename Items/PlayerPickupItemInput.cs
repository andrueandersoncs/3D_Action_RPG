using Abilities;
using Inventory;
using Mouse;
using UnityEngine;

namespace Items
{
    public class PlayerPickupItemInput : MonoBehaviour, IMouseInputConsumer
    {
        public PickUpItemAbility pickUpItemAbility;

        public bool OnMouseInput(MouseInput mouseInput)
        {
            foreach (var hit in mouseInput.hits)
            {
                if (!hit.collider.TryGetComponent<ContainableItem>(out var containableItem)) continue;
                pickUpItemAbility.item = containableItem;
                pickUpItemAbility.Play();
                return true;
            }

            return false;
        }
    }
}