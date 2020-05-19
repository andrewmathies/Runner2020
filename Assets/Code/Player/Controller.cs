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

        private void Start() {
            StartCoroutine(Begin());
        }
        
        private IEnumerator Begin() {
            yield return new WaitForSeconds(3f);
            audioManager.Play("AI Song 0");
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("Run", true);
            rb = gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector3.right * InitialForce);
        }

        void OnTriggerEnter2D(Collider2D otherCollider) {
            //Debug.Log("player hit an obstacle");
        }
    }
}