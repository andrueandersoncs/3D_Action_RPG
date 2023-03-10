using System;
using System.Collections;
using Abilities;
using AI;
using Pathfinding;
using Stats;
using UnityEngine;

namespace Movement.Abilities
{
    public class MoveToDestinationAbility : Ability
    {
        public float baseSpeed = 5f;
        public Pathfinder pathfinder;
        public ActionStats actionStats;
        public Animator animator;
        public TurnTowardLocationAbility turnTowardLocationAbility;

        public Vector3 destination;
        public Action onFailedToFindPath = delegate {  };
        
        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
        private float speed => baseSpeed * (1f + actionStats.FasterRunWalk / 100f);
        
        protected override IEnumerator Execute()
        {
            var movementReceiver = transform;
            
            var path = pathfinder.FindPath(movementReceiver.position, destination);
            
            if (path == null)
            {
                animator.SetFloat(MoveSpeed, 0f);
                onFailedToFindPath();
                yield break;
            }
            
            foreach (var point in path)
            {
                while (movementReceiver.position != point)
                {
                    if (pathfinder.IsPositionBlocked(point))
                    {
                        yield return Execute();
                        yield break;
                    }
                
                    var transformPosition = movementReceiver.position;
                
                    // Otherwise, move toward the point
                    var target = new Vector3(point.x, transformPosition.y, point.z);
                    
                    turnTowardLocationAbility.location = target;
                    yield return turnTowardLocationAbility.Play();
                    
                    var nextPosition = Vector3.MoveTowards(transformPosition, target, speed * Time.deltaTime);
                    movementReceiver.position = nextPosition;

                    animator.SetFloat(MoveSpeed, speed);

                    yield return null;
                }
            }

            animator.SetFloat(MoveSpeed, 0f);
        }
    }
}