using System.Collections;

using UnityEngine;

namespace Player {
    public class Attack : State {
        public Attack(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Start() {
            PlayerSystem.EnemiesKilled++;

            PlayerSystem.Animator.SetBool("Attack", true);
            PlayerSystem.FrameCounter = 0;
            // TODO: replace 5 with attack animation length
            yield return new WaitUntil(() => PlayerSystem.FrameCounter >= 5);
            PlayerSystem.Animator.SetBool("Attack", false);

            int enemiesEncountered = PlayerSystem.EnemiesKilled + PlayerSystem.MaxHealth - PlayerSystem.HitPoints;

            if (enemiesEncountered == PlayerSystem.ObstacleCount) {
                PlayerSystem.SetState(new Win(PlayerSystem));
            } else {
                PlayerSystem.SetState(new Run(PlayerSystem));
            }
        }
    }
}