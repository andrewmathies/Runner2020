using UnityEngine;

namespace Player {
    public class Controller : MonoBehaviour {
        public Animator animator;

        private bool running = false;
        private bool jumping = false;
        private int jumpCounter = 0;
        private const int L = 10;

        private void Update() {
            if (Input.GetButtonDown("Run")) {
                running = !running;
                animator.SetBool("Run", running);
            }

            if (Input.GetButtonDown("Jump") && !jumping) {
                jumping = true;
                jumpCounter = L;
            } else if (jumping) {
                jumpCounter--;

                if (jumpCounter == 0) {
                    jumping = false;
                }
            }
        }

        private void OnCollisionEnter(Collision other) {
            Debug.Log("Hit an obstacle");
        }
    }
}