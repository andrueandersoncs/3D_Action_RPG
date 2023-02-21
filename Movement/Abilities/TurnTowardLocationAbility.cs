using System.Collections;
using Abilities;
using UnityEngine;

namespace Movement.Abilities
{
    public class TurnTowardLocationAbility : Ability
    {
        public Transform objectToTurn;
        public Vector3 location;
        
        protected override IEnumerator Execute()
        {
            objectToTurn.LookAt(location, Vector3.up);
            yield return null;
        }
    }
}