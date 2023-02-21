using System.Collections;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] private bool running = false;
        
        protected bool disabled;
        
        protected abstract IEnumerator Execute();
        
        public Coroutine Play()
        {
            if (disabled) return null;
            
            Stop();
            
            running = true;
            
            return StartCoroutine(Execute());
        }

        private IEnumerator AndThen(Ability ability)
        {
            yield return Execute();
            yield return ability.Execute();
        }

        public Coroutine PlayAndThen(Ability ability)
        {
            if (disabled) return null;
            if (ability.disabled) return null;

            Stop();
            ability.Stop();
            
            return StartCoroutine(AndThen(ability));   
        }

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

        public virtual void Fail()
        {
            Stop();
        }
    }
}