using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Background {
    public class Parallaxing : MonoBehaviour {
        // various backgrounds to be placed in scene
        public Transform[] Backgrounds;
        public Transform Camera;

        // scaling for each background item
        private float[] scales;
        // Smoothing must be > 0
        public float Smoothing;

        private Vector3 previousCamPostion;

        void Start()
        {
            previousCamPostion = Camera.position;

            scales = new float[Backgrounds.Length];

            for (int i = 0; i < Backgrounds.Length; i++) {
                scales[i] = Backgrounds[i].position.z * -1f;
            }
        }

        void Update() {
            for (int i = 0; i < Backgrounds.Length; i++) {
                float parallax = (previousCamPostion.x - Camera.position.x) * scales[i];
                float targetX = Backgrounds[i].position.x + parallax;
                Vector3 targetPosition = new Vector3(targetX, Backgrounds[i].position.y, Backgrounds[i].position.z);

                // transition between cur position and target postion
                Backgrounds[i].position = Vector3.Lerp(Backgrounds[i].position, targetPosition, Smoothing * Time.deltaTime);
            }

            previousCamPostion = Camera.position;
        }
    }
}
/*
public GameObject CreateObstacle(ObstacleType type, Vector3 position) {
            GameObject newObstacleGameObject = Instantiate(obstaclePrefab, 
                                                           position,
                                                           Quaternion.identity,
                                                           obstacleContainer.transform);
            
            Obstacle newObstacle = newObstacleGameObject.GetComponent<Obstacle>();
            newObstacle.Init(type);

            return newObstacleGameObject;
        }
*/