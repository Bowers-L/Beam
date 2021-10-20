using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beam.Core.Player
{
    public class PlayerMoveOnPlat : MonoBehaviour
    {
        public Transform activePlatform;

        CharacterController controller;
        Vector3 moveDirection;
        Vector3 activeGlobalPlatformPoint;
        Vector3 activeLocalPlatformPoint;
        public LayerMask groundMask;

      //  Quaternion activeGlobalPlatformRotation;
      //  Quaternion activeLocalPlatformRotation;
        PlayerMovement pm;
        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();
            pm = GetComponent<PlayerMovement>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!pm.isGrounded)
            {
                activePlatform = null;
            }
            //RaycastHit hit;

           /* if (Physics.Raycast(pm.groundCheck.position, -Vector3.up, out hit, pm.groundDistance, groundMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.gameObject.CompareTag("Platform"))
                {
                   // UpdatePlatform(hit.transform);
                }
                else
                {
                    UpdatePlatform(null);
                }
            }*/

            if (activePlatform != null)
            {
                Vector3 newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
                moveDirection = newGlobalPlatformPoint - activeGlobalPlatformPoint;
                if (moveDirection.magnitude > 0.01f)
                {
                    controller.Move(moveDirection);
                }
                if (activePlatform)
                {
                    /* Support moving platform rotation
                    Quaternion newGlobalPlatformRotation = activePlatform.rotation * activeLocalPlatformRotation;
                    Quaternion rotationDiff = newGlobalPlatformRotation * Quaternion.Inverse(activeGlobalPlatformRotation);
                    // Prevent rotation of the local up vector
                    rotationDiff = Quaternion.FromToRotation(rotationDiff * Vector3.up, Vector3.up) * rotationDiff;
                    transform.rotation = rotationDiff * transform.rotation;
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    */
                    UpdateMovingPlatform();
                }
            }
            else
            {
                if (moveDirection.magnitude > 0.01f)
                {
                    moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, Time.deltaTime);
                    controller.Move(moveDirection);
                }
            }
        }

        public void FixedUpdate()
        {
            
        }


        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // Make sure we are really standing on a straight platform *NEW*
            // Not on the underside of one and not falling down from it either!
            //Debug.Log("hit");
            if (hit.gameObject.CompareTag("Platform"))
            {
                //Debug.Log("hit platform");
                if (hit.moveDirection.y < -0.9 && hit.normal.y > 0.41)
                {
                    if (activePlatform != hit.collider.transform)
                    {
                        UpdatePlatform(hit.collider.transform);
                        /*activePlatform = hit.collider.transform;
                        UpdateMovingPlatform();*/
                    }
                }

            }
            else
            {
                activePlatform = null;
            }

        }

        public void UpdatePlatform(Transform t)
        {
            activePlatform = t;
            if (t == null)
            {
                return;
            }
            activeGlobalPlatformPoint = transform.position;
            activeLocalPlatformPoint = activePlatform.InverseTransformPoint(transform.position);
        }

        void UpdateMovingPlatform()
        {
            activeGlobalPlatformPoint = transform.position;
            activeLocalPlatformPoint = activePlatform.InverseTransformPoint(transform.position);
            // Support moving platform rotation
            //activeGlobalPlatformRotation = transform.rotation;
            //activeLocalPlatformRotation = Quaternion.Inverse(activePlatform.rotation) * transform.rotation;
        }
    }
}
