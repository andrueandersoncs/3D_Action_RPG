using System.Collections;
using UnityEngine;

namespace Abilities
{
    public class TimerAbility : Ability
    {
        [Header("Parameters")]
        public float duration;
        
        protected override IEnumerator Execute()
        {
            yield return new WaitForSeconds(duration);
        }
    }
}