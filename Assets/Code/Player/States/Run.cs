using System.Collections;

using UnityEngine;

namespace Player {
    public class Run : State {
        public Run(PlayerSystem playerSystem) : base(playerSystem) {
        }
         
        public override IEnumerator Start() {
            PlayerSystem.Animator.SetBool("Run", true);
            yield return null;
        }

        // when player is in run state, they are not close enough to hit an enemy when attacking
        public override IEnumerator Attack() {
            /*
            PlayerSystem.Animator.SetBool("Attack", true);
            Debug.Log("attacking in run state");
            PlayerSystem.FrameCounter = 0;
            // TODO: change 5 to whatever length of attack animation is
            yield return new WaitUntil(() => PlayerSystem.FrameCounter >= 5);
            PlayerSystem.Animator.SetBool("Attack", false);

            PlayerSystem.SetState(new Rebound(PlayerSystem));
            */
            PlayerSystem.SetState(new Attack(PlayerSystem, false));
            yield return null;
        }

        // this means the player is close enough to hit the enemy now
        public override IEnumerator Hit(GameObject obstacle) {
            PlayerSystem.SetState(new Hit(PlayerSystem, obstacle));
            yield return null;
        }
    }
}