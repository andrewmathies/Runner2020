using System.IO;

using UnityEngine;

namespace Midi {
    public static class Extensions {

        private const byte MSBTrue = 0b10000000;
        private const byte MASK = 0b01111111;

        public static ulong DecodeUInt64(this BinaryReader reader) {
            bool moreData = true;
            byte[] storage = new byte[4];
            int counter = 0;
            
            while (moreData) {
                byte otherBits = reader.ReadByte();
                storage[counter++] = (byte) (otherBits & MASK);
                moreData = (otherBits & MSBTrue) != 0;
            }

            ulong value = 0;
            int shift = 0;

            for (int i = counter - 1; i >= 0; i--) {
                value |= (ulong) (storage[i] << shift);
                shift += 7;
            }

            return value;
        }

        public static void ReadUntil(this BinaryReader reader, byte end) {
            byte? data = null;
            
            while (data != end) {
                data = reader.ReadByte();
            }
        }
    }
}