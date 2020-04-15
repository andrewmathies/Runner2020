using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Data;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject blackObstacle, blueObstacle, greenObstacle;
    public Vector3 newObstaclePosition;
    public Transform obstacleContainer;

    // if we decide an enum is appropriate for difficulty selection, we can make this public and extend editor UI to
    // use correct enum types in inspector. reference comment by aqibsadiq
    // at https://forum.unity.com/threads/multiple-enum-select-from-inspector.184729/
    private Difficulty difficulty;
    private AdjacencyRules rules;
    private System.Random numberGenerator;
    private Queue<GameObject> obstacles;
    private Dictionary<GameObject, ObstacleType> typeDict;

    // Start is called before the first frame update
    void Start()
    {
        obstacles = new Queue<GameObject>();
        typeDict = new Dictionary<GameObject, ObstacleType>();
        numberGenerator = new System.Random();
        difficulty = Difficulty.Easy;

        DataStore datastore = DataStore.Instance;
        rules = datastore.Rules;
    }

    // return a randomly selected obstacle type (or null which means no obstacle). right now probability
    // that a particular type is generated is decided by difficulty enum. should this be a number that slowly
    // ramps up? should chance to generate one type change if one or more types are not in the options?
    private ObstacleType? obstacleDecider(Difficulty difficulty, List<ObstacleType> options) {
        float chanceOfEachObstacle;

        switch (difficulty) {
            case Difficulty.Easy:
                chanceOfEachObstacle = .05f;
                break;
            case Difficulty.Medium:
                chanceOfEachObstacle = .1f;
                break;
            case Difficulty.Hard:
                chanceOfEachObstacle = .15f;
                break;
            default:
                throw new System.ArgumentException("Invalid difficulty: " + difficulty);
        }

        foreach (ObstacleType type in options) {
            double d = numberGenerator.NextDouble();
            if (d < chanceOfEachObstacle) {
                return type;
            }
        }

        return null;
    }

    // returns distances to the closest obstacle of each type in cells. 
    private Dictionary<ObstacleType, int> calculateDistances() {
        Dictionary<ObstacleType, int> distances = new Dictionary<ObstacleType, int>();
        Grid grid = GameObject.FindObjectOfType(typeof(Grid)) as Grid;

        foreach (GameObject obstacle in obstacles) {
            ObstacleType type = typeDict[obstacle];
            
            if (distances.ContainsKey(type)) {
                distances.Remove(type);
            }
            
            Vector3Int playerCellPosition = grid.WorldToCell(player.transform.position);
            Vector3Int obstacleCellPosition = grid.WorldToCell(obstacle.transform.position);
            int cellDist = (int) Math.Abs(playerCellPosition.x - obstacleCellPosition.x);
            distances.Add(type, cellDist);
        }

        return distances;
    }

    // returns a new game object to be placed in the scene as a new obstacle
    // TODO: instead of switch, have init function that can be called on the newly made object that takes obstacle type as paramater
    private GameObject createObstacle(ObstacleType type) {
        GameObject prefab;
        
        switch (type) {
            case ObstacleType.Black:
                prefab = blackObstacle;
                break;
            case ObstacleType.Blue:
                prefab = blueObstacle;
                break;
            case ObstacleType.Green:
                prefab = greenObstacle;
                break;
            default:
                throw new System.ArgumentException("Invalid obstacle type: " + type);
        }
        
        return Instantiate(prefab, newObstaclePosition, Quaternion.identity, obstacleContainer);
    }

    // Remove the oldest cell from our queue, and create a new cell. Decide what kind of object should be in the new cell. 
    public void GenerateNext() {
        Dictionary<ObstacleType, int> distances = calculateDistances();

        // maybe this has empty elements that mean do not generate an obstacle. fill it with different amounts
        // of obstacles depending on how rare they are?
        List<ObstacleType> potentialObstacles = rules.Apply(distances);
        ObstacleType? obstacle = obstacleDecider(difficulty, potentialObstacles);

        if (obstacle is ObstacleType newObstacleType) {
            Debug.Log("Enqueueing a new obstacle: " + newObstacleType);
            GameObject newObstacle = createObstacle(newObstacleType);
            obstacles.Enqueue(newObstacle);
            typeDict.Add(newObstacle, newObstacleType);
        }
    }
}