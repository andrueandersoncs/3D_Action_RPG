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
    }
}