using System.Collections;

using UnityEngine;

namespace Player {
    public class Hit : State {
        private bool attacked = false;
        private GameObject obstacle;

        public Hit(PlayerSystem playerSystem, GameObject obstacle) : base(playerSystem) {
            this.obstacle = obstacle;
        }

        public override IEnumerator Start() {
            // the player gets 2n frames to react and make an attack
            PlayerSystem.FrameCounter = 0;
            yield return new WaitUntil(() => PlayerSystem.FrameCounter >= 10 || attacked);

            if (attacked) {
                yield break;
            }

            PlayerSystem.TookDamage();
        }

        public override IEnumerator Attack() {
            attacked = true;

            PlayerSystem.EnemiesKilled++;
            // TODO: when we get art and animation for beholders, we should tell the obstacle
            // it needs to start the death animation and destroy itself instead of destroying it here
            Object.Destroy(obstacle);

            PlayerSystem.SetState(new Attack(PlayerSystem, true));
            yield return null;
        }
    }
}