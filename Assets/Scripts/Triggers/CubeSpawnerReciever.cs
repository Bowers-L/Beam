using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beam.Triggers
{
    public class CubeSpawnerReciever : TriggerReceiver
    {
        public GameObject cubePrefab;
        public float bufferTime;
        public bool infiniteCubes;
        public int maxCubes;

        //float bufferTimer;
        int cubeCount;

        // Start is called before the first frame update
        new public void Start()
        {
            base.Start();
            cubeCount = 0;
            //bufferTimer = 0;
        }

        public override void HandleActivated()
        {
            StartCoroutine(countdown());
        }

        public override void HandleDeactivated()
        {
            //Don't do anything because cube spawners only respond to being activated.
        }

        IEnumerator countdown()
        {
            yield return new WaitForSeconds(bufferTime);

            if (infiniteCubes || cubeCount < maxCubes)
            {
                //bufferTimer = 0f;
                GameObject newCube = Instantiate(cubePrefab, gameObject.transform.position, cubePrefab.transform.rotation);
                listenee = newCube.GetComponent<PlaneTrigger>(); //Change the listenee to be the cube that just spawned
                cubeCount++;
            }

            yield return null;
        }
    }
}

