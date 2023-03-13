using System;
using System.Collections;
using AI.Abilities;
using Combat;
using Combat.Abilities;
using Movement.Abilities;
using UniRx;
using UnityEngine;

namespace AI
{
    /*
     * Things a monster can do:
     * - Roam
     * - Chase / Seek
     * - Melee Attack
     * - Ranged Attack
     * - Cast Spell
     * - Retreat
     * - Heal Self
     * - Heal Ally
     *
     * Detectors:
     * - Enemy In Sight
     * - Ally In Sight
     * - Enemy Low Health
     * - Ally Low Health
     * - Self Low Health
     * - Self Low Mana
     * - Enemy Retreating
     * - Ally Retreating
     * - Random Number in Range
     * - Timer Expiration
     * - Buff Expiration
     * - Effect Expiration
     * 
     * Conditions:
     * - And / Or / Not
     */
    
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