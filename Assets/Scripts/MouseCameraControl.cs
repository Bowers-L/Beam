using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=_QajrabyTJc
public class MouseCameraControl : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;

    public Transform playerTrans;

    private float xRot = 0f;    //Look up/down ("pitch")

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRot -= mouseY;

        xRot = Mathf.Clamp(xRot, -90f, 90f);

        //The mouse x rotates the whole player while the y is used to look up and down, 
        //which is why these require 2 separate lines.
        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        playerTrans.Rotate(Vector3.up * mouseX);
    }
}
