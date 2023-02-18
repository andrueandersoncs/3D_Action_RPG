using System;
using Combat.Abilities;
using Stats.DamageTypes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Combat
{
    public class DamageTrigger : MonoBehaviour
    {
        public Collider trigger;

        public bool onEnter;
        
        public bool onStay;
        public float stayInterval;
        
        private void OnEnable()
        {
            var damageStats = GetComponent<DamageStats>();
            
            trigger
                .OnTriggerEnterAsObservable()
                .Subscribe(collider =>
                {
                    if (!collider.TryGetComponent<ReceiveDamageAbility>(out var receiveDamageAbility)) return;

                    if (onEnter)
                    {
                        receiveDamageAbility.damageStatsToReceive = damageStats;
                        receiveDamageAbility.Play();
                    }
                    
                    if (onStay)
                    {
                        Observable
                            .Interval(TimeSpan.FromSeconds(stayInterval))
                            .TakeUntil(trigger.OnTriggerExitAsObservable().Where(exitCollider => exitCollider == collider))
                            .Subscribe(_ =>
                            {
                                receiveDamageAbility.damageStatsToReceive = damageStats;
                                receiveDamageAbility.Play();
                            })
                            .AddTo(this);
                    }
                })
                .AddTo(this);
        }
    }
}