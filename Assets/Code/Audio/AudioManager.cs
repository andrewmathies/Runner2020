﻿using System;

using UnityEngine.Audio;
using UnityEngine;

namespace Audio {
    public class AudioManager : MonoBehaviour {

        public Sound[] Sounds;

        void Awake() {
            foreach (Sound s in Sounds) {
                s.Source = gameObject.AddComponent<AudioSource>();
                s.Source.clip = s.Clip;
                s.Source.volume = s.Volume;
                s.Source.pitch = s.Pitch;
            }            
        }

        public void Play(string name) {
            Sound s = Array.Find(Sounds, sound => sound.Name == name);

            if (s == null) {
                Debug.Log("Could not find sound with name: " + name);
                return;
            }

            s.Source.Play();
        }

        public void Stop(string name) {
            Sound s = Array.Find(Sounds, sound => sound.Name == name);

            if (s == null) {
                Debug.Log("Could not find sound with name: " + name);
                return;
            }

            s.Source.Stop();
        }
    }
}
