using UnityEngine;
using System.Collections;

namespace Player {
    public class End : State {
        public End(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Start() {
            PlayerSystem.Rigidbody.velocity = new Vector3(0, 0, 0);
            PlayerSystem.audioManager.StopAll();

            /*
            figure out if player won from the score
            float score = PlayerSystem.EnemiesKilled / PlayerSystem.ObstacleCount;

            get children of PlayerSystem.GameEnder
            set text for result and score textObjects
            
            get animator component of PlayerSystem.GameEnder
            set trigger to fade in menu and make menu interactable
            */
            yield return null;
        }
    }
}