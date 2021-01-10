using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Audio;
using UnityEngine;

namespace Audio {
    public class AudioManager : MonoBehaviour {

        private Sound[] Sounds;

        void Awake() {
            string[] songPaths = Directory.GetFiles(".\\Assets\\Resources\\Audio\\", "*.mp3", SearchOption.TopDirectoryOnly);
            string[] songNames = new string[songPaths.Length];
            Sounds = new Sound[songPaths.Length];

            for (int i = 0; i < songNames.Length; i++) {
                songNames[i] = Path.GetFileNameWithoutExtension(songPaths[i]);
                Sounds[i] = new Sound();
                Sounds[i].Source = gameObject.AddComponent<AudioSource>();
                AudioClip audioClip = Resources.Load<AudioClip>("Audio\\" + songNames[i]);
                Sounds[i].Source.clip = audioClip;
                Sounds[i].Source.volume = 0.5f;

                // really dumb way of doing this. should move this to a seperate audio game object that doesn't load clips from disk
                if (songNames[i] == "sword-slash-sound") {
                    Sounds[i].Source.volume = 0.15f;
                }

                Sounds[i].Source.pitch = 1f;
                Sounds[i].Name = songNames[i];
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

        public void StopAll() {
            foreach (Sound s in Sounds) {
                s.Source.Stop();
            }
        }
    }
}
