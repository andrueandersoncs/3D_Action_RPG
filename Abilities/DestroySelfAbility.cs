using System.Collections;
using UnityEngine;

namespace Abilities
{
    public class DestroySelfAbility : Ability
    {
        protected override IEnumerator Execute()
        {
            Debug.Log("Destroying self!");
            Destroy(gameObject);
            yield break;
        }
    }
}