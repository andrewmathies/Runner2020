using System;
using System.IO;

using UnityEngine;

namespace Midi {
    public class Parser : MonoBehaviour {

        public MidiFile Song;

        public void Start() {
            Debug.Log("starting to parse midi file");
            Song = new MidiFile("D:\\Programming\\Unity Projects\\Runner2020\\Assets\\Code\\Midi\\test.mid");
        }
    }
}
