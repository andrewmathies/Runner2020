using System.Collections;

using UnityEngine;

using Audio;

namespace Player {
    public class PlayerSystem : StateMachine {
        public GameObject gameManager;
        
        public float Speed;

        public AudioManager audioManager;
        public Rigidbody2D Rigidbody;
        public Animator Animator;

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
        
        // this is how many times player can be hit and still live
        // TODO: will definitely need to be tuned
        public int MaxHealth;
        
        private void Start() {
            this.HitPoints = this.MaxHealth;
            this.InitialForce = this.Speed * 50;
            
            this.audioManager = this.gameManager.GetComponent<AudioManager>();
            this.Animator = gameObject.GetComponent<Animator>();
            this.Rigidbody = gameObject.GetComponent<Rigidbody2D>();
            SetState(new Begin(this));
        }

        private void OnTriggerEnter2D(Collider2D otherCollider) {
            GameObject obstacleGameObject = otherCollider.gameObject;
            StartCoroutine(State.Hit(obstacleGameObject));
        }

        private void FixedUpdate() {
            FrameCounter++;
        }

        private void Update() {
            bool tap = tap = Input.GetButton("Attack");
            debugState = this.State.ToString();

            /*
            TODO: implement touch on android
            foreach(Touch touch in Input.touches) {
                if (touch.phase == TouchPhase.Began) {
                    tap = true;
                }
            }
            */

            if (tap) {
                if (debugState == "Player.Hit" || debugState == "Player.Run") {
                    StartCoroutine(State.Attack());
                }
            }
        }

        // this should only be called from a state
        public void TookDamage() {
            this.HitPoints--;

            if (this.HitPoints == 0) {
                SetState(new Lose(this));
            } else {
                SetState(new Invulnerable(this));
            }
        }
    }
}

/*
TODO:
need UI for current health and max health of player
130, 130, 80 rgb
*/