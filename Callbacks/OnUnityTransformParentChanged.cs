using UnityEngine;
using UnityEngine.Events;

namespace Callbacks
{
    public class OnUnityTransformParentChanged : MonoBehaviour
    {
        public UnityEvent onTransformParentChanged;
        
        private void OnTransformParentChanged()
        {
            onTransformParentChanged.Invoke();
        }
    }
}