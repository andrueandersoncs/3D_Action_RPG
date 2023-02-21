using Mouse;
using Movement.Abilities;
using UnityEngine;

namespace Movement
{
    public class PlayerMovementInput : MonoBehaviour, IMouseInputConsumer
    {
        public MoveToDestinationAbility moveToDestinationAbility;
        
        public bool OnMouseInput(MouseInput mouseInput)
        {
            foreach (var hit in mouseInput.hits)
            {
                moveToDestinationAbility.destination = hit.point;
                moveToDestinationAbility.Play();
                return true;
            }

            return false;
        }
    }
}