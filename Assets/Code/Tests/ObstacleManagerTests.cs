using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

using Obstacle;

namespace Tests
{
    public class ObstacleManagerTests
    {
        private ObstacleManager obstacleManager;
        
        [UnityTest]
        public IEnumerator Obstacle_Manager_Can_Create_And_Destroy_Obstacles() {
            obstacleManager.CreateObstacle(ObstacleType.Blue, new Vector3(0, 0, 0));
            GameObject[] objectsWithObstacleTag = GameObject.FindGameObjectsWithTag("Obstacle");

            Assert.That(objectsWithObstacleTag, Has.Property("Length").EqualTo(1));
            Assert.AreEqual(objectsWithObstacleTag[0].GetComponent<Obstacle.Obstacle>().Type, ObstacleType.Blue);

            // the obstacle should delete itself eventually
            yield return new WaitForSeconds(20);

            objectsWithObstacleTag = GameObject.FindGameObjectsWithTag("Obstacle");
            Assert.IsEmpty(objectsWithObstacleTag);
        }
/*
        [UnityTest]
        public IEnumerator Obstacle_Manager_Can_Calculate_Distances() {
            GameObject gridPrefab = Resources.Load<GameObject>("Prefabs/Grid");
            var gridObject = (GameObject) PrefabUtility.InstantiatePrefab(gridPrefab);

            var distances = obstacleManager.calculateDistances();

            Assert.IsEmpty(distances);

            obstacleManager.createObstacle(ObstacleType.Green);

            // wait 5 seconds
            yield return new WaitForSeconds(5);

            // checking that obstacle moved in 5 seconds
            distances = obstacleManager.calculateDistances();
            
            //foreach (KeyValuePair<ObstacleType, int> entry in distances) {
            //    Debug.Log(entry.Key + " " + entry.Value);
            //}
            
            Assert.That(distances[ObstacleType.Green] > 0);
        }
*/
        // since this has the Setup attribue, it will run before every test in this file
        [SetUp]
        public void BeforeEveryTest() {
            GameObject gameManagerPrefab = Resources.Load<GameObject>("Prefabs/GameManager");
            var gameManagerObject = (GameObject) PrefabUtility.InstantiatePrefab(gameManagerPrefab);
            obstacleManager = gameManagerObject.GetComponent<ObstacleManager>();
        }

        // since this has the TearDown attribute, it will run after every test in this file 
        [TearDown]
        public void AfterEveryTest() {
            Object.Destroy(obstacleManager);
            
            var objectsWithObstacleTag = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach(GameObject obstacle in objectsWithObstacleTag) {
                Object.Destroy(obstacle);
            }
        }
    }
}