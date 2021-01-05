using System.Collections;

using UnityEngine;

namespace Player {
    public class Attack : State {
        public Attack(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Start() {
            PlayerSystem.AttackAnimator.SetTrigger("Attack");
            Debug.Log("starting an attack");
            
            if (PlayerSystem.obstaclesInRange.Count == 0) {
                PlayerSystem.SetState(new Rebound(PlayerSystem, false));
                yield break;
            }

            // TODO: when we get art and animation for beholders, we should tell the obstacle
            // it needs to start the death animation and destroy itself instead of destroying it here
            GameObject obstacle = PlayerSystem.obstaclesInRange.Dequeue();
            obstacle.GetComponent<SpriteRenderer>().enabled = false;
            obstacle.GetComponent<BoxCollider2D>().enabled = false;

            // check if game is over
            PlayerSystem.EnemiesKilled++;
            int enemiesEncountered = PlayerSystem.EnemiesKilled + PlayerSystem.MaxHealth - PlayerSystem.HitPoints;

            if (enemiesEncountered == PlayerSystem.ObstacleCount) {
                PlayerSystem.SetState(new End(PlayerSystem));
            } else {
                PlayerSystem.SetState(new Rebound(PlayerSystem, true));
            }

            yield return null;
        }
    }
}