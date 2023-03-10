using System;
using System.Collections;
using Abilities;
using Pathfinding;
using UnityEngine;

namespace Movement.Abilities
{
    public class FollowTargetAbility : Ability
    {
        [Header("Dependencies")]
        public MoveToGameObjectAbility moveToGameObjectAbility;
        
        [Header("Parameters")]
        public GameObject target;
        
        [Header("State")]
        public Action onReachedTarget = delegate {  };
        
        protected override IEnumerator Execute()
        {
            while (gameObject != null && !gameObject.IsNeighbor(target))
            {
                moveToGameObjectAbility.target = target;
                yield return moveToGameObjectAbility.Play();
            }
            onReachedTarget();
        }
    }
}