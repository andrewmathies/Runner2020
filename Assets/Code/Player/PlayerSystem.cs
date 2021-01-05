using System.Collections.Generic;

using UnityEngine;

using Audio;

namespace Player {
    public class PlayerSystem : StateMachine {
        public GameObject gameManager;
        public GameObject GameEnder;
        
        public float Speed;

        public AudioManager audioManager;
        public Rigidbody2D Rigidbody;
        public Animator AttackAnimator;
        [HideInInspector]
        public Animator RunAnimator;

        public int FrameCounter = 0;
        public int EnemiesKilled = 0;
        // obstacle count will be set by the obstacle manager
        [HideInInspector]
        public int ObstacleCount;
        // current health of player
        public int HitPoints;
        [HideInInspector]
        public float InitialForce;
        public string debugState;

        [HideInInspector]
        public Queue<GameObject> obstaclesInRange;
        
        // this is how many times player can be hit and still live
        // TODO: will definitely need to be tuned
        public int MaxHealth;
        
        private void Start() {
            this.obstaclesInRange = new Queue<GameObject>();
            this.HitPoints = this.MaxHealth;
            this.InitialForce = this.Speed * 50;

            this.RunAnimator = gameObject.GetComponent<Animator>();
            this.audioManager = this.gameManager.GetComponent<AudioManager>();
            this.Rigidbody = gameObject.GetComponent<Rigidbody2D>();
            SetState(new Begin(this));
        }

        /*
        private void OnTriggerEnter2D(Collider2D obstacleCollider) {
            GameObject obstacleGameObject = obstacleCollider.gameObject;
            obstaclesInRange.Enqueue(obstacleGameObject);
            //StartCoroutine(State.Hit());
        }

        private void OnTriggerExit2D(Collider2D obstacleCollider) {
            // this is messy, but I want to destroy the obstacles after they are hit succesfully so I don't aware more than one point per enemy
            if (obstaclesInRange.Count != 0) {
                obstaclesInRange.Dequeue();
            }
        }
        */

        private void FixedUpdate() {
            FrameCounter++;
        }

        private void Update() {
            bool tap = tap = Input.GetButton("Attack");
            debugState = this.State.ToString();
            
            //TODO: implement touch on android
            foreach(Touch touch in Input.touches) {
                if (touch.phase == TouchPhase.Began) {
                    tap = true;
                }
            }

            if (tap) {
                if (debugState == "Player.Run") {
                    StartCoroutine(State.Attack());
                }
            }
        }

        /*
        // this should only be called from a state
        public void TookDamage() {
            this.HitPoints--;

            if (this.HitPoints == 0) {
                SetState(new Lose(this));
            } else {
                SetState(new Invulnerable(this));
            }
        }
        */
    }
}