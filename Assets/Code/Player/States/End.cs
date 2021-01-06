using UnityEngine;
using System.Collections;
using TMPro;

namespace Player {
    public class End : State {
        public End(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Start() {
            PlayerSystem.Rigidbody.velocity = new Vector3(0, 0, 0);
            PlayerSystem.audioManager.StopAll();

            bool won = PlayerSystem.HitPoints != 0;
            string resultText;

            if (won) {
                resultText = "You Won";
            } else {
                resultText = "You Lost";
            }

            TextMeshProUGUI resultTMP = PlayerSystem.ResultObject.GetComponent<TextMeshProUGUI>();
            resultTMP.SetText(resultText);

            TextMeshProUGUI scoreTMP = PlayerSystem.ScoreObject.GetComponent<TextMeshProUGUI>();
            scoreTMP.SetText("Score: " + PlayerSystem.EnemiesKilled + "/" + PlayerSystem.ObstacleCount);

            Animator endMenuAnimator = PlayerSystem.GameEnder.GetComponent<Animator>();
            endMenuAnimator.SetTrigger("ShowMenu");
            Debug.Log("Transitioning in end menu");

            yield return null;
        }
    }
}