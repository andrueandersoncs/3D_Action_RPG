using AI;
using Mouse;
using Movement.Abilities;
using Unity.AI.Navigation;
using UnityEngine;

namespace Movement
{
    public class PlayerMovementInput : MonoBehaviour, IMouseInputConsumer
    {
        public MoveToDestinationAbility moveToDestinationAbility;
        // public Pathfinder pathfinder;
        
        public bool OnMouseInput(MouseInput mouseInput)
        {
            foreach (var hit in mouseInput.hits)
            {
                var navMeshSurface = hit.collider.GetComponent<NavMeshSurface>();
                if (navMeshSurface == null) continue;
                // moveToDestinationAbility.stoppingDistance = 0f;
                moveToDestinationAbility.destination = hit.point;
                moveToDestinationAbility.Play();
                // pathfinder.SetDestination(hit.point);
                return true;
            }

            return false;
        }
    }
}