using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;

using Midi;

namespace Obstacle {
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
            //Texture2D blackTexture = Resources.Load<Texture2D>("Textures/black");
            //Texture2D blueTexture = Resources.Load<Texture2D>("Textures/blue");
            //Texture2D greenTexture = Resources.Load<Texture2D>("Textures/green");
            Texture2D beholderTexture = Resources.Load<Texture2D>("Textures/black");
            // this is an empty texture
            Texture2D signalTexture = new Texture2D(2, 2);

            ObstacleTextures = new Dictionary<ObstacleType, Texture2D>();

            //ObstacleTextures.Add(ObstacleType.Black, blackTexture);
            //ObstacleTextures.Add(ObstacleType.Blue, blueTexture);
            //ObstacleTextures.Add(ObstacleType.Green, greenTexture);
            ObstacleTextures.Add(ObstacleType.Beholder, beholderTexture);
            ObstacleTextures.Add(ObstacleType.Signal, signalTexture);

            StartCoroutine(MidiReader());
        }
/*
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

        // returns distances to the closest obstacle of each type in cells
        public Dictionary<ObstacleType, int> calculateDistances() {
            Dictionary<ObstacleType, int> distances = new Dictionary<ObstacleType, int>();
            Grid grid = GameObject.FindObjectOfType(typeof(Grid)) as Grid;
            Vector3Int newObstacleCellPosition = grid.WorldToCell(newObstaclePosition);

            // iterate through all obstacles in the scene, starting with oldest
            // i.e. farthest from the right side of the screen
            foreach (Obstacle obstacle in obstacles) {
                ObstacleType type = obstacle.Type;
                
                if (distances.ContainsKey(type)) {
                    distances.Remove(type);
                }
                
                Vector3Int curObstacleCellPosition = grid.WorldToCell(obstacle.transform.position);
                int cellDist = (int) Math.Abs(newObstacleCellPosition.x - curObstacleCellPosition.x);
                distances[type] = cellDist;
            }

            return distances;
        }
*/
        // returns a new game object to be placed in the scene as a new obstacle
        // create it under the obstaclecontainer in the scene hierarchy
        public Obstacle createObstacle(ObstacleType type) {
            //Dictionary<ObstacleType, int> distances = calculateDistances();
            //List<ObstacleType> potentialObstacles = rules.Apply(distances);
            //ObstacleType newObstacleType = obstacleDecider(difficulty, potentialObstacles);
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

            // send an invisible obstacle indicating that the song has started
            Obstacle startObstacle = createObstacle(ObstacleType.Signal);
            obstacles.Enqueue(startObstacle);
            // AIVA adds half a second of silence to the beginning of the audio files to avoid clipping
            yield return new WaitForSeconds(0.5f);

            Track metaTrack = parser.Tracks[0];
            Track melodyTrack = parser.Tracks[1];
            bool generate = false;
            uint microSecondsPerQuarterNote = 0;
            ushort ticksPerQuarterNote = 0;

            if (!parser.TimeBasedDivision) {
                ticksPerQuarterNote = parser.TimeDivision;
            } else {
                UnityEngine.Debug.Log("this midi files uses a rare/strange way to encode timing of events");
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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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
                    Obstacle newObstacle = createObstacle(ObstacleType.Beholder);
                    obstacles.Enqueue(newObstacle);
                    generate = false;
                }
                
                // wait event delta time
                float sleepTime = Convert.ToSingle(e.Delta) * (Convert.ToSingle(microSecondsPerQuarterNote) / 1000000f) / Convert.ToSingle(ticksPerQuarterNote);
                //Debug.Log("Sleeping for " + sleepTime + " seconds");
                long stopWatchTime = stopwatch.ElapsedMilliseconds;
                sleepTime -= Convert.ToSingle(stopWatchTime) / 1000f;
                sleepTime -= 0.01f;
                //UnityEngine.Debug.Log("stopwatch time: " + time);
                yield return new WaitForSeconds(sleepTime);
                stopwatch.Restart();
            }

            UnityEngine.Debug.Log("No more events to parse! level is finished");
        }
    }
}