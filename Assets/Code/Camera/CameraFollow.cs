using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    private Vector3 offset;

    private void Awake() {
        // get player position. distance from origin is our offset
        GameObject player = GameObject.Find("Player");
        offset = player.transform.position * -1f;
        offset.z = -1f;
    }

    private void FixedUpdate() {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;   
    }
}
