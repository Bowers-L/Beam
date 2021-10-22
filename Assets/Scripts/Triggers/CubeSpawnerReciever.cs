using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beam.Triggers
{
    public class CubeSpawnerReciever : TriggerReceiver
    {
        public float bufferTime;
        float bufferTimer;
        public int maxCubes;
        int cubeCount;
        public GameObject cubePrefab;



        // Start is called before the first frame update
        new public void Start()
        {
            cubeCount = 0;
            bufferTimer = 0;
            countdown();
        }

        public override void HandleActivated()
        {
            if (cubeCount < maxCubes && bufferTimer == bufferTime)
            {
                bufferTimer = 0f;
                GameObject newCube = Instantiate(cubePrefab, gameObject.transform.position, cubePrefab.transform.rotation);
                cubeCount++;
            }
        }

        public override void HandleDeactivated()
        {
            countdown();
        }


        void countdown()
        {
            while (!(bufferTimer == bufferTime))
            {
                bufferTimer += Time.deltaTime;
            }

        }
    }
}

