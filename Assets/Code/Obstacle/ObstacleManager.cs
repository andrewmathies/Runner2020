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
        public float obstacleSpriteOffset;

        private GameObject obstacleContainer;
        private PlayerSystem playerSystem;

        void Start()
        {   
            playerSystem = player.GetComponent<PlayerSystem>();
            obstacleContainer = new GameObject("Obstacle Container");
            LoadObstacleTextures();

            MidiParser parser = gameObject.GetComponent<MidiParser>();

            if (parser == null) {
                Debug.LogError("Midi parser was not initialized before obstacle manager");
                return;
            }

            if (parser.Tracks.Count == 0) {
                Debug.Log("Could not parse the midi file");
                return;
            }

            // wait for the queues to fill up to avoid race conditions
            while (parser.Tracks[1].Events.Count == 0) {}

            // cannot use header track, that contains metadata
            //int trackSelection = UnityEngine.Random.Range(1, parser.Tracks.Count);
            GenerateObstacles(parser.Tracks[1], parser.SecondsPerTick);
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

        private void GenerateObstacles(Track track, float secondsPerTick) {
            float timeInSong = 0f;
            float playerSpeed = playerSystem.Speed;
            int obstacleCount = 0;

            // 1/3/21 put in  5 seconds of moving before audio plays to give player better chance with first obstacle
            float runningSilenceTime = 5f;
            float offset = runningSilenceTime * playerSpeed;
            // 0.25 is about half the character model. we want to place obstacles to line up with front of player not center
            float startPosition = player.transform.position.x + obstacleSpriteOffset + offset;
            
            while (track.Events.Count > 0) {
                // read the next event in this track, and add the delta time for this event to our counter
                Midi.Event e = track.Events.Dequeue();
                timeInSong += Convert.ToSingle(e.Delta) * secondsPerTick;

                if (e.GetType() == typeof(Midi.MidiEvent)) {
                    MidiEvent midiEvent = (MidiEvent) e;

                    // create a new obstacle if it was a note on event
                    if (midiEvent.Type == MidiEventType.NoteOn) {
                        obstacleCount++;
                        float obstaclePosition = startPosition + timeInSong * playerSpeed;
                        GameObject newObstacle = CreateObstacle(ObstacleType.Beholder, new Vector3(obstaclePosition, player.transform.position.y, 0));
                    }
                }
            }

            playerSystem.ObstacleCount = obstacleCount;
            Debug.Log("This track has " + obstacleCount + " obstacles");
        }
    }
}