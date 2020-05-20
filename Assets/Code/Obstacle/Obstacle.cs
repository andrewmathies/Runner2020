using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Obstacles;

namespace MovingObjects 
{
    public class Obstacle : Foreground 
    {
        public ObstacleType Type;

        private SpriteRenderer sr;
        private ObstacleManager obstacleManager;

        // create a sprite renderer component, get reference to global data store, get reference to obstacle manager
        void Awake()
        {
            sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
            obstacleManager = GameObject.FindObjectOfType(typeof(ObstacleManager)) as ObstacleManager;
        }

        // set the type of obstacle and sprite accordingly
        public void Init(ObstacleType t) 
        {
            Type = t;

            Dictionary<ObstacleType, Texture2D> obstacleTextureMap = obstacleManager.ObstacleTextures;
            
            Texture2D texture = obstacleTextureMap[t];
            //Debug.Log(texture);
            Rect rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            Vector2 position = new Vector2(0.0f, 0.0f);
            sr.sprite = Sprite.Create(texture, rect, position);
            // change sorting layer. if this string is not a sorting layer, sets to default sorting layer
            sr.sortingLayerName = "Obstacles";
        }
    }
}