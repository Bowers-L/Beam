using System.Collections;

using UnityEngine;

using Beam.Core.Beams;

namespace Beam.Triggers
{
    public class CubeSpawnerReciever : TriggerReceiver
    {
        public GameObject cubePrefab;
        public GameObject beamEffectPrefab;
        public float smallCubeScale;

        public float bufferTime;
        private float minSmallCubeTimeWait;
        public float effectDuration;
        public bool infiniteCubes;
        public int maxCubes;

        int cubeCount;

        private bool spawningCube = false;

        // Start is called before the first frame update
        new public void Start()
        {
            
            base.Start();
            cubeCount = 0;
            minSmallCubeTimeWait = 1.0f;
        }

        public override void HandleActivated()
        {
            if (!spawningCube)
            {
                spawningCube = true;
                StartCoroutine(SpawnCube());
            }

        }

        public override void HandleDeactivated()
        {
            //Don't do anything because cube spawners only respond to being activated.
        }

        IEnumerator SpawnCube()
        {
            yield return new WaitForSeconds(bufferTime);

            if (infiniteCubes || cubeCount < maxCubes)
            {
                GameObject newCube = Instantiate(cubePrefab, gameObject.transform.position, cubePrefab.transform.rotation);
                newCube.transform.localScale = cubePrefab.transform.localScale.normalized * smallCubeScale;

                yield return new WaitForSeconds(minSmallCubeTimeWait);
                yield return new WaitUntil(() => newCube.GetComponent<Rigidbody>().velocity.magnitude < 0.1f);  //Wait until the cube has landed.

                //Do the effect.
                float overExtendFactor = 0.1f;
                GameObject effect = Instantiate(beamEffectPrefab);
                Vector3[] positions = { 
                    this.transform.position, 
                    (1+overExtendFactor) * newCube.transform.position - overExtendFactor * this.transform.position};
                effect.GetComponent<BeamLineEffect>().SetHasTarget(true);
                effect.GetComponent<BeamLineEffect>().SetPosLinear(positions);
                yield return new WaitForSeconds(effectDuration);

                Destroy(effect);
                newCube.transform.localScale = cubePrefab.transform.localScale;

                listenee = newCube.GetComponent<PlaneTrigger>(); //Change the listenee to be the cube that just spawned
                spawningCube = false;
                cubeCount++;
            }

            yield return null;
        }
    }
}

