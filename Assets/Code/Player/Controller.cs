using UnityEngine;

using Audio;

namespace Player {
    public class Controller : MonoBehaviour {
        public Animator animator;
        public AudioManager audioManager;
        public float speed;

        private Rigidbody2D rb;

        private void Start() {
            animator.SetBool("Run", true);
            rb = gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector3.right * speed);
            audioManager.Play("AI Song 0");
        }

        void OnTriggerEnter2D(Collider2D otherCollider) {
            //Debug.Log("player hit an obstacle");
        }
    }
}