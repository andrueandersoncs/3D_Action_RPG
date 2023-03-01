using System;
using UnityEngine;
using UnityEngine.Events;

namespace Callbacks
{
    public class OnUnityTriggerEnter : MonoBehaviour
    {
        public UnityEvent<Collider> onTriggerEnterCollider;
        public UnityEvent onTriggerEnter;
        
        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnterCollider.Invoke(other);
            onTriggerEnter.Invoke();
        }
    }
}