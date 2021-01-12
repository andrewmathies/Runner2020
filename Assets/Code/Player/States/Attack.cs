using System.Collections;

using UnityEngine;

namespace Player {
    public class Attack : State {
        public Attack(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Start() {
            Debug.Log("starting an attack");
            PlayerSystem.AttackAnimator.SetTrigger("Attack");
            PlayerSystem.audioManager.Play("sword-slash-sound");

            if (PlayerSystem.obstaclesInRange.Count == 0) {
                PlayerSystem.SetState(new Rebound(PlayerSystem, false));
                yield break;
            }

            float framesOfAttack = 3f;
            yield return new WaitForSeconds(PlayerSystem.MillisecondsPerFrame * framesOfAttack);

            // TODO: when we get art and animation for beholders, we should tell the obstacle
            // it needs to start the death animation and destroy itself instead of destroying it here
            if (PlayerSystem.obstaclesInRange.Count != 0) {
                GameObject obstacle = PlayerSystem.obstaclesInRange.Dequeue();
                obstacle.GetComponent<SpriteRenderer>().enabled = false;
                obstacle.GetComponent<BoxCollider2D>().enabled = false;
            }

            // check if game is over
            PlayerSystem.EnemiesKilled++;
            int enemiesEncountered = PlayerSystem.EnemiesKilled + PlayerSystem.StartingHealth - PlayerSystem.HealthUI.Health;

            if (enemiesEncountered == PlayerSystem.ObstacleCount) {
                PlayerSystem.SetState(new End(PlayerSystem));
            } else {
                PlayerSystem.SetState(new Rebound(PlayerSystem, true));
            }

            yield return null;
        }
    }
}