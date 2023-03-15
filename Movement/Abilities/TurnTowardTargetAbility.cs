using System.Collections;
using Abilities;
using UnityEngine;

namespace Movement.Abilities
{
    public class TurnTowardTargetAbility : Ability
    {
        [Header("Dependencies")]
        public Transform objectToTurn;
        
        [Header("Parameters")]
        public GameObject target;
        
        protected override IEnumerator Execute()
        {
            objectToTurn.LookAt(target.transform, Vector3.up);
            yield return null;
        }
    }
}