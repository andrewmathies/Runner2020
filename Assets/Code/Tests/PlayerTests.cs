using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerTests
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator Player_Prefab_Has_Player_Tag() {
            var playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
            var player = PrefabUtility.InstantiatePrefab(playerPrefab);
            var objectsWithPlayerTag = GameObject.FindGameObjectsWithTag("Player");
            Assert.That(objectsWithPlayerTag, Has.Property("Length").EqualTo(1));
            yield return null;
        }

        // since this has the TearDown property or whatever, it will run after each other test in this file passes or fails 
        [TearDown]
        public void AfterEveryTest() {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player")) {
                Object.Destroy(obj);
            }
        }
    }
}