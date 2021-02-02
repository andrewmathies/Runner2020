using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Background {
    public class BackgroundGenerator : MonoBehaviour {
        public GameObject stalactite;
        public GameObject stalagmite;
        public GameObject column;
        public GameObject background;

        public Transform Player;

        private float currentBackgroundX = 14.7f;
        private float backgroundWidth = 19.2f;
        private float backgroundY = 0.4f;

        private Parallaxing p;

        // Start is called before the first frame update
        void Start()
        {
            p = GetComponent<Parallaxing>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Player.position.x > currentBackgroundX) {
                currentBackgroundX += backgroundWidth;
                GameObject newBackground = Instantiate(background, new Vector3(currentBackgroundX, backgroundY, 200f), Quaternion.identity);
                p.AddBackground(newBackground.transform);
            }
        }
    }
}

/*
requirements:

place the actual background next to each other so there is never empty space
place other objects psuedo randomly
place them before they appear on screen
*/