using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class UserInterface : MonoBehaviour
    {
        // character's inventory
        private struct CharacterInventory {}
        
        // character's equipment
        private struct CharacterEquipment {}
        
        // character's stats
        private struct CharacterStats {}
        
        // character's skills
        private struct CharacterSkills {}
        
        // character's quests
        private struct CharacterQuests {}
        
        // character's map
        private struct CharacterMap {}
        
        // character's health
        private struct CharacterHealth {}
        
        // character's mana
        private struct CharacterMana {}
        
        // character's stamina
        private struct CharacterStamina {}
        
        // character's experience
        private struct CharacterExperience {}
        
        // character's open shop
        private struct CharacterOpenShop {}
        
        // character's open bank
        private struct CharacterOpenBank {}

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