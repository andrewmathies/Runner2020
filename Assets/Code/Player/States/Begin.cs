using System.Collections;

using UnityEngine;
using Midi;

namespace Player {
    public class Begin : State {
        public Begin(PlayerSystem playerSystem) : base(playerSystem) {
        }

        public override IEnumerator Start() {
            // wait for everything in the scene to load/initialize. TODO: replace with loading screen and async scene load stuff
            yield return new WaitForSeconds(3f);
            GameObject gameManager = GameObject.Find("GameManager");
            MidiParser parser = gameManager.GetComponent<MidiParser>();
            PlayerSystem.audioManager.Play(parser.SelectedSong);
            // audio file has 0.5 seconds of silence at beginning to avoid audio player hiccups
            yield return new WaitForSeconds(0.5f);
            PlayerSystem.Animator.SetBool("Run", true);
            PlayerSystem.Rigidbody.AddForce(Vector3.right * PlayerSystem.InitialForce);
            PlayerSystem.SetState(new Run(PlayerSystem));
        }
    }
}