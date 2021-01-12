using System.Collections.Generic;

using UnityEngine;

using Audio;

namespace Player {
    public class Sword : MonoBehaviour {
        private PlayerSystem playerSystem;
        
        private void Start() {
            GameObject player = gameObject.transform.parent.gameObject;
            this.playerSystem = player.GetComponent<PlayerSystem>();
        }

        // enqueue any objects that touch the area sword covers
        private void OnTriggerEnter2D(Collider2D obstacleCollider) {
            GameObject obstacleGameObject = obstacleCollider.gameObject;

            if (obstacleGameObject.tag != "Player" && obstacleGameObject.tag != "Sword") {
                Debug.Log("enemy has entered sword area");
                this.playerSystem.obstaclesInRange.Enqueue(obstacleGameObject);
            }
        }

        // dequeue any objects that leave the area the sword covers
        private void OnTriggerExit2D(Collider2D obstacleCollider) {
             GameObject obstacleGameObject = obstacleCollider.gameObject;

            // this is messy, but I want to destroy the obstacles after they are hit succesfully so I don't aware more than one point per enemy
            if (this.playerSystem.obstaclesInRange.Count != 0 && obstacleGameObject.tag != "Player" && obstacleGameObject.tag != "Sword") {
                Debug.Log("enemy has left sword area");
                this.playerSystem.obstaclesInRange.Dequeue();
            }
        }
    }
}