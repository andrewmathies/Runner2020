using System.Collections;

using UnityEngine;

namespace Player {
    public class Rebound : State {
        private bool Success;

        public Rebound(PlayerSystem playerSystem, bool success) : base(playerSystem) {
            this.Success = success;
        }

        public override IEnumerator Start() {
            PlayerSystem.FrameCounter = 0;

            if (!this.Success) {
                yield return new WaitUntil(() => PlayerSystem.FrameCounter >= 20);
            }

            // this number > the length in frames of attack animation
            yield return new WaitUntil(() => PlayerSystem.FrameCounter >= 12);

            PlayerSystem.SetState(new Run(PlayerSystem));
        }

        /*
        public override IEnumerator Hit() {
            wasHit = true;
            PlayerSystem.TookDamage();
            yield break;
        }
        */
    }
}