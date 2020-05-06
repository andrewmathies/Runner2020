using System;
using System.IO;
using System.Collections;

using UnityEngine;

namespace Midi {
    public class Chunk {
        public string Type;
        public uint Length;
        public byte[] Data;

        public Chunk(BinaryReader reader) {
            char[] typeCharArray = reader.ReadChars(4);
            Type = new string(typeCharArray);

            byte[] lengthData = reader.ReadBytes(4);
            Array.Reverse(lengthData);
            Length = BitConverter.ToUInt16(lengthData, 0);
            
            Data = reader.ReadBytes((int) Length);
        }
    }
}