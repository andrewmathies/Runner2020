using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Data;

namespace Obstacle {
    public class Obstacle : MonoBehaviour {
        public float Speed;
        public ObstacleType Type;

        private float edgeOfScreen = -12f;
        private SpriteRenderer sr;
        private DataStore dataStore;
        private ObstacleManager obstacleManager;

        // create a sprite renderer component, get reference to global data store, get reference to obstacle manager
        void Awake()
        {
            sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
            dataStore = DataStore.Instance;
            obstacleManager = GameObject.FindObjectOfType(typeof(ObstacleManager)) as ObstacleManager;
        }

        // set the type of obstacle and sprite accordingly
        public void Init(ObstacleType t) {
            Type = t;

            Dictionary<ObstacleType, Texture2D> obstacleTextureMap = dataStore.ObstacleTextures;
            
            Texture2D texture = obstacleTextureMap[t];
            Debug.Log(texture);
            Rect rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            Vector2 position = new Vector2(0.0f, 0.0f);
            sr.sprite = Sprite.Create(texture, rect, position);
            // change sorting layer. if this string is not a sorting layer, sets to default sorting layer
            sr.sortingLayerName = "Obstacles";
        }

        // Update is called once per frame
        // If we hit the left edge of the screen, delete this obstacle. otherwise keep moving move left
        void Update()
        {
            if (transform.position.x <= edgeOfScreen) {
                obstacleManager.RemoveObstacle();
                Destroy(gameObject);
            }

            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }
    }
}