using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Data;

public class ObstacleManager : MonoBehaviour
{
    public GameObject player;
    public GameObject obstaclePrefab;
    public Vector3 newObstaclePosition;
    public Transform obstacleContainer;

    // if we decide an enum is appropriate for difficulty selection, we can make this public and extend editor UI to
    // use correct enum types in inspector. reference comment by aqibsadiq
    // at https://forum.unity.com/threads/multiple-enum-select-from-inspector.184729/
    private Difficulty difficulty;
    private AdjacencyRules rules;
    private System.Random numberGenerator;
    private Queue<Obstacle> obstacles;

    // Start is called before the first frame update
    void Start()
    {
        obstacles = new Queue<Obstacle>();
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

        // in this case, do not generate an obstacle
        return null;
    }

    // returns distances to the closest obstacle of each type in cells
    private Dictionary<ObstacleType, int> calculateDistances() {
        Dictionary<ObstacleType, int> distances = new Dictionary<ObstacleType, int>();
        Grid grid = GameObject.FindObjectOfType(typeof(Grid)) as Grid;

        // iterate through all obstacles in the scene, starting with oldest
        // i.e. farthest from the right side of the screen
        foreach (Obstacle obstacle in obstacles) {
            ObstacleType type = obstacle.Type;
            
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
    // create it under the obstaclecontainer in the scene hierarchy
    private Obstacle createObstacle(ObstacleType type) {
        GameObject newObstacleGameObject = Instantiate(obstaclePrefab, newObstaclePosition, Quaternion.identity, obstacleContainer);
        Obstacle newObstacle = newObstacleGameObject.GetComponent<Obstacle>();
        newObstacle.Init(type);
        return newObstacle;
    }

    // this is called by the player every time it "runs the length of a new cell"
    public void NextCell() {
        Dictionary<ObstacleType, int> distances = calculateDistances();

        // maybe this has empty elements that mean do not generate an obstacle. fill it with different amounts
        // of obstacles depending on how rare they are?
        List<ObstacleType> potentialObstacles = rules.Apply(distances);
        ObstacleType? obstacle = obstacleDecider(difficulty, potentialObstacles);

        if (obstacle is ObstacleType newObstacleType) {
            Obstacle newObstacle = createObstacle(newObstacleType);
            Debug.Log("Enqueueing a new obstacle: " + newObstacle.Type);
            obstacles.Enqueue(newObstacle);
        }
    }

    // Remove the oldest obstacle from our queue
    // Are we going to have obstacles that move at different speeds? if so then we can't just blindly assume
    // the oldest thing in the queue is what we need to delete. TODO: change queue to dict with unique ids for each obstacle
    public void RemoveObstacle() {
        obstacles.Dequeue();
    }
}