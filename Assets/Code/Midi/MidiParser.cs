using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Midi {
    public class MidiParser : MonoBehaviour {
        public ushort Format, TrackCount, TimeDivision;
        public bool TimeBasedDivision;
        public List<Track> Tracks;
        public double SecondsPerTick;
        public string SelectedSong;

        private void Awake() {
            Debug.Log("starting to parse the midi file for ai13");
            this.SelectedSong = "ai13";
            TextAsset MidiData = Resources.Load("Midi\\ai13") as TextAsset;
            Stream s = new MemoryStream(MidiData.bytes);
            this.Parse(s);
            this.SecondsPerTick = this.CalculateSecondsPerTick();
        }

        private void Parse(Stream stream) {
            using (BinaryReader reader = new BinaryReader(stream)) {
                Chunk headerChunk = new Chunk(reader);

                if (headerChunk.Type != "MThd") {
                    throw new Exception("no header chunk at start of file!");
                }

                if (headerChunk.Data.Length != 6) {
                    throw new Exception("invalid header chunk length: " + headerChunk.Data.Length);
                }

                byte[] buffer = new byte[2];
                
                Buffer.BlockCopy(headerChunk.Data, 0, buffer, 0, 2);
                Array.Reverse(buffer);
                Format = BitConverter.ToUInt16(buffer, 0);

                Buffer.BlockCopy(headerChunk.Data, 2, buffer, 0, 2);
                Array.Reverse(buffer);
                TrackCount = BitConverter.ToUInt16(buffer, 0);

                Buffer.BlockCopy(headerChunk.Data, 4, buffer, 0, 2);
                Array.Reverse(buffer);
                TimeDivision = BitConverter.ToUInt16(buffer, 0);

                BitArray timeDivisionBits = new BitArray(buffer);
                TimeBasedDivision = timeDivisionBits[0];

                if (TimeBasedDivision) {
                    Debug.Log("negativee SMPTE format");
                } else {
                    Debug.Log("ticks per quarter note");
                }

                Tracks = new List<Track>();

                for (ushort i = 0; i < TrackCount; i++) {
                    Chunk trackChunk = new Chunk(reader);
                    Tracks.Add(new Track(trackChunk));
                }

                if (Tracks.Count == TrackCount) {
                    Debug.Log("Finished parsing midi file");
                }
            }
        }

        private double CalculateSecondsPerTick() {
            Track metaTrack = this.Tracks[0];

            uint microSecondsPerQuarterNote = 0;
            ushort ticksPerQuarterNote = 0;

            if (!this.TimeBasedDivision) {
                ticksPerQuarterNote = this.TimeDivision;
            } else {
                throw new System.Exception("this midi files uses a rare/strange way to encode timing of events");
            }

            while (true) {
                if (metaTrack.Events.Count == 0) {
                    throw new System.Exception("could not find a set tempo event in first track of midi file");
                }

                 Midi.Event e = metaTrack.Events.Dequeue();
                 if (e.GetType() == typeof(Midi.MetaEvent)) {
                     MetaEvent metaEvent = (MetaEvent) e;
                     if (metaEvent.Type == MetaEventType.SetTempo) {
                         microSecondsPerQuarterNote = metaEvent.MicroSecondsPerQuarterNote;
                         break;
                     }
                 }
            }

            // this is really stupid and hacky but clearly the midi is slightly slower than the audio
            return ((Convert.ToDouble(microSecondsPerQuarterNote) / 1000000.0) / Convert.ToDouble(ticksPerQuarterNote)) * .992;
        }
    }
}
