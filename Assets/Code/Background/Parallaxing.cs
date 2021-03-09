using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Background {
    public class Parallaxing : MonoBehaviour {        
        public Transform Camera;
        public float Smoothing;

        private Dictionary<Transform, float> BackgroundObjects = new Dictionary<Transform, float>();
        private Vector3 previousCamPostion;
        
        void Start()
        {
            previousCamPostion = Camera.position;
        }

        void FixedUpdate() {
            foreach (KeyValuePair<Transform, float> entry in BackgroundObjects) {
                Transform bg = entry.Key;
                float scale = entry.Value;

                float parallax = (previousCamPostion.x - Camera.position.x) * scale;
                float targetX = bg.position.x + parallax;
                Vector3 targetPosition = new Vector3(targetX, bg.position.y, bg.position.z);

                // transition between cur position and target postion
                bg.position = Vector3.Lerp(bg.position, targetPosition, Smoothing * Time.deltaTime);
            }

            previousCamPostion = Camera.position;
        }

        public void AddBackgroundObject(Transform bg) {
            BackgroundObjects[bg] = bg.position.z * -1f;
        }
    }
}