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

        private Queue<Obstacle> obstacles = new Queue<Obstacle>();
        private GameObject obstacleContainer;
        private float startPos = -4f;

        void Start()
        {   
            obstacleContainer = new GameObject("Obstacle Container");
            LoadObstacleTextures();

            MidiParser parser = gameObject.GetComponent<MidiParser>();
            StartCoroutine(MidiReader(parser));
        }

        private void LoadObstacleTextures() {
            // load textures for obstacles
            Texture2D beholderTexture = Resources.Load<Texture2D>("Textures/black");
            // this is an empty texture
            Texture2D signalTexture = new Texture2D(2, 2);

            ObstacleTextures = new Dictionary<ObstacleType, Texture2D>();

            ObstacleTextures.Add(ObstacleType.Beholder, beholderTexture);
            ObstacleTextures.Add(ObstacleType.Signal, signalTexture);
        }

        // returns a new game object to be placed in the scene as a new obstacle
        // create it under the obstaclecontainer in the scene hierarchy
        public GameObject CreateObstacle(ObstacleType type) {
            GameObject newObstacleGameObject = Instantiate(obstaclePrefab, newObstaclePosition, Quaternion.identity, obstacleContainer.transform);
            Obstacle newObstacle = newObstacleGameObject.GetComponent<Obstacle>();
            newObstacle.Init(type);

            return newObstacleGameObject;
        }

        public void RemoveObstacle() {
            obstacles.Dequeue();
        }

        public void SendSignalObstacle() {
            // send an invisible obstacle indicating that the song has started
            GameObject startObstacle = CreateObstacle(ObstacleType.Signal);
            startObstacle.transform.position = new Vector3(startPos, 13, 0);
            //obstacles.Enqueue(startObstacle);
        }

        private IEnumerator MidiReader(MidiParser parser) {
            // wait for the queue to have something in it if this starts first
            while (parser.Tracks.Count == 0) {
                yield return null;    
            }

            SendSignalObstacle();
            // AIVA adds half a second of silence to the beginning of the audio files to avoid clipping
            //yield return new WaitForSeconds(0.5f);

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

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            float curTime = 0;

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
                
                float sleepTime = Convert.ToSingle(e.Delta) * (Convert.ToSingle(microSecondsPerQuarterNote) / 1000000f) / Convert.ToSingle(ticksPerQuarterNote);
                curTime += sleepTime;
                float dist = startPos + curTime * 3;

                // create a new obstacle if it was a note on event
                if (generate) {
                    GameObject newObstacle = CreateObstacle(ObstacleType.Beholder);
                    //obstacles.Enqueue(newObstacle);
                    newObstacle.transform.position = new Vector3(dist, 13, 0);
                    generate = false;
                }

                //Debug.Log("Sleeping for " + sleepTime + " seconds");
                //long stopWatchTime = stopwatch.ElapsedMilliseconds;
                //sleepTime -= Convert.ToSingle(stopWatchTime) / 1000f;
                //sleepTime -= 0.01f;
                //UnityEngine.Debug.Log("stopwatch time: " + time);
                //yield return new WaitForSeconds(sleepTime);
                //stopwatch.Restart();
            }

            UnityEngine.Debug.Log("No more events to parse! level is finished");
        }
    }
}