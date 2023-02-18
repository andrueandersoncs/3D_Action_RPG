using System.Collections;
using Abilities;
using Movement.Abilities;
using UnityEngine;

namespace AI.Abilities
{
    public class RoamAbility : Ability
    {
        public MoveToDestinationAbility moveToDestinationAbility;
        public Vector3 origin;

        protected override IEnumerator Execute()
        {
            while (true)
            {
                var destination = origin + Random.insideUnitSphere * 10f;
                moveToDestinationAbility.destination = destination;
                yield return moveToDestinationAbility.Play();
                yield return new WaitForSeconds(Random.Range(3f, 10f));    
            }
        }
        
        public override void Stop()
        {
            base.Stop();
            // moveToDestinationAbility.Stop();
        }
    }
}