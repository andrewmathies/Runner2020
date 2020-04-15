using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed, CellWidth;

    private ObstacleGenerator gen;
    private float sim;

    // Start is called before the first frame update
    void Start()
    {
        sim = 0;
        gen = GameObject.FindObjectOfType(typeof(ObstacleGenerator)) as ObstacleGenerator;
    
        Debug.Log("Player is initialized");
    }

    // Update is called once per frame
    void Update()
    {
        sim += Speed * Time.deltaTime;

        if (sim > CellWidth) {
            Debug.Log("Hit a new cell!");
            gen.GenerateNext();
            sim = 0;
        }
    }
}
