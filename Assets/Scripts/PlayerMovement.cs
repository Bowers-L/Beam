using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 deltaPos = (transform.right * x + transform.forward * z) * speed;

        if (isGrounded)
        {
            if (vel.y < 0)
            {
                vel.y = 0;
            }

            if (Input.GetButtonDown("Jump"))
            {
                vel.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
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
}
