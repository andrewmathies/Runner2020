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

            float framesOfAttack = 12f;
            int time = 0;
            bool hitAnEnemy = false;

            while (time < framesOfAttack) {
                while (PlayerSystem.obstaclesInRange.Count > 0) {
                    GameObject obstacle = PlayerSystem.obstaclesInRange.Dequeue();
                    obstacle.GetComponent<BoxCollider2D>().enabled = false;
                    obstacle.GetComponent<SpriteRenderer>().enabled = false;
                    UnityEngine.Object.Destroy(obstacle);

                    PlayerSystem.EnemiesKilled++;
                    hitAnEnemy = true;
                }

                yield return new WaitForSeconds(PlayerSystem.MillisecondsPerFrame);
                time++;
            }

            // TODO: death animation for beholder

            // check if game is over
            int enemiesEncountered = PlayerSystem.EnemiesKilled + PlayerSystem.StartingHealth - PlayerSystem.HealthUI.Health;

            if (enemiesEncountered == PlayerSystem.ObstacleCount) {
                PlayerSystem.SetState(new End(PlayerSystem));
            } else {
                PlayerSystem.SetState(new Rebound(PlayerSystem, hitAnEnemy));
            }

            yield return null;
        }
    }
}