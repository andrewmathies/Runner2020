using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Midi;
using Player;

namespace Obstacle {
    public class ObstacleManager : MonoBehaviour {
        public GameObject obstaclePrefab;
        public GameObject player;
        public Dictionary<ObstacleType, Texture2D> ObstacleTextures;
        
        private Queue<GameObject> obstacles = new Queue<GameObject>();
        private GameObject obstacleContainer;
        private Vector3 newObstaclePosition = new Vector3(20, 13, 0);
        private float speed = 150f;

        void Start()
        {   
            obstacleContainer = new GameObject("Obstacle Container");
            LoadObstacleTextures();

            MidiParser parser = gameObject.GetComponent<MidiParser>();

            if (parser == null) {
                Debug.Log("MidiParser was not initialized before ObstacleManager");
                return;
            }

            // wait for the queue to have something in it to avoid race conditions
            while (parser.Tracks.Count == 0) {}

            Track melodyTrack = parser.Tracks[1];   

            GenerateObstacles(melodyTrack);
            StartCoroutine(SendObstacles(melodyTrack, parser.SecondsPerTick));
        }

        private void LoadObstacleTextures() {
            // load textures for obstacles
            Texture2D beholderTexture = Resources.Load<Texture2D>("Textures/black");

            ObstacleTextures = new Dictionary<ObstacleType, Texture2D>();
            ObstacleTextures.Add(ObstacleType.Beholder, beholderTexture);
        }

        // returns a new game object to be placed in the scene as a new obstacle
        // create it under the obstaclecontainer in the scene hierarchy
        public GameObject CreateObstacle(ObstacleType type, Vector3 position) {
            GameObject newObstacleGameObject = Instantiate(obstaclePrefab, 
                                                           position,
                                                           Quaternion.identity,
                                                           obstacleContainer.transform);
            
            Obstacle newObstacle = newObstacleGameObject.GetComponent<Obstacle>();
            newObstacle.Init(type);

            return newObstacleGameObject;
        }

        private void GenerateObstacles(Track track) {
            foreach (Midi.Event e in track.Events.ToArray()) {
                if (e.GetType() == typeof(Midi.MidiEvent)) {
                    MidiEvent midiEvent = (MidiEvent) e;

                    // create a new obstacle if it was a note on event
                    if (midiEvent.Type == MidiEventType.NoteOn) {
                        GameObject newObstacle = CreateObstacle(ObstacleType.Beholder, newObstaclePosition);
                        obstacles.Enqueue(newObstacle);
                    }
                }
            }
        }

        private IEnumerator SendObstacles(Track track, float secondsPerTick) {
            while (track.Events.Count > 0) {
                // read the next event in this track
                Midi.Event e = track.Events.Dequeue();
                float sleepTime = Convert.ToSingle(e.Delta) * secondsPerTick;

                if (e.GetType() == typeof(Midi.MidiEvent)) {
                    MidiEvent midiEvent = (MidiEvent) e;

                    // create a new obstacle if it was a note on event
                    if (midiEvent.Type == MidiEventType.NoteOn) {
                        if (obstacles.Count == 0) {
                            Debug.Log("Tried to dequeue an obstacle from an empty queue");
                            yield break;
                        }

                        GameObject obstacle = obstacles.Dequeue();
                        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
                        rb.AddForce(Vector2.left * speed);
                    }
                }

                yield return new WaitForSeconds(sleepTime);
            }
        }
    }
}