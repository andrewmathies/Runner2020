using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MovingObjectManager {
    public class MovingObjectManager : MonoBehaviour 
    {
        [SerializeField] private GameObject movingObjectPrefab;
        [SerializeField] private Vector3 newMovingObjectPosition;
        [SerializeField] private Transform movingObjectContainer;
        [SerializeField] private string creationDebugMsg;
        [SerializeField] private float creationInterval;
        private float creationTimer = 0f;

        //Instantiate the appropriate MovingObject
        private void CreateObject() 
        {
            Instantiate(movingObjectPrefab, newMovingObjectPosition, Quaternion.identity, movingObjectContainer);
            
            Debug.Log(creationDebugMsg);
        }

        //Create the object on an interval determined by creationInterval
        void Update()
        {
            creationTimer += 0.01f;
            if (creationTimer >= creationInterval)
            {
                CreateObject();
                creationTimer = 0f;
            }
        }
    }
}