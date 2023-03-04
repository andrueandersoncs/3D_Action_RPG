using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Keyboard;
using UnityEngine;
using UnityEngine.UIElements;

namespace Skills
{
    public class BindSkillHotkeyAbility : Ability
    {
        [Header("Dependencies")]
        public new Camera camera;
        public UseSkillAbility useSkillAbility;
        
        [Header("Parameters")]
        public SkillScriptableObject skill;
        public KeyCode hotkey;
        
        // State
        private RaycastHit[] raycastHits = new RaycastHit[10];
        private LayerMask terrainMask;
        private Dictionary<KeyCode, IDisposable> bindings = new();
        private UIDocument uiDocument;
        private VisualElement root;
        private VisualElement skillBar;

        private void OnEnable()
        {
            terrainMask = LayerMask.GetMask("Terrain");
            
            uiDocument = FindObjectOfType<UIDocument>();
            root = uiDocument.rootVisualElement;
            skillBar = root.Q("SkillBar");
        }

        protected override IEnumerator Execute()
        {
            if (bindings.TryGetValue(hotkey, out var existingBinding))
            {
                existingBinding.Dispose();
                bindings.Remove(hotkey);
            }
            
            var localSkill = Instantiate(skill);
            var binding = Keybindings.Bind(hotkey, () =>
            {
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                var numHits = Physics.RaycastNonAlloc(ray, raycastHits, 50f, terrainMask); 
                if (numHits == 0) return;
                useSkillAbility.skillScriptableObject = localSkill;
                useSkillAbility.target = raycastHits[0].point;
                useSkillAbility.Play();
            });
            
            bindings.Add(hotkey, binding);

            switch (hotkey)
            {
                case KeyCode.Q:
                {
                    var actionBarButton = (Button) skillBar.Children().ElementAt(0);
                    actionBarButton.style.backgroundImage = skill.icon;
                    break;
                }
                case KeyCode.W:
                {
                    var actionBarButton = (Button) skillBar.Children().ElementAt(1);
                    actionBarButton.style.backgroundImage = skill.icon;
                    break;
                }
                case KeyCode.E:
                {
                    var actionBarButton = (Button) skillBar.Children().ElementAt(2);
                    actionBarButton.style.backgroundImage = skill.icon;
                    break;
                }
                case KeyCode.R:
                {
                    var actionBarButton = (Button) skillBar.Children().ElementAt(3);
                    actionBarButton.style.backgroundImage = skill.icon;
                    break;
                }
            }
            
            yield break;
        }
    }
}