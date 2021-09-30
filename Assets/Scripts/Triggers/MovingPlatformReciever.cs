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
         * If OneWay, the platform will move through the set of points once, then stop. (1 -> 2 -> 3)
         * If Cycle, it will move through the points in a circle (1 -> 2 -> 3 -> 1 -> 2 -> 3)
         * If DownBack, it will travel back and forth along the path (1 -> 2 -> 3 -> 2 -> 1)
         * If Door, this platform will move through the points until deactivated, then it will move back
         */
        public enum Type
        {
            ONEWAY,
            CYCLE,
            DOWNBACK,
            DOOR
        }
        public Type type;


        private float wait;
        private int pointIndex = 0;
        private bool stopMoving = true;
        private Vector3 orignialPos;
        private Rigidbody rBody;
        Coroutine move;
        void Start()
        {
            base.Start();
            wait = waitTime;
            rBody = GetComponent<Rigidbody>();

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
            orignialPos = transform.position;
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
                    switch (type)
                    {
                        case Type.ONEWAY:
                            stopMoving = true;
                            break;
                        case Type.DOWNBACK:
                            Array.Reverse(movementPointTransforms);
                            pointIndex = 0;
                            break;
                        case Type.DOOR:
                            stopMoving = true;
                            if (transform.position == orignialPos)
                            {
                                Array.Reverse(movementPointTransforms);
                                pointIndex = 0;
                            }
                            break;
                    }
                }
                if (!stopMoving)
                {
                    wait -= Time.deltaTime;
                    if (wait <= 0)
                    {
                        pointIndex = ++pointIndex % movementPointTransforms.Length;
                        move = StartCoroutine(MovePlatformCoroutine(transform.position, movementPointTransforms[pointIndex].position));
                        wait = waitTime;
                    }
                }
            }
        }

        public override void HandleActivated()
        {
            stopMoving = false;
            if (move == null)
            {
                move = StartCoroutine(MovePlatformCoroutine(transform.position, movementPointTransforms[++pointIndex].position));
            }
        }

        public override void HandleDeactivated()
        {
            if (move != null)
            {
                StopCoroutine(move);
                move = null;
            }
            if (type == Type.DOOR)
            {
                if (pointIndex == movementPointTransforms.Length - 1 || !stopMoving)
                {
                    Array.Reverse(movementPointTransforms);
                    pointIndex = movementPointTransforms.Length - 1 - pointIndex;
                    move = StartCoroutine(MovePlatformCoroutine(transform.position, movementPointTransforms[++pointIndex].position));
                    stopMoving = false;
                }
            }
            else
            {
                stopMoving = true;
                if (pointIndex > 0)
                {
                    pointIndex--;
                }
            }
        }


        IEnumerator MovePlatformCoroutine(Vector3 start, Vector3 target)
        {
            float time = 0f;

            while (transform.position != target)
            {
                rBody.MovePosition(Vector3.Lerp(start, target, (time / Vector3.Distance(start, target)) * movespeed));
                time += Time.deltaTime;
                yield return null;
            }
        }

        /*public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                PlayerMoveOnPlat = other.gameObject.Player
            }
        }*/
    }
}