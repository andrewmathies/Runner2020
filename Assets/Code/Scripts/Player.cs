using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed, CellWidth;

    private ObstacleManager obstacleManager;
    private float sim;

    // Start is called before the first frame update
    void Start()
    {
        sim = 0;
        obstacleManager = GameObject.FindObjectOfType(typeof(ObstacleManager)) as ObstacleManager;
    
        Debug.Log("Player is initialized");
    }

    // Update is called once per frame
    void Update()
    {
        sim += Speed * Time.deltaTime;

        if (sim > CellWidth) {
            //Debug.Log("Player ran the length of a cell");
            obstacleManager.NextCell();
            sim = 0;
        }
    }
}
