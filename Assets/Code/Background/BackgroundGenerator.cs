using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player;

namespace Background {
    public class BackgroundGenerator : MonoBehaviour {
        public GameObject[] bgObjects;
        
        public float stepSize;
        public float chanceToPlaceObject;

        private float lastObstaclePosition;
        private float floorY;
        private float ceilingY;

        private Parallaxing p;
        private GameObject foregroundContainer;

        // Start is called before the first frame update
        void Awake()
        {
            p = GetComponent<Parallaxing>();
            foregroundContainer = new GameObject("Foreground Container");

            // get bounds of screen in game coords
            GameObject camObject = GameObject.Find("Main Camera");
            Camera cam = camObject.GetComponent<Camera>();
            float halfSize = cam.orthographicSize;
            
            this.floorY = camObject.transform.position.y - halfSize;
            this.ceilingY = camObject.transform.position.y + halfSize;
        }

        public void SetLastObstaclePosition(float pos) {
            lastObstaclePosition = pos / 2f;
            placeBackgroundObjects();
        }

        private void placeBackgroundObjects() {
            float perlinY = 0;

            foreach (GameObject bgObject in bgObjects) {
                // since we are parallaxing things to the right as the player runs, we want some stuff to the left of start position
                // originally I planned to place things up to the last obstacle position. however because parallaxing moves things
                // to the right, we dont need to make them after a certain point
                for (float x = -20f; x < lastObstaclePosition; x += stepSize) {   
                    float sample = Mathf.PerlinNoise(x, perlinY);
                    perlinY += sample;

                    if (sample > chanceToPlaceObject) {
                        PlaceForegroundObject(x, bgObject);
                    }
                }

                //perlinY += (float.MaxValue / bgObjects.Length) - 1;
            }
        }

        private void PlaceForegroundObject(float xPosition, GameObject objectPrefab) {
            // find the normal height of object, choose randomly between half that and full value
            SpriteRenderer sr = objectPrefab.GetComponent<SpriteRenderer>();
            float height = sr.bounds.size.y;
            float newHeight = Random.Range(0.5f * height, height);
            
            // for some reason, I need to scale all of the foreground stuff by 3.773917 to get it to be the right size
            float scalingFactor = 3.773917f;
            float newYOrigin = objectPrefab.transform.position.y;

            switch(objectPrefab.name) {
                case "stalagmite":
                    newYOrigin = this.floorY + newHeight / 2;
                    scalingFactor *= newHeight / height;
                    break;
                case "stalactite":
                    newYOrigin = this.ceilingY - newHeight / 2;
                    scalingFactor *= newHeight / height;
                    break;
                case "column":
                    break;
                default:
                    Debug.LogError("unexpected foreground prefab name: " + objectPrefab.name);
                    break;
            }

            // place an object at this xPosition
            GameObject foregroundObject = Instantiate(objectPrefab, new Vector3(xPosition, newYOrigin, objectPrefab.transform.position.z), Quaternion.identity, foregroundContainer.transform);
            // resize sprite
            Transform foreGroundObjectTransform = foregroundObject.transform;
            foreGroundObjectTransform.localScale = new Vector3(scalingFactor, scalingFactor, scalingFactor);
            p.AddBackgroundObject(foreGroundObjectTransform);
        }
    }
}