using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class UserInterface : MonoBehaviour
    {
        private static VisualElement _root;

        public static VisualElement Root
        {
            get
            {
                if (_root != null) return _root;
                
                var uiDocument = FindObjectOfType<UIDocument>();
                _root = uiDocument.rootVisualElement;
                
                return _root;
            }
        }
        
        public static void Toggle(VisualElement v)
        {
            v.style.display = v.style.display != DisplayStyle.None
                ? new StyleEnum<DisplayStyle>(DisplayStyle.None)
                : new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }
        
        public static void Toggle(VisualElement v, bool show)
        {
            v.style.display = show
                ? new StyleEnum<DisplayStyle>(DisplayStyle.Flex)
                : new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        public static VisualElement CreateHoverPopup(VisualElement hoverTarget)
        {
            var visualElement = new VisualElement
            {
                style =
                {
                    display = DisplayStyle.None,
                    position = Position.Absolute,
                    backgroundColor = new Color(0.2666f, 0.2666f, 0.2666f),
                    color = Color.white,
                    borderBottomWidth = 1,
                    borderBottomColor = Color.white,
                    borderLeftWidth = 1,
                    borderLeftColor = Color.white,
                    borderRightWidth = 1,
                    borderRightColor = Color.white,
                    borderTopWidth = 1,
                    borderTopColor = Color.white,
                }
            };

            hoverTarget.RegisterCallback<MouseEnterEvent, VisualElement>((evt, visualElement) =>
            {
                visualElement.style.display = DisplayStyle.Flex;
                visualElement.style.left = evt.mousePosition.x + 5;
                visualElement.style.top = evt.mousePosition.y + 5;
            }, visualElement);
            
            hoverTarget.RegisterCallback<MouseMoveEvent, VisualElement>((evt, visualElement) =>
            {
                visualElement.style.left = evt.mousePosition.x + 5;
                visualElement.style.top = evt.mousePosition.y + 5;
            }, visualElement);
            
            hoverTarget.RegisterCallback<MouseLeaveEvent, VisualElement>((_, visualElement) =>
            {
                visualElement.style.display = DisplayStyle.None;
            }, visualElement);

            return visualElement;
        }
    }
}