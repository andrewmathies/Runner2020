using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour 
{
    protected float baseSpeed = 1f;
    protected Rigidbody2D rigidComponent;

    private float leftEdgeOfScreen = -12f;

    //Use a Property for Speed so that child classes can override it
    protected virtual float Speed
    {
        get { return baseSpeed; }
    }

    //Set the Rigidbody2D's velocity component to the desired value to move the object
    protected void SetVelocity()
    {
        rigidComponent = gameObject.GetComponent<Rigidbody2D>();
        rigidComponent.velocity = Speed*Vector3.left;
    }

    void Start()
    {
        SetVelocity();
    }

    //Delete the MovingObjects after they have passed off the left edge of the screen
    void Update()
    {
        if (transform.position.x <= (leftEdgeOfScreen-2)) {
            Destroy(gameObject);
        }
    }
}