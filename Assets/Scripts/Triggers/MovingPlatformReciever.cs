using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Beam.Triggers
{
    public class MovingPlatformReciever : TriggerReceiver
    {

        public bool isPowered = false; 

        //moving platform variables
        public GameObject path; //the path for the moving platform to follow
        public Transform[] movementPointTransforms; //the transforms of all of the points, taken at runtime

        public float movespeed = 5.0f; //speed at which platform moves between points
        public float waitTime = 1.0f; //how long to wait at each point
        public int type = 0; //if 0, the platform will move through the set of points once, then stop. If 1, it will move through the points in a circle (1 -> 2 -> 3 -> 1 -> 2 -> 3), If 2, it will travel back and forth along the path (1 -> 2 -> 3 -> 2 -> 1)

        bool isMoving = false;
        float wait;
        public int pointIndex = 0;
        Vector3 nextVector;
        public bool stopMoving = false;


        // Start is called before the first frame update

        void Start()
        {
            base.Start();
            movementPointTransforms = path.GetComponentsInChildren<Transform>();
            Transform[] temp = new Transform[movementPointTransforms.Length - 1];
            for (int i = 0; i < movementPointTransforms.Length - 1; i++)
            {
                temp[i] = movementPointTransforms[i + 1];
                Debug.Log(i);
            }
            movementPointTransforms = temp;

            for (int i = 0; i < path.transform.childCount; i++)
            {
                var child = path.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(false);
                }
            }
            transform.position = movementPointTransforms[0].position;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0) return;
            if (isPowered)
            {
                if (!stopMoving)
                {
                    MovePlatform();
                }
            }
        }
        public override void HandleActivated()
        {
            Debug.Log("Activate Method Running");
            if (!isPowered) //if not already powered, then become powered and power the system
            {
                isPowered = true;
                isMoving = true;
                nextVector = movementPointTransforms[pointIndex].position - transform.position;

            }
        }

        public override void HandleDeactivated()
        {
            Debug.Log("Deactivate Method Running");
            if (!isPowered) //if not already powered, then become powered and power the system
            {
                isPowered = false;
                isMoving = false;
            }
        }
     
        public void MovePlatform()
        {
            Debug.Log("Move");
            if (isMoving)
            {
                transform.Translate(nextVector * Time.deltaTime * movespeed * 0.1f);
            }
            if (Vector3.Distance(movementPointTransforms[pointIndex].position, transform.position) < 0.1f)
            {
                isMoving = false;
                pointIndex++;
                wait = waitTime;
                if (pointIndex > movementPointTransforms.Length - 1)
                {
                    if (type == 0)
                    {
                        stopMoving = true;
                    }
                    else if (type == 2)
                    {
                        Array.Reverse(movementPointTransforms);
                    }
                    pointIndex = 0;
                }
            }
            if (!isMoving)
            {
                wait -= Time.deltaTime;
                if (wait < 0)
                {
                    isMoving = true;
                    nextVector = movementPointTransforms[pointIndex].position - transform.position;

                }

            }

        }
    }
}