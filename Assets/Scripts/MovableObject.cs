using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovableObject : MonoBehaviour
{
    private Rigidbody rbody;
    private bool isOnPlatform;
    private Rigidbody platformRBody;

    //private Transform parent;
    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        /*
        if (isOnPlatform)
        {
            rbody.velocity = platformRBody.velocity;
        }
        */
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Platform")
        {
            Debug.Log(col.gameObject);
            platformRBody = col.gameObject.GetComponent<Rigidbody>();
            col.transform.parent = platformRBody.transform;
            isOnPlatform = true;
        }
 
    }

    void OnCollisionExit(Collision col)
    {
        
        if (col.gameObject.tag == "Platform")
        {
            platformRBody = null;
            col.transform.parent = platformRBody.transform;
            isOnPlatform = false;
        }
        
    }
}
