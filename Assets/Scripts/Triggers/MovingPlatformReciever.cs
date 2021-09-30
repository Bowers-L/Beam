using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Beam.Triggers
{
    public class MovingPlatformReciever : TriggerReceiver
    {

        //moving platform variables
        public GameObject path; //the path for the moving platform to follow
        public Transform[] movementPointTransforms; //the transforms of all of the points, taken at runtime

        public float movespeed = 5.0f; //speed at which platform moves between points
        public float waitTime = 1.0f; //how long to wait at each point

        /*Platform Type
         * If 0, the platform will move through the set of points once, then stop. (1 -> 2 -> 3)
         * If 1, it will move through the points in a circle (1 -> 2 -> 3 -> 1 -> 2 -> 3)
         * If 2, it will travel back and forth along the path (1 -> 2 -> 3 -> 2 -> 1)
         * If 3, this platform will move through the points until deactivated, then it will move back
         */
        public enum Type
        {
            ONEWAY,
            CYCLE,
            DOWNBACK
        }
        public Type type;
        public enum type {LINE, CIRCLE, REVERSE, DOOR};

        //private bool isMoving = false;
        private float wait;
        public int pointIndex = 0;
        //public Vector3 nextVector;
        private bool stopMoving = true;
        Coroutine move;
        public type t = (type) 1;

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

            if (movementPointTransforms.Length == 0)
            {
                Debug.LogError("Moving platform has no path.");
            }
        }

        void Update()
        {
            if (Time.timeScale == 0) return;
            if (transform.position == movementPointTransforms[pointIndex].position)
            {
                if (pointIndex == movementPointTransforms.Length - 1)
                {
                    if (t == (type)3 || t == (type)0)
                    {
                        stopMoving = true;
                    }
                    else if (t == (type) 2)
                    {
                        Array.Reverse(movementPointTransforms);
                    }
                    if (t != (type) 3)
                    {
                        pointIndex = 0;
                    }
                }
                if(!stopMoving)
                {
                    move = StartCoroutine(MovePlatformCoroutine(transform.position, movementPointTransforms[++pointIndex].position));
                }
            }
        }

        public override void HandleActivated()
        {
            stopMoving = false;
        }

        public override void HandleDeactivated()
        {
            StopCoroutine(move);
            stopMoving = true;
            if(t == (type) 3)
            {
                Array.Reverse(movementPointTransforms);
                pointIndex = movementPointTransforms.Length - 1 - pointIndex;
                move = StartCoroutine(MovePlatformCoroutine(transform.position, movementPointTransforms[++pointIndex].position));
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
                    if (type == Type.ONEWAY)
                    {
                        stopMoving = true;
                    }
                    else if (type == Type.DOWNBACK)
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

        IEnumerator MovePlatformCoroutine(Vector3 start, Vector3 target)
        {
            float time = 0f;

            while (transform.position != target)
            {
                transform.position = Vector3.Lerp(start, target, (time / Vector3.Distance(start, target)) * movespeed);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}