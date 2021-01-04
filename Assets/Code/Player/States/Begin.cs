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

            // audio file has 0.5 seconds of silence at beginning to avoid audio player hiccups
            float silenceAtBeginning = 0.5f;
            // the player needs some time to see the obstacles, so they will be running before the music starts
            float runTime = 5f;

            // start moving the player
            PlayerSystem.RunAnimator.SetBool("Run", true);
            // start animating player run
            PlayerSystem.Rigidbody.AddForce(Vector3.right * PlayerSystem.InitialForce);
            yield return new WaitForSeconds(runTime - silenceAtBeginning);
            
            // get reference to the midi data for this level
            MidiParser parser = PlayerSystem.gameManager.GetComponent<MidiParser>();
            string songTitle = parser.SelectedSong;
            // start playing the audio for this level
            PlayerSystem.audioManager.Play(songTitle);
            Debug.Log("Starting audio for: " + songTitle);

            PlayerSystem.SetState(new Run(PlayerSystem));
        }
    }
}