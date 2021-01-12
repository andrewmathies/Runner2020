using System.Collections;

using UnityEngine;

namespace Player {
    public class Rebound : State {
        private bool AttackHitEnemy;
        private bool HitDuringRebound;

        public Rebound(PlayerSystem playerSystem, bool AttackHitEnemy) : base(playerSystem) {
            this.AttackHitEnemy = AttackHitEnemy;
            this.HitDuringRebound = false;
        }

        public override IEnumerator Start() {
            float framesOfMissedAttack = 20f;
            float framesOfRebound = 9f;

            if (!this.AttackHitEnemy) {
                yield return new WaitForSeconds(PlayerSystem.MillisecondsPerFrame * framesOfMissedAttack);
            }

            // this is protection for concurrency. the Hit method could be called while we sleep above
            if (this.HitDuringRebound) {
                this.HitDuringRebound = false;
                yield break;
            }

            yield return new WaitForSeconds(PlayerSystem.MillisecondsPerFrame * framesOfRebound);

            PlayerSystem.SetState(new Run(PlayerSystem));
        }

        
        public override IEnumerator Hit() {
            this.HitDuringRebound = true;

            PlayerSystem.SetState(new Hit(PlayerSystem));
            yield return null;
        }
    }
}