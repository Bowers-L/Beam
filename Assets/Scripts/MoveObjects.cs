using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.SetParent(this.gameObject.transform, true);
    }
    public void OnCollisionExit(Collision collision)
    {
        collision.gameObject.transform.parent = null;
    }
}
