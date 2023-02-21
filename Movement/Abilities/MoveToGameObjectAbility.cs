using System.Collections;
using Abilities;
using UnityEngine;

namespace Movement.Abilities
{
    public class MoveToGameObjectAbility : Ability
    {
        public MoveToDestinationAbility moveToDestinationAbility;
        public GameObject target;
        public float stoppingDistance = 0.5f;

        protected override IEnumerator Execute()
        {
            // Added a while here to account for a moving target
            while (Vector3.Distance(transform.position, target.transform.position) > stoppingDistance)
            {
                moveToDestinationAbility.destination = target.transform.position;
                moveToDestinationAbility.stoppingDistance = stoppingDistance;
                moveToDestinationAbility.Play();
                yield return new WaitForSeconds(0.5f);
            }
        }

        public override void Stop()
        {
            base.Stop();
            moveToDestinationAbility.Stop();
        }
    }
}