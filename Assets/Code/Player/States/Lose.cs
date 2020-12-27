using System.Collections;

using UnityEngine;

namespace Player {
    public class Lose : State {
        public Lose(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Start() {
            PlayerSystem.Rigidbody.velocity = new Vector3(0, 0, 0);
            PlayerSystem.audioManager.Stop("AI Song 0");
            
            // TODO: make a UI panel that we can unhide and configure here
            // UI panel will need textbox that says either "nice job winning" or "whoops you lost", a textbox for the score,
            // and a button to go back to main menu scene
            float score = PlayerSystem.EnemiesKilled / PlayerSystem.ObstacleCount;
            Debug.Log("Score was: " + score);
            yield return null;
        }
    }
}