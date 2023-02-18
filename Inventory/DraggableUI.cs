using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory
{
    public class DraggableUI : MonoBehaviour
    {
        public static Action<DraggableUI> OnBeginDrag = delegate(DraggableUI ui) {  };
        public static DraggableUI Dragging;
        
        private IUserInterface _userInterface;
        private VisualElement _visualElement;
        private VisualElement _root;
        
        private void Start()
        {
            _userInterface = GetComponent<IUserInterface>();
            _visualElement = _userInterface.GetVisualElement();
            _visualElement.RegisterCallback<MouseDownEvent>(HandleMouseDown);
            
            _root = FindObjectOfType<UIDocument>().rootVisualElement;
            _root.RegisterCallback<MouseMoveEvent>(HandleMouseMove);

            void HandleMouseMove(MouseMoveEvent evt)
            {
                if (Dragging != this) return;
                _visualElement.style.left = evt.mousePosition.x + 5;
                _visualElement.style.top = evt.mousePosition.y + 5;
            }
            
            void HandleMouseDown(MouseDownEvent evt)
            {
                if (Dragging != null) return;
                _visualElement.style.position = Position.Absolute;
                _visualElement.style.left = evt.mousePosition.x + 5;
                _visualElement.style.top = evt.mousePosition.y + 5;
                _root.Add(_visualElement);
                Dragging = this;
                OnBeginDrag(this);
                evt.StopPropagation();
            }
        }

        public void DropInto(VisualElement container)
        {
            DropInto(container, Vector2.zero);
        }
        
        public void DropInto(VisualElement container, Vector2 position)
        {
            if (Dragging != this) return;
            Dragging = null;
            // _visualElement.style.position = Position.Relative;
            _visualElement.style.left = position.x;
            _visualElement.style.top = position.y;
            container.Add(_visualElement);
        }
    }
}