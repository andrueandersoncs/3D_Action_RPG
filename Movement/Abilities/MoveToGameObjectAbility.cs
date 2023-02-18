using System.Collections;
using Abilities;
using UnityEngine;

namespace Movement.Abilities
{
    public class MoveToGameObjectAbility : Ability
    {
        public MoveToDestinationAbility moveToDestinationAbility;
        public GameObject target;
        public float stoppingDistance = 2f;

        protected override IEnumerator Execute()
        {
            Vector3 destination;
            
            // Added a while here to account for a moving target
            do
            {
                destination = target.transform.position;
                moveToDestinationAbility.destination = destination;
                moveToDestinationAbility.stoppingDistance = stoppingDistance;
                yield return moveToDestinationAbility.Play();
            } while (target.transform.position != destination);
        }

        public override void Stop()
        {
            base.Stop();
            moveToDestinationAbility.Stop();
        }
    }
}