using UnityEngine;
using UnityEngine.InputSystem;

namespace Beam.Core.Player
{

    //https://www.youtube.com/watch?v=_QajrabyTJc
    public class PlayerMovement : MonoBehaviour
    {

        [System.Serializable]
        public struct MovementParameters
        {
            public float maxMoveSpeed;

            public float accel;

            //raw movement from player's input
            //x = right movement, y = forward movement
            public Vector2 rawMoveInput {
                get;
                internal set;
            }
        }

        public CharacterController controller;
        
        //might need to put gravity in an external game manager.
        public float gravity = 1f;
        public float jumpHeight = 3f;

        public Transform groundCheck;
        public float groundDistance = 0.1f;
        public LayerMask groundMask;

        public float forceMag = 10.0f;  //used for physics when the player collides with a rigidbody.

        [SerializeField]
        private Vector3 vel;

        [SerializeField]
        private MovementParameters moveParams;


        [SerializeField]
        private bool isGrounded
        {
            get
            {
                return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //float deltaX = Input.GetAxis("Horizontal");
            //float deltaZ = Input.GetAxis("Vertical");

            Vector3 moveVel = calcMovementVel();

            vel.x = moveVel.x;
            vel.z = moveVel.z;

            if (isGrounded)
            {
                if (vel.y < 0)
                {
                    vel.y = 0;
                }
            }
            else
            {
                vel.y -= gravity * Time.deltaTime;
            }


            controller.Move(vel * Time.deltaTime);
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            moveParams.rawMoveInput = ctx.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                vel.y = isGrounded ? Mathf.Sqrt(2f * jumpHeight * gravity) : vel.y;
            }

            if (ctx.canceled && vel.y > 0f)
            {
                vel.y = 0f;
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //Implement collision physics manually because character controller doesn't come with rigidbody.
            if (hit.rigidbody != null) {
                hit.rigidbody.AddForceAtPosition(vel * forceMag, hit.point);
            }
        }

        private Vector3 calcMovementVel()
        {
            //Outputs the final movement of the player based on input, taking into account smoothing, player rotation, etc.
            Vector3 deltaVelUnit = Vector3.Normalize((transform.right * moveParams.rawMoveInput.x + transform.forward * moveParams.rawMoveInput.y));

            Vector3 newMoveVel = deltaVelUnit * moveParams.accel * Time.deltaTime + new Vector3(vel.x, 0, vel.z);
            return Vector3.Normalize(newMoveVel) * Mathf.Min(newMoveVel.magnitude, moveParams.maxMoveSpeed);
        }
        
    }
}
