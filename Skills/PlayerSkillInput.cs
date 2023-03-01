using System;
using System.Linq;
using Keyboard;
using UniRx;
using UnityEngine;

namespace Skills
{
    public class PlayerSkillInput : MonoBehaviour
    {
        public Skills skills;
        public UseSkillAbility useSkillAbility;
        public Camera camera;
        
        private RaycastHit[] raycastHits = new RaycastHit[10];
        private LayerMask terrainMask;
        
        private void OnEnable()
        {
            terrainMask = LayerMask.GetMask("Terrain");
            
            skills.skillPointAllocations
                .ObserveAdd()
                .Subscribe(_ => BindSkills())
                .AddTo(this);
        }

        private void BindSkills()
        {
            var skillsOrderedByLearned = skills.DistinctSkills.ToArray();

            var count = 1;
            foreach (var skill in skillsOrderedByLearned)
            {
                Keybindings.Bind(Enum.TryParse<KeyCode>($"Alpha{count}", out var key) ? key : KeyCode.None, () =>
                {
                    var ray = camera.ScreenPointToRay(Input.mousePosition);
                    var numHits = Physics.RaycastNonAlloc(ray, raycastHits, 50f, terrainMask); 
                    if (numHits == 0) return;
                    useSkillAbility.skill = skill;
                    useSkillAbility.target = raycastHits[0].point;
                    useSkillAbility.Play();
                });
                count++;
            }
        }
    }
}