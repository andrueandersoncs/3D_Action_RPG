using System;
using System.Collections;
using Abilities;
using Pathfinding;
using Stats;
using UnityEngine;

namespace Movement.Abilities
{
    public class MoveToDestinationAbility : Ability
    {
        [Header("Dependencies")]
        public Pathfinder pathfinder;
        public ActionStats actionStats;
        public Animator animator;
        public TurnTowardLocationAbility turnTowardLocationAbility;

        [Header("Parameters")]
        public Vector3 destination;
        
        [Header("State")]
        public Action onFailedToFindPath = delegate {  };
        public float baseSpeed = 5f;
        
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
        
        public override void Stop()
        {
            base.Stop();
            animator.SetFloat(MoveSpeed, 0f);
        }
    }
}