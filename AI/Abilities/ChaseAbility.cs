using System.Collections;
using Abilities;
using Combat;
using Movement.Abilities;
using UnityEngine;

namespace AI.Abilities
{
    public class ChaseAbility : Ability
    {
        public EnemyDetector enemyDetector;
        public MoveToGameObjectAbility moveToGameObjectAbility;
        public GameObject enemy;
        
        protected override IEnumerator Execute()
        {
            while (enemyDetector.detectedEnemies.Contains(enemy))
            {
                moveToGameObjectAbility.target = enemy;
                yield return moveToGameObjectAbility.Play();
                yield return new WaitForSeconds(0.5f);
            }
        }
        
        public override void Stop()
        {
            base.Stop();
            moveToGameObjectAbility.Stop();
        }
    }
}