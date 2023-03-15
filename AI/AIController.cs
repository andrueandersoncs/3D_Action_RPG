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
     * - Roam (DONE)
     * - Seek (DONE)
     *
     * - Melee Attack
     * (Instant Damage, Damage Over Time, Status Effect)
     * 
     * - Ranged Attack
     * (Projectile, Magic projectile, Explosive projectile)
     * 
     * - Magic Attack
     * (Projectile, Area of Effect, Instant Damage, Damage Over Time)
     * 
     * - Flee
     *
     * - Heal Self
     * (Heal Over Time, Instant Heal)
     * 
     * - Buff Self
     * (Damage Over Time, Damage Reduction, Damage Increase, Health Increase, Health Regeneration)
     * 
     * - Heal Ally
     * (Heal Over Time, Instant Heal)
     * 
     * - Buff Ally
     * (Damage Over Time, Damage Reduction, Damage Increase, Health Increase, Health Regeneration)
     *
     * ------------------------------------------------------------------------------------------------
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
     * Combinators:
     * - And / Or / Not
     *
     *------------------------------------------------------------------------------------------------
     * 
     * Combat Roles:
     * - Tank
     * (Behavior: Seek, Melee Attack, Buff Self, Buff Ally)
     * 
     * - Healer
     * (Behavior: Heal Self, Heal Ally, Flee)
     * 
     * - Support
     * (Behavior: Buff Self, Buff Ally, Flee)
     * 
     * - Ranged DPS
     * (Behavior: Seek, Ranged Attack, Flee)
     * 
     * - Melee DPS
     * (Behavior: Seek, Melee Attack, Flee)
     * 
     * - Magic DPS
     * (Behavior: Magic Attack, Flee, Buff Ally)
     * 
     * - Summoner
     * (Behavior: Summon, Flee, Magic Attack)
     * 
     * - Crowd Control
     * (Behavior: Seek, Melee Attack, Flee, Buff Ally)
     *
     * ------------------------------------------------------------------------------------------------
     *
     * Spell Effects:
     * - (Elemental) Resistance Buff
     * - (Elemental) Damage Buff
     * - Vitals Buff
     * - ActionStats Buff
     * - Attributes Buff
     * 
     * - Attacks Gain (Elemental) Damage Over Time
     * - Attacks Gain (Elemental) Area of Effect Damage
     * - Attacks Cause (Vitals | ActionStats | Attributes) Reduction
     * - Attacks Cast Spell (Any?)
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