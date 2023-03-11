using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace Skills.UI
{
    public class PlayerSkillBarUI : MonoBehaviour
    {
        [Header("Dependencies")]
        public Skills skills;
        public BindSkillHotkeyAbility bindSkillHotkeyAbility;
        
        [Header("Parameters")]
        public VisualTreeAsset skillIconHoverPopupTemplate;
        
        private static readonly KeyCode[] Hotkeys = {
            KeyCode.Q,
            KeyCode.W,
            KeyCode.E,
            KeyCode.R
        };

        private void OnEnable()
        {
            var uiDocument = FindObjectOfType<UIDocument>();
            var root = uiDocument.rootVisualElement;
            
            // when I click any of the action bar items, pop a popover with a grid of skills
            // clicking a skill from the grid assigns it to the action bar item
            var skillBar = root.Q("SkillBar");
            var skillGridPopover = root.Q("SkillGridPopover");
            var skillIconHoverPopupTemplateContainer = skillIconHoverPopupTemplate.Instantiate();
            var skillIconHoverPopup = skillIconHoverPopupTemplateContainer.contentContainer.Children().ElementAt(0);
            root.Add(skillIconHoverPopup);
                
            for (var i = 0; i < 4; i++)
            {
                var keycode = Hotkeys[i];
                var actionBarButton = (Button) skillBar.Children().ElementAt(i);
                actionBarButton.clickable.clicked += () =>
                {
                    // Clear the grid, populate with currently allocated skills
                    skillGridPopover.Clear();

                    foreach (var skill in skills.DistinctSkills)
                    {
                        var skillItem = new Button
                        {
                            style =
                            {
                                backgroundImage = skill.icon,
                                width = 50,
                                height = 50
                            }
                        };
                        
                        skillItem.RegisterCallback<MouseEnterEvent, VisualElement>((evt, visualElement) =>
                        {
                            // Fill the popover with the skill's description
                            visualElement.Clear();
                            visualElement.Add(new Label(skill.skillName));
                            visualElement.Add(new Label(skill.description));
                            
                            // Display the popover
                            visualElement.style.display = DisplayStyle.Flex;
                            visualElement.style.left = evt.mousePosition.x + 5;
                            visualElement.style.top = evt.mousePosition.y + 5;
                        }, skillIconHoverPopup);
            
                        skillItem.RegisterCallback<MouseMoveEvent, VisualElement>((evt, visualElement) =>
                        {
                            visualElement.style.left = evt.mousePosition.x + 5;
                            visualElement.style.top = evt.mousePosition.y + 5;
                        }, skillIconHoverPopup);
            
                        skillItem.RegisterCallback<MouseLeaveEvent, VisualElement>((_, visualElement) =>
                        {
                            visualElement.style.display = DisplayStyle.None;
                        }, skillIconHoverPopup);
                        
                        // When I click the skill in the popover, assign it to the action bar item
                        skillItem.clickable.clicked += () =>
                        {
                            bindSkillHotkeyAbility.skill = skill;
                            bindSkillHotkeyAbility.hotkey = keycode;
                            bindSkillHotkeyAbility.Play();
                            UserInterface.Toggle(skillGridPopover, false);
                        };
                        
                        skillGridPopover.Add(skillItem);
                    }
                    
                    UserInterface.Toggle(skillGridPopover, true);
                };
            }
        }

        // pressing the action bar item's hotkey executes the skill
        // visual feedback on the action bar item when I press the associated hotkey?
    }
}