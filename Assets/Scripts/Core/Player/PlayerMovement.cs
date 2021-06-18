using UnityEngine;
using UnityEngine.InputSystem;

namespace Beam.Core.Player
{

    //https://www.youtube.com/watch?v=_QajrabyTJc
    public class PlayerMovement : MonoBehaviour
    {
        //Parameters used for calculated the velocity
        [System.Serializable]
        public struct MovementParameters
        {
            public float maxMoveSpeed;

            public float accel;

            //Controls how fast the player slows down when exceeding the speed cap.
            public float overCapSmoothing;

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

            updateVelocity();

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

            /* Don't know if this should be kept since there's problems with external forces.
            if (ctx.canceled && vel.y > 0f)
            {
                //release jump button and player is still moving upwards.
                vel.y = 0f;
            }
            */
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //Implement collision physics manually because character controller doesn't come with rigidbody.
            if (hit.rigidbody != null) {
                hit.rigidbody.AddForceAtPosition(vel * forceMag, hit.point);
            }
        }

        //Updates the player's velocity, taking into account smoothing, player rotation, input, etc.
        private void updateVelocity()
        {

            Vector3 moveAccel = moveParams.accel * Vector3.Normalize((transform.right * moveParams.rawMoveInput.x + transform.forward * moveParams.rawMoveInput.y));

            //determines the max xz speed that the player can have 
            float playerSpeedCap = moveParams.maxMoveSpeed * moveParams.rawMoveInput.magnitude;

            //update player's y velocity based on gravity
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

            Vector3 oldXZVel = new Vector3(vel.x, 0, vel.z);
            Vector3 newXZVel = moveAccel * Time.deltaTime + oldXZVel;
            Vector3 newVelDir = Vector3.Normalize(newXZVel);

            float newVelMagAdjusted;
            if (newXZVel.magnitude > playerSpeedCap)
            {
                newVelMagAdjusted = Mathf.Lerp(oldXZVel.magnitude, playerSpeedCap, moveParams.overCapSmoothing);
            } else
            {
                newVelMagAdjusted = Mathf.Min(newXZVel.magnitude, playerSpeedCap);
            }

            newXZVel = newVelDir * newVelMagAdjusted;
            vel.x = newXZVel.x;
            vel.z = newXZVel.z;
        }
    }
}
