using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    public sealed class DataStore {
        private static readonly DataStore instance = new DataStore();
        
        private AdjacencyRules rules = new AdjacencyRules();
        private Dictionary<ObstacleType, Texture2D> obstacleTextures = new Dictionary<ObstacleType, Texture2D>();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static DataStore()
        {
        }

        private DataStore()
        {
            initTextures();
        }

        public static DataStore Instance {
            get { return instance; }
        }

        public AdjacencyRules Rules {
            get { return rules; }
        }

        public Dictionary<ObstacleType, Texture2D> ObstacleTextures {
            get { return obstacleTextures; }
        }

        private void initTextures() {
            Texture2D blackTexture = Resources.Load<Texture2D>("Textures/black");
            Texture2D blueTexture = Resources.Load<Texture2D>("Textures/blue");
            Texture2D greenTexture = Resources.Load<Texture2D>("Textures/green");

            obstacleTextures.Add(ObstacleType.Black, blackTexture);
            obstacleTextures.Add(ObstacleType.Blue, blueTexture);
            obstacleTextures.Add(ObstacleType.Green, greenTexture);
        }
    }
}