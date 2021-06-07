using UnityEngine;
using UnityEngine.InputSystem;

namespace Beam.Core.Player
{

    //https://www.youtube.com/watch?v=_QajrabyTJc
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController controller;
        public float speed = 10f;
        public float gravity = -1f;
        public float jumpHeight = 3f;

        public Transform groundCheck;
        public float groundDistance = 0.1f;
        public LayerMask groundMask;

        [SerializeField]
        private Vector3 vel;

        private float deltaX;
        private float deltaZ;

        [SerializeField]
        private bool isGrounded;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            //float deltaX = Input.GetAxis("Horizontal");
            //float deltaZ = Input.GetAxis("Vertical");

            Vector3 deltaPos = (transform.right * deltaX + transform.forward * deltaZ) * speed;

            if (isGrounded)
            {
                if (vel.y < 0)
                {
                    vel.y = 0;
                }
            }
            else
            {
                vel.y += gravity * Time.deltaTime;
            }
            vel.x = deltaPos.x;
            vel.z = deltaPos.z;

            controller.Move(vel * Time.deltaTime);
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            Vector2 moveDir = ctx.ReadValue<Vector2>();
            deltaX = moveDir.x;
            deltaZ = moveDir.y;
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            vel.y = isGrounded ? Mathf.Sqrt(-2f * jumpHeight * gravity) : 0;
        }
    }
}
