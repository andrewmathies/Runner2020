using System.Collections;

using UnityEngine;

using Audio;

namespace Player {
    public class Controller : MonoBehaviour {
        public Animator animator;
        public AudioManager audioManager;
        public static float InitialForce = 200f;
        public static float Speed = InitialForce / 50;

        private Rigidbody2D rb;
        private Player player = new Player();

        private void Start() {
            StartCoroutine(Begin());
        }
        
        private IEnumerator Begin() {
            // wait for everything in the scene to load/initialize. TODO: replace with loading screen and async scene load stuff
            yield return new WaitForSeconds(3f);
            audioManager.Play("AI Song 0");
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("Run", true);
            rb = gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector3.right * InitialForce);
        }

        private void OnTriggerEnter2D(Collider2D otherCollider) {
            //Debug.Log("player hit an obstacle");
            GameObject obstacleGameObject = otherCollider.gameObject;
            Destroy(obstacleGameObject);
        }

        private void Update() {
            bool tap = false;

            foreach(Touch touch in Input.touches) {
                if (touch.phase == TouchPhase.Began) {
                    tap = true;
                }
            }

            if (tap && player.State == PlayerState.Run) {
                player.ChangeState(Command.StartAttack);
            }
        }
    }
}