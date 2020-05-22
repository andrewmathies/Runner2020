using System.Collections;

using UnityEngine;

namespace Player {
    public class Rebound : State {
        private bool wasHit = false;

        public Rebound(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Start() {
            PlayerSystem.FrameCounter = 0;
            yield return new WaitUntil(() => PlayerSystem.FrameCounter >= 5 || wasHit);

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