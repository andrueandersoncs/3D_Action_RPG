using System;
using System.Collections;
using Abilities;
using UnityEngine;

namespace Movement.Abilities
{
    public class FollowTargetAbility : Ability
    {
        public MoveToDestinationAbility moveToDestinationAbility;
        public GameObject target;
        public Action onReachedTarget = delegate {  };
        
        protected override IEnumerator Execute()
        {
            while (true)
            {
                moveToDestinationAbility.destination = target.transform.position;
                
                yield return moveToDestinationAbility.Play();
                
                if (Vector3.Distance(target.transform.position, transform.position) > 0.1f)
                    continue;

                onReachedTarget();
            }
        }
    }
}