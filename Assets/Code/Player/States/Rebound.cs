using System.Collections;

using UnityEngine;

namespace Player {
    public class Rebound : State {
        private bool wasHit = false;
        private bool Success;

        public Rebound(PlayerSystem playerSystem, bool success) : base(playerSystem) {
            this.Success = success;
        }

        public override IEnumerator Start() {
            PlayerSystem.FrameCounter = 0;

            if (this.Success) {
                yield return new WaitUntil(() => PlayerSystem.FrameCounter >= 5 || wasHit);
            } else {
                yield return new WaitUntil(() => PlayerSystem.FrameCounter >= 25 || wasHit);
            }
            
            if (wasHit) {
                yield break;
            }

            PlayerSystem.SetState(new Run(PlayerSystem));
        }

        public override IEnumerator Hit(GameObject obstacle) {
            wasHit = true;
            PlayerSystem.TookDamage();
            yield break;
        }
    }
}