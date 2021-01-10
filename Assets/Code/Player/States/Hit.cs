using System.Collections;

using UnityEngine;

namespace Player {
    public class Hit : State {

        public Hit(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Start() {
            PlayerSystem.HitPoints--;
            Debug.Log("Player was hit by an enemy. current health is: " + PlayerSystem.HitPoints);

            // update ui with new hit points

            if (PlayerSystem.HitPoints == 0) {
                PlayerSystem.SetState(new End(PlayerSystem));
                yield break;
            }

            float framesOfHitStun = 8f;

            yield return new WaitForSeconds(PlayerSystem.MillisecondsPerFrame * framesOfHitStun);
            PlayerSystem.SetState(new Run(PlayerSystem));
        }
    }
}