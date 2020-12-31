using System.Collections;

using UnityEngine;

namespace Player {
    public class Run : State {
        public Run(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Attack() {
            PlayerSystem.SetState(new Attack(PlayerSystem));
            yield return null;
        }

        /*
        // this means the player is close enough to hit the enemy now
        public override IEnumerator Hit() {
            PlayerSystem.SetState(new Hit(PlayerSystem));
            yield return null;
        }
        */
    }
}