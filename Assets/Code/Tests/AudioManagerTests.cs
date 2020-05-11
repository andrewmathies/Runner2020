using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

using Audio;

namespace Tests {
    public class AudioManagerTests {
        [UnityTest]
        public IEnumerator Audio_Manager_Can_Play_Sounds() {
            GameObject gameManagerPrefab = Resources.Load<GameObject>("Prefabs/GameManager");
            var gameManagerObject = (GameObject) PrefabUtility.InstantiatePrefab(gameManagerPrefab);
            AudioManager audioManager = gameManagerObject.GetComponent<AudioManager>();
            Sound[] sounds = audioManager.Sounds;

            Assert.IsNotEmpty(sounds);

            gameManagerObject.AddComponent<AudioListener>();
            audioManager.Play(sounds[0].Name);

            yield return new WaitForSeconds(5);
        }
    }
}