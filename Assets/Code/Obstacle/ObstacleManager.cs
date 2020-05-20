﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Midi;
using MovingObjects;

namespace Obstacles {
    public class ObstacleManager : MonoBehaviour {
        public GameObject obstaclePrefab;
        public Vector3 newObstaclePosition;
        public Dictionary<ObstacleType, Texture2D> ObstacleTextures;

        // if we decide an enum is appropriate for difficulty selection, we can make this public and extend editor UI to
        // use correct enum types in inspector. reference comment by aqibsadiq
        // at https://forum.unity.com/threads/multiple-enum-select-from-inspector.184729/
        private Difficulty difficulty;
        private AdjacencyRules rules;
        private System.Random numberGenerator;
        private Queue<Obstacle> obstacles = new Queue<Obstacle>();
        private GameObject obstacleContainer;
        
        private MidiParser parser;

        // Start is called before the first frame update
        void Start()
        {
            numberGenerator = new System.Random();
            difficulty = Difficulty.Easy;
            rules = new AdjacencyRules();
            obstacleContainer = new GameObject("Obstacle Container");
            parser = gameObject.GetComponent<MidiParser>();

            // load textures for obstacles
            Texture2D blackTexture = Resources.Load<Texture2D>("Textures/black");
            Texture2D blueTexture = Resources.Load<Texture2D>("Textures/blue");
            Texture2D greenTexture = Resources.Load<Texture2D>("Textures/green");

            ObstacleTextures = new Dictionary<ObstacleType, Texture2D>();

            ObstacleTextures.Add(ObstacleType.Black, blackTexture);
            ObstacleTextures.Add(ObstacleType.Blue, blueTexture);
            ObstacleTextures.Add(ObstacleType.Green, greenTexture);

            StartCoroutine(MidiReader());
        }

        // return a randomly selected obstacle type (or null which means no obstacle). right now probability
        // that a particular type is generated is decided by difficulty enum. should this be a number that slowly
        // ramps up? should chance to generate one type change if one or more types are not in the options?
        private ObstacleType obstacleDecider(Difficulty difficulty, List<ObstacleType> options) {
            float chanceOfEachObstacle;

            switch (difficulty) {
                case Difficulty.Easy:
                    chanceOfEachObstacle = .05f;
                    break;
                case Difficulty.Medium:
                    chanceOfEachObstacle = .1f;
                    break;
                case Difficulty.Hard:
                    chanceOfEachObstacle = .15f;
                    break;
                default:
                    throw new System.ArgumentException("Invalid difficulty: " + difficulty);
            }

            foreach (ObstacleType type in options) {
                double d = numberGenerator.NextDouble();
                if (d < chanceOfEachObstacle) {
                    return type;
                }
            }

            // for now just default to black
            return ObstacleType.Black;
        }

        // returns a new game object to be placed in the scene as a new obstacle
        // create it under the obstaclecontainer in the scene hierarchy
        public Obstacle createObstacle(ObstacleType type) {
            GameObject newObstacleGameObject = Instantiate(obstaclePrefab, newObstaclePosition, Quaternion.identity, obstacleContainer.transform);
            Obstacle newObstacle = newObstacleGameObject.GetComponent<Obstacle>();
            newObstacle.Init(type);

            return newObstacle;
        }

        public void RemoveObstacle() {
            obstacles.Dequeue();
        }

        private IEnumerator MidiReader() {
            // wait for the queue to have something in it if this starts first
            while (parser.Tracks.Count == 0) {
                yield return null;    
            }

            Track metaTrack = parser.Tracks[0];
            Track melodyTrack = parser.Tracks[1];
            bool generate = false;
            uint microSecondsPerQuarterNote = 0;
            ushort ticksPerQuarterNote = 0;

            if (!parser.TimeBasedDivision) {
                ticksPerQuarterNote = parser.TimeDivision;
            } else {
                Debug.Log("this midi files uses a rare/strange way to encode timing of events");
                yield break;
            }

            // TODO: currently this blocks forever if there is no set tempo meta event. that is bad
            while (true) {
                 Midi.Event e = metaTrack.Events.Dequeue();
                 if (e.GetType() == typeof(Midi.MetaEvent)) {
                     MetaEvent metaEvent = (MetaEvent) e;
                     if (metaEvent.Type == MetaEventType.SetTempo) {
                         microSecondsPerQuarterNote = metaEvent.MicroSecondsPerQuarterNote;
                         break;
                     }
                 }
            }

            // this loop reads through every event in the Track and sleeps the appropriate amount of time between each
            while (melodyTrack.Events.Count > 0) {
                // read event
                Midi.Event e = melodyTrack.Events.Dequeue();

                if (e.GetType() == typeof(Midi.MidiEvent)) {
                    MidiEvent midiEvent = (MidiEvent) e;

                    if (midiEvent.Type == MidiEventType.NoteOn) {
                        generate = true;
                    }
                }

                // create a new obstacle if it was a note on event
                if (generate) {
                    Dictionary<ObstacleType, int> distances = new Dictionary<ObstacleType, int>();
                    List<ObstacleType> potentialObstacles = rules.Apply(distances);
                    ObstacleType newObstacleType = obstacleDecider(difficulty, potentialObstacles);
                    Obstacle newObstacle = createObstacle(newObstacleType);
                    obstacles.Enqueue(newObstacle);
                    generate = false;
                }
                
                // wait event delta time
                float sleepTime = Convert.ToSingle(e.Delta) * (Convert.ToSingle(microSecondsPerQuarterNote) / 1000000f) / Convert.ToSingle(ticksPerQuarterNote);
                //Debug.Log("Sleeping for " + sleepTime + " seconds");
                yield return new WaitForSeconds(sleepTime);
            }

            Debug.Log("No more events to parse! level is finished");
        }
    }
}