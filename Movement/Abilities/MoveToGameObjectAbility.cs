using System.Collections;
using Abilities;
using UnityEngine;

namespace Movement.Abilities
{
    public class MoveToGameObjectAbility : Ability
    {
        public MoveToDestinationAbility moveToDestinationAbility;
        public GameObject target;

        protected override IEnumerator Execute()
        {
            moveToDestinationAbility.destination = target.transform.position;
            yield return moveToDestinationAbility.Play();
        }

        public override void Stop()
        {
            base.Stop();
            moveToDestinationAbility.Stop();
        }
    }
}