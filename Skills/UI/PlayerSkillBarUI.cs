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