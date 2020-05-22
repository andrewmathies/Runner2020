using UnityEngine;

using Audio;

namespace Player {
    public class Controller : MonoBehaviour {
        public Animator animator;

        private bool running = false;
        private bool songStarted = false;

        private void Update() {
            if (Input.GetButtonDown("Run")) {
                running = !running;
                animator.SetBool("Run", running);
            }
        }

        void OnTriggerEnter2D(Collider2D otherCollider) {
            //Debug.Log("player hit an obstacle");

            if (!songStarted) {
                songStarted = true;
                FindObjectOfType<AudioManager>().Play("AI Song 0");
            }
        }
    }
}