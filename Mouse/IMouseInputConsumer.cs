using System;
using UnityEngine;

namespace Mouse
{
    public struct MouseInput
    {
        public RaycastHit[] hits;
    }
    
    public interface IMouseInputConsumer
    {
        public bool OnMouseInput(MouseInput mouseInput);
    }
}