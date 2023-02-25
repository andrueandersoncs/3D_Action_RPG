using DefaultNamespace;
using Keyboard;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Skills
{
    public class PlayerSkillsUI : MonoBehaviour
    {
        private void OnEnable()
        {
            var uiDocument = FindObjectOfType<UIDocument>();
            var root = uiDocument.rootVisualElement;
            var visualElement = root.Q("Skills");
            var toggle = root.Q<Toggle>("SkillsToggle");
            
            toggle.ObserveEveryValueChanged(i => i.value)
                .Subscribe(v => UserInterface.Toggle(visualElement, v))
                .AddTo(this);
            
            Keybindings.Bind(KeyCode.S, () => toggle.value = !toggle.value);
        }
    }
}