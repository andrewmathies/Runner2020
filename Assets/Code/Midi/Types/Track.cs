using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Midi {
    public class Track {
        public List<Event> Events = new List<Event>();

        public Track(Chunk chunk) {
            if (chunk.Type != "MTrk") {
                throw new System.ArgumentException("Can only make tracks with track chunks");
            }

            byte[] data = chunk.Data;
            byte previousCommand = 0x00;
            byte command = 0x00;
            Event newEvent;

            using (BinaryReader decoder = new BinaryReader(new MemoryStream(data))) {
                while (decoder.BaseStream.Position != decoder.BaseStream.Length) {
                    ulong delta = decoder.DecodeUInt64();
                    command = decoder.ReadByte();

                    if ((0x80 & command) == 0) {
                        // running status
                        long curPosition = decoder.BaseStream.Position;
                        decoder.BaseStream.Position = curPosition - 1;
                        command = previousCommand;
                    } else {
                        previousCommand = command;
                    }
                    
                    if (command == 0xFF) {
                        // meta event
                        newEvent = new MetaEvent(decoder);
                    } else if (command == 0xF0 || command == 0xF7) {
                        //sys ex event
                        newEvent = new SysExEvent(decoder);
                    } else {
                        // midi event
                        newEvent = new MidiEvent(command, decoder);
                    }

                    newEvent.Delta = delta;
                    //Debug.Log("created new event: " + newEvent);
                    Events.Add(newEvent);
                }
            }

            //Debug.Log("Read " + Events.Count + " events for a track");
        }
    }
}