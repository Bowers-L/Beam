using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Beam.Core.Player;

namespace Beam.Triggers

{
    public class MultiStageMovingPlatformReceiver : TriggerReceiver
    {
        //moving platform variables
        public GameObject path; //the path for the moving platform to follow
        public Transform[] movementPointTransforms; //the transforms of all of the points, taken at runtime
        public float movespeed = 5.0f; //speed at which platform moves between points

        /*Platform Type
         * This is a special type of moving platform whose position is dependent on the number of activated triggers.
         * IE The platform is at position 0 by default. If it is activated once, then it will move to position 1. 
         * If it is activated again, it will move 2 position 2. If it is deactivated at this point, it will move back
         * to position 1, etc.
         */

        public int pointIndex = 0;
        public int targetIndex = 0;
        private Vector3 orignialPos;
        private Rigidbody rBody;
        public Coroutine move;
        PlayerMoveOnPlat pmp;

        void Awake()
        {
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
            if(transform.position != movementPointTransforms[targetIndex].position) //not at rest
            {
                if (move == null)
                {
                    move = StartCoroutine(MovePlatformCoroutine(transform.position, movementPointTransforms[targetIndex].position));
                }
                //if (transform.position == movementPointTransforms[targetIndex].position)
                //{
               //     pointIndex = targetIndex;
              //  }
            }
            else if (pointIndex != targetIndex)
            {
                pointIndex = targetIndex;
                if(move != null)
                {
                    StopCoroutine(move);
                    move = null;
                }
            }
            
        }

        public override void HandleActivated()
        {
            Debug.Log("Activate");
            UpdateTarget(true);
            if (move != null)
            {
                StopCoroutine(move);
                move = null;
            }
            /*stopMoving = false;
            if (move == null)
            {
                move = StartCoroutine(MovePlatformCoroutine(transform.position, movementPointTransforms[++pointIndex].position));
            }*/
        }

        public override void HandleDeactivated()
        {
            Debug.Log("DeActivate");
            UpdateTarget(false);
            if (move != null)
            {
                StopCoroutine(move);
                move = null;
            }
            /*if (type == Type.DOOR)
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
            }*/
        }

        void UpdateTarget(Boolean increase)
        {
            if (increase)
            {
                if(targetIndex < movementPointTransforms.Length - 1)
                {
                    targetIndex++;
                }
            }
            else
            {
                if(targetIndex > 0)
                {
                    targetIndex--;
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

       public void OnTriggerEnter(Collider other)
        {
            Debug.Log("enter "+other.gameObject.name);
            if(other.gameObject.CompareTag("Player"))
            {
                pmp = other.gameObject.GetComponent<PlayerMoveOnPlat>();
                pmp.UpdatePlatform(this.transform);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            Debug.Log("Exit "+other.gameObject.name);
            if (other.gameObject.CompareTag("Player"))
            {
                pmp = other.gameObject.GetComponent<PlayerMoveOnPlat>();
                pmp.UpdatePlatform(null);
                //StartCoroutine(removePlayer(other));
            }
        }

        /*public IEnumerator removePlayer(Collider other)
        {
            yield return new WaitForSeconds(0.1f);
            pmp = other.gameObject.GetComponent<PlayerMoveOnPlat>();
            pmp.UpdatePlatform(null);
        }*/
    }
}