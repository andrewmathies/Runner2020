using System.Collections;

using UnityEngine;

namespace Player {
    public class Invulnerable : State {
        public Invulnerable(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Start() {
            // TODO: make the player flash or change to a was hit animation or something
            PlayerSystem.FrameCounter = 0;
            yield return new WaitUntil(() => PlayerSystem.FrameCounter >= 5);
            PlayerSystem.SetState(new Run(PlayerSystem));
        }

        /*
        TODO: we need to figure out how to work this into the game        
    
        maybe make a MissedAttack method in PlayerSystem similar to the TookDamage method, and we call that from here and run
        in the Attack override method

        maybe in the hit method, we change to hit state but pass in an int that is the number of frames player
        has already been invulnerable. then we handle the logic of what happens when we encounter an enemy in the hit state

        seems kinda inelegant though
        */
    }
}