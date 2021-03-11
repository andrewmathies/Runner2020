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
        //private float floorY;
        private Parallaxing p;
        private GameObject foregroundContainer;

        // Start is called before the first frame update
        void Awake()
        {
            p = GetComponent<Parallaxing>();
            foregroundContainer = new GameObject("Foreground Container");

            /* get floorY
            GameObject player = GameObject.Find("Player");
            BoxCollider2D collider = player.GetComponent<BoxCollider2D>();
            this.floorY = player.transform.position.y + collider.offset.y - collider.size.y / 2;
            */
        }

        public void SetLastObstaclePosition(float pos) {
            // kinda weird, because paralaxxing moves these objects, we dont need to make them after a point
            lastObstaclePosition = pos / 2f;

            placeBackgroundObjects();
        }

        private void placeBackgroundObjects() {
            float perlinY = 0;

            foreach (GameObject bgObject in bgObjects) {
                // step through 0 to last obstacle position in increments
                for (float x = 0f; x < lastObstaclePosition; x += stepSize) {   
                    float sample = Mathf.PerlinNoise(x, perlinY);
                    perlinY += sample;

                    if (sample > chanceToPlaceObject) {
                        // place an object at this x
                        GameObject backgroundObject = Instantiate(bgObject, new Vector3(x, 0.09f, bgObject.transform.position.z), Quaternion.identity, foregroundContainer.transform);
                        p.AddBackgroundObject(backgroundObject.transform);
                    }
                }

                //perlinY += (float.MaxValue / bgObjects.Length) - 1;
            }
        }
    }
}