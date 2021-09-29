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

        /*Platform Type
         * If 0, the platform will move through the set of points once, then stop. (1 -> 2 -> 3)
         * If 1, it will move through the points in a circle (1 -> 2 -> 3 -> 1 -> 2 -> 3)
         * If 2, it will travel back and forth along the path (1 -> 2 -> 3 -> 2 -> 1)
         */
        public int type = 0; 

        private bool isMoving = false;
        private float wait;
        private int pointIndex = 0;
        private Vector3 nextVector;
        private bool stopMoving = false;

        void Start()
        {
            base.Start();
            movementPointTransforms = path.GetComponentsInChildren<Transform>();
            Transform[] temp = new Transform[movementPointTransforms.Length - 1];
            for (int i = 0; i < movementPointTransforms.Length - 1; i++)
            {
                temp[i] = movementPointTransforms[i + 1];
            }
            movementPointTransforms = temp;

            for (int i = 0; i < path.transform.childCount; i++)
            {
                GameObject child = path.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(false);
                }
            }
            transform.position = movementPointTransforms[0].position;
        }

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
            if (!isPowered) //if not already powered, then become powered and power the system
            {
                isPowered = true;
                isMoving = true;
                nextVector = movementPointTransforms[pointIndex].position - transform.position;

            }
        }

        public override void HandleDeactivated()
        {
            if (!isPowered) 
            {
                isPowered = false;
                isMoving = false;
            }
        }
     
        public void MovePlatform()
        {
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