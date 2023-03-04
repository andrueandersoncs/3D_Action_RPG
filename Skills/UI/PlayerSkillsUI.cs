using System.Collections;
using System.Linq;
using DefaultNamespace;
using Keyboard;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Skills
{
    public class PlayerSkillsUI : MonoBehaviour
    {
        public Skills skills;
        public SpendSkillPointAbility spendSkillPointAbility;
        public VisualTreeAsset skillListItemTemplate;
        
        private void OnEnable()
        {
            var uiDocument = FindObjectOfType<UIDocument>();
            var root = uiDocument.rootVisualElement;
            var visualElement = root.Q("Skills");
            
            var toggle = root.Q<Toggle>("SkillsToggle");
            Keybindings.Bind(KeyCode.K, () => toggle.value = !toggle.value);
            
            toggle.ObserveEveryValueChanged(i => i.value)
                .Subscribe(v => UserInterface.Toggle(visualElement, v))
                .AddTo(this);
            
            var skillPointsText = root.Q<Label>("SkillPoints");
            skills.skillPoints
                .Subscribe(p => skillPointsText.text = p.ToString())
                .AddTo(this);
            
            // allow me to see the skills available = populate the skills list with clickable buttons
            // click button to allocate the skill point into that skill
            var skillList = root.Q<ListView>("SkillList");
            skillList.itemsSource = skills.availableSkills;

            skillList.makeItem = () => skillListItemTemplate.Instantiate();
            skillList.bindItem = (el, index) =>
            {
                var skill = skills.availableSkills[index];
                
                var button = (Button) el.Children().First();

                IEnumerator OnClick()
                {
                    spendSkillPointAbility.skillScriptableObject = skill;
                    yield return spendSkillPointAbility.Play();
                    skillList.RefreshItem(index);
                }

                button.clickable.clicked += () =>
                {
                    StopAllCoroutines();
                    StartCoroutine(OnClick());
                };
                
                var icon = button.Children().First();
                icon.style.backgroundImage = skill.icon;
                
                var textContainer = button.Children().Last();
                
                var nameContainer = textContainer.Children().First();
                var nameLabel = (Label) nameContainer.Children().Last();
                nameLabel.text = skill.skillName;

                var descriptionContainer = textContainer.Children().ElementAt(1);
                var descriptionLabel = (Label) descriptionContainer.Children().Last();
                descriptionLabel.text = skill.description;
                
                var requiredLevelContainer = textContainer.Children().ElementAt(2);
                var requiredLevelLabel = (Label) requiredLevelContainer.Children().Last();
                requiredLevelLabel.text = skill.levelRequirement.ToString();
                
                var skillLevelContainer = textContainer.Children().ElementAt(3);
                var skillLevelLabel = (Label) skillLevelContainer.Children().Last();
                skillLevelLabel.text = skills.skillPointAllocations.Count(s => s == skill).ToString();
            };
        }
    }
}