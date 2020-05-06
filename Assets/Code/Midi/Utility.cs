using System;
using System.Text;
using System.Collections;

using UnityEngine;

namespace Midi {
    public static class Utility {
        public static void PrintBytes(byte[] data) {
            StringBuilder sb = new StringBuilder("bytes:\n");

            foreach (byte b in data) {
                sb.Append((uint) b + " ");
            }

            Debug.Log(sb.ToString());
        }

        public static void PrintBits(byte[] data) {
            StringBuilder sb = new StringBuilder("bits:\n");
            BitArray bits = new BitArray(data);

            for (int i = 0; i < bits.Count; i++) {
                if (i % 8 == 0) {
                    sb.Append("\n");
                }

                sb.Append(bits[i] + " ");
            }

            Debug.Log(sb.ToString());
        }

        public static void PrintHex(byte b) {
            byte[] temp = new byte[1];
            temp[0] = b;
            string byteAsHexString = BitConverter.ToString(temp).Replace("-","");
            Debug.Log("byte as hex: " + byteAsHexString);
        }
    }
}