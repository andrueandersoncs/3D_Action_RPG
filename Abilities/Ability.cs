using System.Collections;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        public Ability[] nextAbilities;
        public bool successfullyExecuted = true;
        
        protected abstract IEnumerator Execute();

        public Coroutine Play()
        {
            Stop();
            return StartCoroutine(Execute());
        }

        public Coroutine PlayAll()
        {
            Stop();

            IEnumerator DoPlayAll()
            {
                yield return Execute();
                if (!successfullyExecuted) yield break;
                
                foreach (var ability in nextAbilities)
                {
                    yield return ability.Play();
                    successfullyExecuted = ability.successfullyExecuted;
                    if (!successfullyExecuted) yield break;
                }
            }

            return StartCoroutine(DoPlayAll());
        }
        
        public void FireAndForget() => Play();
        public void FireAndForgetAll() => PlayAll();

        public virtual void Stop() => StopAllCoroutines();
    }
}