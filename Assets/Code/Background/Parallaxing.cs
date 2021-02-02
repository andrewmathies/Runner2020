using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Background {
    public class Parallaxing : MonoBehaviour {
        private Dictionary<Transform, float> BackgroundScales;
        public Transform Camera;

        // Smoothing must be > 0
        public float Smoothing;
        private Vector3 previousCamPostion;

        void Start()
        {
            previousCamPostion = Camera.position;
            BackgroundScales = new Dictionary<Transform, float>();
            /*
            scales = new float[Backgrounds.Length];

            for (int i = 0; i < Backgrounds.Length; i++) {
                scales[i] = Backgrounds[i].position.z * -1f;
            }
            */
        }

        void Update() {
            /*
            for (int i = 0; i < Backgrounds.Length; i++) {
                float parallax = (previousCamPostion.x - Camera.position.x) * scales[i];
                float targetX = Backgrounds[i].position.x + parallax;
                Vector3 targetPosition = new Vector3(targetX, Backgrounds[i].position.y, Backgrounds[i].position.z);

                // transition between cur position and target postion
                Backgrounds[i].position = Vector3.Lerp(Backgrounds[i].position, targetPosition, Smoothing * Time.deltaTime);
            }
            */
            foreach (Transform bg in BackgroundScales.Keys) {
                float scale = BackgroundScales[bg];
                float parallax = (previousCamPostion.x - Camera.position.x) * scale;
                float targetX = bg.position.x + parallax;
                Vector3 targetPosition = new Vector3(targetX, bg.position.y, bg.position.z);

                // transition between cur position and target postion
                bg.position = Vector3.Lerp(bg.position, targetPosition, Smoothing * Time.deltaTime);
            }

            previousCamPostion = Camera.position;
        }

        public void AddBackground(Transform bg) {
            BackgroundScales[bg] = bg.position.z * -1f;
        }
    }
}

/*



in other script, generate bg objects. then add them to structure in this script

threshold is the length from character s.t. something is off screen

on each frame:
    for each object in structure:
        if object.x - player.x > threshold:
            move object
        else:
            remove object from the structure

*/