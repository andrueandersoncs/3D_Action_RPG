using System;
using System.Collections;
using Abilities;
using AI;
using Stats;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Movement.Abilities
{
    public class MoveToDestinationAbility : Ability
    {
        public Pathfinder pathfinder;
        public ActionStats actionStats;
        public Animator animator;

        public Vector3 destination;
        public float stoppingDistance;
        
        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

        private void OnEnable()
        {
            gameObject
                .UpdateAsObservable()
                .Subscribe(_ => animator.SetFloat(MoveSpeed, pathfinder.velocity.magnitude))
                .AddTo(this);
        }

        protected override IEnumerator Execute()
        {
            pathfinder.speed = 3.5f * (1f + actionStats.FasterRunWalk / 100f);
            var path = pathfinder.SetDestination(destination);
            yield return pathfinder.WalkPath(path);
        }
    }
}