using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Midi {
    public class MidiFile {

        public ushort Format, TrackCount, TimeDivision;

        public List<Track> Tracks;

        public MidiFile(string path) {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open))) {
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

                //Debug.Log("Successfully read header chunk");
                //Debug.Log("Track count is: " + TrackCount);

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
    }
}