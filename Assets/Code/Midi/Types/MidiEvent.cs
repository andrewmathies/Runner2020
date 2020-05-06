using System;
using System.IO;

using UnityEngine;

namespace Midi {
    public class MidiEvent : Event {
        public MidiEventType Type;

        public MidiEvent(byte commandByte, BinaryReader reader) {
            bool channelVoiceMessage = (0xF0 & commandByte) != 0xF0;

            if (channelVoiceMessage) {
                switch (0xF0 & commandByte) {
                    case 0x80:
                        Type = MidiEventType.NoteOff;
                        reader.ReadBytes(2);
                        break;
                    case 0x90:
                        Type = MidiEventType.NoteOn;
                        reader.ReadBytes(2);
                        break;
                    case 0xA0:
                        Type = MidiEventType.PolyphonicKeyPressure;
                        reader.ReadBytes(2);
                        break;
                    case 0xB0:
                        // same as Control Change. not important right now
                        // if we ever actually care about event data will have to handle this
                        Type = MidiEventType.ChannelModeMessage;
                        reader.ReadBytes(2);
                        break;
                    case 0xC0:
                        Type = MidiEventType.ProgramChange;
                        reader.ReadByte();
                        break;
                    case 0xD0:
                        Type = MidiEventType.ChannelPressure;
                        reader.ReadByte();
                        break;
                    case 0xE0:
                        Type = MidiEventType.PitchWheelChange;
                        reader.ReadBytes(2);
                        break;
                    default:
                        throw new System.Exception("Unknown midi event, command byte: " + commandByte);
                }
            } else {
                switch (commandByte) {
                    case 0xF0:
                        Type = MidiEventType.SystemExclusive;
                        // read until 11110111
                        reader.ReadUntil(0b11110111);
                        break;
                    case 0xF2:
                        Type = MidiEventType.SongPositionPointer;
                        reader.ReadBytes(2);
                        break;
                    case 0xF3:
                        Type = MidiEventType.SongSelect;
                        reader.ReadByte();
                        break;
                    case 0xF6:
                        Type = MidiEventType.TuneRequest;
                        break;
                    case 0xF7:
                        Type = MidiEventType.EndOfExclusive;
                        break;
                    case 0xF8:
                        Type = MidiEventType.TimingClock;
                        break;
                    case 0xFA:
                        Type = MidiEventType.Start;
                        break;
                    case 0xFB:
                        Type = MidiEventType.Continue;
                        break;
                    case 0xFC:
                        Type = MidiEventType.Stop;
                        break;
                    case 0xFE:
                        Type = MidiEventType.ActiveSensing;
                        break;
                    case 0xFF:
                        Type = MidiEventType.Reset;
                        break;
                    default:
                        throw new System.Exception("Unknown midi event, command byte: " + commandByte);
                }
            }
        }
    }
}