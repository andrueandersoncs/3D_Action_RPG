using System;
using System.Collections;
using Abilities;
using AI.Abilities;
using Combat;
using Combat.Abilities;
using Movement.Abilities;
using UniRx;
using UnityEngine;

namespace AI
{
    // Melee state
    // Spell state
    // Chase state
    // Roam state
    
    public class AIController : MonoBehaviour
    {
        public EnemyDetector enemyDetector;
        
        public RoamAbility roamAbility;
        public MeleeAttackAbility meleeAttackAbility;
        public FollowTargetAbility followTargetAbility;
        
        private IEnumerator currentState;
        private Vector3 origin;
        
        private void OnEnable()
        {
            origin = transform.position;
            
            roamAbility.origin = origin;
            roamAbility.Play();
            
            enemyDetector
                .detectedEnemies
                .ObserveAdd()
                .Throttle(TimeSpan.FromSeconds(0.5f))
                .Subscribe(pair =>
                {
                    if (!pair.Value.TryGetComponent(out CombatGroup enemy)) return;
                    
                    meleeAttackAbility.enemy = enemy;
                    
                    followTargetAbility.target = enemy.gameObject;
                    followTargetAbility.onReachedTarget += () => meleeAttackAbility.Play();
                    followTargetAbility.Play();
                })
                .AddTo(this);
        }
    }
}