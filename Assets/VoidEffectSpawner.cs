using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VoidEffectSpawner : MonoBehaviour
{
    public GameObject voidEffectPrefab;

    //MMaybe can automate these by getting it from ProBuilder?
    public float width;
    public float length;

    public Transform bottomLeft;    //Used to make this easier to manage
    public float spaceBtwEffects;

    public void Start()
    {
        for (float x = 0; x < width; x += spaceBtwEffects)
        {
            for (float z = 0; z < length; z += spaceBtwEffects)
            {
                GameObject.Instantiate(voidEffectPrefab, bottomLeft.position + new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }
}
