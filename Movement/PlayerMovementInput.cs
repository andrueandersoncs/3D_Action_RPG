using Mouse;
using Movement.Abilities;
using Skills;
using Skills.Abilities;
using UnityEngine;

namespace Movement
{
    public class PlayerMovementInput : MonoBehaviour, IMouseInputConsumer
    {
        public MoveToDestinationAbility moveToDestinationAbility;
        public UseSkillAbility useSkillAbility;
        
        public bool OnMouseInput(MouseInput mouseInput)
        {
            foreach (var hit in mouseInput.hits)
            {
                if (useSkillAbility != null) useSkillAbility.Stop();
                moveToDestinationAbility.destination = hit.point;
                moveToDestinationAbility.Play();
                return true;
            }

            return false;
        }
    }
}