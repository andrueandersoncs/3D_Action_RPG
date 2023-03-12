using System.Linq;
using Combat;
using Stats.Vitals;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UserInterface;

namespace Mouse
{
    public class MouseTargetUI : MonoBehaviour
    {
        public PlayerMouseInput playerMouseInput;
        public CombatGroup combatGroup;
        
        private void OnEnable()
        {
            var targetIndicatorContainer = UserInterfaceUtils.Root.Q("TargetIndicatorContainer");
            var targetHealthBar = UserInterfaceUtils.Root.Q("TargetHealthBar");
            var targetNameLabel = UserInterfaceUtils.Root.Q<Label>("TargetNameLabel");
            var targetDetailsLabel = UserInterfaceUtils.Root.Q<Label>("TargetDetailsLabel");

            playerMouseInput
                .hitsSubject
                .Select(hits => hits.FirstOrDefault(hit => hit.collider.GetComponent<VitalStats>()))
                .Subscribe(hit =>
                {
                    var hasCollider = hit.collider != null;
                    if (!hasCollider)
                    {
                        UserInterfaceUtils.Toggle(targetIndicatorContainer, false);
                        return;
                    }
                    
                    var hasVitalStats = hit.collider.TryGetComponent<VitalStats>(out var vitalStats);
                    if (!hasVitalStats || vitalStats.Health <= 0f)
                    {
                        UserInterfaceUtils.Toggle(targetIndicatorContainer, false);
                        return;
                    }
                    
                    var hasCombatGroup = hit.collider.TryGetComponent<CombatGroup>(out var colliderCombatGroup);
                    if (!hasCombatGroup)
                    {
                        UserInterfaceUtils.Toggle(targetIndicatorContainer, false);
                        return;
                    }
                    
                    var isFriendly = !combatGroup.CanDamage(colliderCombatGroup);
                    if (isFriendly)
                    {
                        UserInterfaceUtils.Toggle(targetIndicatorContainer, false);
                        return;
                    }
                    
                    var health = vitalStats.Health;
                    var maxHealth = vitalStats.MaxHealth;
                    var colliderName = hit.collider.name;
                    var details = $"{colliderName} - {health.ToString()} / {maxHealth.ToString()}";

                    targetHealthBar.style.width = Length.Percent(health / maxHealth * 100f);
                    targetNameLabel.text = colliderName;
                    targetDetailsLabel.text = details;
                    UserInterfaceUtils.Toggle(targetIndicatorContainer, true);
                });
        }
    }
}