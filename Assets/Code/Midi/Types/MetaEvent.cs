using System;
using System.IO;

using UnityEngine;

namespace Midi {
    public class MetaEvent : Event {
        public MetaEvent(BinaryReader reader) {
            byte metaTypeData = reader.ReadByte();

            //Debug.Log("Meta Event Type:");
            //Utility.PrintHex(metaTypeData);

            switch (metaTypeData) {
                case 0x00:
                    // sequence number
                    reader.ReadByte();
                    break;
                case 0x20:
                    // midi channel prefix
                    reader.ReadByte();
                    byte midiChannel = reader.ReadByte();
                    break;
                case 0x2F:
                    // end of track
                    reader.ReadByte();
                    //Debug.Log("read end of track meta event!");
                    break;
                case 0x51:
                    // set tempo
                    reader.ReadBytes(4);
                    break;
                case 0x54:
                    // SMPTE offset
                    reader.ReadBytes(6);
                    break;
                case 0x58:
                    // time signature
                    reader.ReadBytes(5);
                    break;
                case 0x59:
                    // key signature
                    reader.ReadBytes(3);
                    break;
                default:
                    // there are several meta events that follow the pattern: command len data
                    // where data is len bytes long. this case covers all of those meta events
                    if (metaTypeData == 0x7f || (metaTypeData >= 1 && metaTypeData <= 14)) {
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