using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Audio;
using Midi;

namespace Player {
    public class PlayerSystem : StateMachine {
        public GameObject gameManager;
        public GameObject GameEnder;
        public GameObject ResultObject;
        public GameObject ScoreObject;
        public HealthUI HealthUI;
        
        public float Speed;

        public AudioManager audioManager;
        public Rigidbody2D Rigidbody;
        public Animator AttackAnimator;
        [HideInInspector]
        public Animator RunAnimator;
        public int ObstacleCount;
        [HideInInspector]
        public int StartingHealth;

        public int EnemiesKilled = 0;
        public float InitialForce;
        public string debugState;

        public Queue<GameObject> obstaclesInRange;

        public const float MillisecondsPerFrame = 0.0166f;
        
        private void Start() {
            this.obstaclesInRange = new Queue<GameObject>();
            this.InitialForce = this.Speed * 50;
            this.StartingHealth = HealthUI.Health;

            this.RunAnimator = gameObject.GetComponent<Animator>();
            this.audioManager = this.gameManager.GetComponent<AudioManager>();
            this.Rigidbody = gameObject.GetComponent<Rigidbody2D>();

            // start moving the player
            RunAnimator.SetBool("Run", true);
            // start animating player run
            Rigidbody.AddForce(Vector3.right * InitialForce);
            SetState(new Run(this));
            StartCoroutine(PlaySong());
        }

        
        private void OnTriggerEnter2D(Collider2D obstacleCollider) {
            GameObject obstacleGameObject = obstacleCollider.gameObject;

            if (obstacleGameObject.tag != "Sword") {
                obstaclesInRange.Dequeue();
                Debug.Log("player hit: " + obstacleGameObject.tag);
                // hide it and turn off the collission so we will only get hit once per enemy
                obstacleCollider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                obstacleCollider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(State.Hit());
            }
        }

        private void Update() {
            bool tap = Input.GetButton("Attack");
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

         private IEnumerator PlaySong() {
            // audio file has 0.5 seconds of silence at beginning to avoid audio player hiccups
            float silenceAtBeginning = 0.5f;
            // the player needs some time to see the obstacles, so they will be running before the music starts
            float runTime = 5f;

            yield return new WaitForSeconds(runTime - silenceAtBeginning);
            
            // get reference to the midi data for this level
            MidiParser parser = this.gameManager.GetComponent<MidiParser>();
            string songTitle = parser.SelectedSong;
            // start playing the audio for this level
            this.audioManager.Play(songTitle);
            Debug.Log("Starting audio for: " + songTitle);
        }
    }
}