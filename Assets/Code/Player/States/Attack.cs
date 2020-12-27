using System.Collections;

using UnityEngine;

namespace Player {
    public class Attack : State {
        private bool Success;

        public Attack(PlayerSystem playerSystem, bool success) : base(playerSystem) {
            this.Success = success;
        }

        public override IEnumerator Start() {
            PlayerSystem.Animator.SetBool("Attack", true);
            Debug.Log("starting an attack");

            PlayerSystem.FrameCounter = 0;
            // TODO: replace n with attack animation length
            yield return new WaitUntil(() => PlayerSystem.FrameCounter >= 10);
            PlayerSystem.Animator.SetBool("Attack", false);

            

            int enemiesEncountered = PlayerSystem.EnemiesKilled + PlayerSystem.MaxHealth - PlayerSystem.HitPoints;

            if (enemiesEncountered == PlayerSystem.ObstacleCount) {
                PlayerSystem.SetState(new Win(PlayerSystem));
            } else {
                PlayerSystem.SetState(new Rebound(PlayerSystem, this.Success));
            }
        }
    }
}