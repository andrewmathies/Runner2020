using System.Collections;

using UnityEngine;

namespace Player {
    public abstract class State {
        protected PlayerSystem PlayerSystem;

        public State(PlayerSystem playerSystem) {
            PlayerSystem = playerSystem;
        }

        public virtual IEnumerator Start() {
            yield break;
        }

        public virtual IEnumerator Attack() {
            yield return null;
        }

        public virtual IEnumerator Hit(GameObject obstacle) {
            yield break;
        }
    }
}