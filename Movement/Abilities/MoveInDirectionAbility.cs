using System.Collections;
using Abilities;
using UnityEngine;

namespace Movement.Abilities
{
    public class MoveInDirectionAbility : Ability
    {
        public Vector3 direction;
        public float speed;
        
        protected override IEnumerator Execute()
        {
            while (true)
            {
                transform.Translate(direction * (Time.deltaTime * speed));
            }
        }
    }
}