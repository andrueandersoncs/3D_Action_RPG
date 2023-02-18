using DefaultNamespace;
using Keyboard;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stats
{
    public class PlayerStatsUI : MonoBehaviour
    {
        private void OnEnable()
        {
            var uiDocument = FindObjectOfType<UIDocument>();
            var root = uiDocument.rootVisualElement;
        
            // UI Toggle keybinding:
            var statsVisualElement = root.Q("Stats");
            Keybindings.Bind(KeyCode.S, () => UserInterface.Toggle(statsVisualElement));
        }
    }
}