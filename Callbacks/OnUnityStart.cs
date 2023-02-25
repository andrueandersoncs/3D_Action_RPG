using UnityEngine;
using UnityEngine.Events;

namespace Callbacks
{
    public class OnUnityStart : MonoBehaviour
    {
        public UnityEvent onUnityStart;
        
        private void Start()
        {
            onUnityStart.Invoke();
        }
    }
}