using System.Collections;
using UnityEngine;

namespace Abilities
{
    public class InstantiateAbility : Ability
    {
        public GameObject prefab;
        
        protected override IEnumerator Execute()
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
            yield break;
        }
    }
}