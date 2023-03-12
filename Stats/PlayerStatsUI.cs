using DefaultNamespace;
using Keyboard;
using UniRx;
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
            var statsVisualElement = root.Q("Stats");
            var statsToggle = root.Q<Toggle>("StatsToggle");
            
            statsToggle.ObserveEveryValueChanged(i => i.value)
                .Subscribe(v => UserInterface.UserInterfaceUtils.Toggle(statsVisualElement, v))
                .AddTo(this);
            
            Keybindings.Bind(KeyCode.S, () => statsToggle.value = !statsToggle.value);
        }
    }
}