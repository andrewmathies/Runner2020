using System;
using System.IO;

using UnityEngine;

namespace Midi {
    public class MetaEvent : Event {

        public uint MicroSecondsPerQuarterNote;
        public MetaEventType Type;

        public MetaEvent(BinaryReader reader) {
            byte metaTypeData = reader.ReadByte();

            //Debug.Log("Meta Event Type:");
            //Utility.PrintHex(metaTypeData);

            switch (metaTypeData) {
                case 0x00:
                    Type = MetaEventType.SequenceNumber;
                    reader.ReadByte();
                    break;
                case 0x20:
                    Type = MetaEventType.MidiChannelPrefix;
                    reader.ReadByte();
                    byte midiChannel = reader.ReadByte();
                    break;
                case 0x2F:
                    Type = MetaEventType.EndOfTrack;
                    reader.ReadByte();
                    //Debug.Log("read end of track meta event!");
                    break;
                case 0x51:
                    Type = MetaEventType.SetTempo;
                    reader.ReadByte();
                    byte[] tempoData = new byte[4];
                    tempoData[3] = 0x00;
                    tempoData[2] = reader.ReadByte();
                    tempoData[1] = reader.ReadByte();
                    tempoData[0] = reader.ReadByte();
                    MicroSecondsPerQuarterNote = BitConverter.ToUInt32(tempoData, 0);
                    //Debug.Log(MicroSecondsPerQuarterNote + " micro seconds per quarter note");
                    break;
                case 0x54:
                    Type = MetaEventType.SMPTEOffset;
                    reader.ReadBytes(6);
                    break;
                case 0x58:
                    Type = MetaEventType.TimeSignature;
                    reader.ReadBytes(5);
                    break;
                case 0x59:
                    Type = MetaEventType.KeySignature;
                    reader.ReadBytes(3);
                    break;
                default:
                    // there are several meta events that follow the pattern: command len data
                    // where data is len bytes long. this case covers all of those meta events
                    if (metaTypeData == 0x7f || (metaTypeData >= 1 && metaTypeData <= 14)) {
                        Type = MetaEventType.Misc;
                        
                        byte lengthData = reader.ReadByte();
                        byte[] buf= new byte[2];
                        buf[0] = lengthData;
                        int length = BitConverter.ToInt16(buf, 0);

                        reader.ReadBytes(length);
                        break;
                    }

                    throw new Exception("unknown type of meta event: " + metaTypeData);
            }
        }
    }
}