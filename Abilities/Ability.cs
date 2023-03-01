using System.Collections;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        public Ability[] nextAbilities;
        public bool successfullyExecuted = true;
        
        [SerializeField] private bool running = false;

        private bool disabled;
        
        protected abstract IEnumerator Execute();

        private IEnumerator ExecuteAll()
        {
            // Debug.Log("Executing all..");
            
            yield return Execute();
            
            // Debug.Log("Finished Execute");
            
            if (!successfullyExecuted)
            {
                Debug.Log("Didn't successfully execute:" + name);
                yield break;
            }
            
            foreach (var nextAbility in nextAbilities)
            {
                Debug.Log("Playing next ability:" + nextAbility.name);
                yield return nextAbility.Execute();
                if (!nextAbility.successfullyExecuted)
                {
                    Debug.Log("Next ability did not successfully execute!");
                    yield break;
                }
            }
        }
        
        public Coroutine Play()
        {
            if (disabled)
            {
                // Debug.Log("Play disabled");
                return null;
            }
            Stop();
            running = true;
            return StartCoroutine(Execute());
        }

        public Coroutine PlayAll()
        {
            if (disabled) return null;
            Stop();
            running = true;
            return StartCoroutine(ExecuteAll());   
        }
        
        public void FireAndForget() => Play();
        public void FireAndForgetAll() => StartCoroutine(ExecuteAll());

        public virtual void Stop()
        {
            StopAllCoroutines();
            running = false;
        }

        public void Disable()
        {
            Stop();
            disabled = true;
        }

        public void Enable() => disabled = false;
    }
}