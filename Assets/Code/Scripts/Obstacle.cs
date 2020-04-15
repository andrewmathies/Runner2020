using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Data;

public class Obstacle : MonoBehaviour
{
    public float Speed;

    private ObstacleType type;

    public void Init(ObstacleType t) {
        type = t;
        // TODO: set sprite and collider components in switch statement
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1 * Speed, 0, 0)); 
    }
}
