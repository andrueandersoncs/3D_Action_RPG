using System.Collections;
using Abilities;
using UnityEngine;

namespace Movement.Abilities
{
    public class TurnTowardLocationAbility : Ability
    {
        [Header("Dependencies")]
        public Transform objectToTurn;
        
        [Header("Parameters")]
        public Vector3 location;
        
        protected override IEnumerator Execute()
        {
            objectToTurn.LookAt(location, Vector3.up);
            yield return null;
        }
    }
}