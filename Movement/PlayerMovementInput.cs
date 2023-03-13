using Mouse;
using Movement.Abilities;
using Skills;
using Skills.Abilities;
using UnityEngine;

namespace Movement
{
    public class PlayerMovementInput : MonoBehaviour, IMouseInputConsumer
    {
        public KeyCode haltKey = KeyCode.LeftShift;
        public MoveToDestinationAbility moveToDestinationAbility;
        public UseSkillAbility useSkillAbility;
        
        public bool OnMouseInput(MouseInput mouseInput)
        {
            if (Input.GetKey(haltKey)) return false;
            
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