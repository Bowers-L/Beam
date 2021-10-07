

using UnityEngine;
using UnityEngine.Events;

using Beam.Events;
using Beam.Utility;

using System.Collections;
using System.Collections.Generic;

namespace Beam.Core.Beams
{
    public enum BeamType
    {
        None,
        Grab,
        Swap
    }

    public class BeamShot : UnityEvent<BeamSource, Ray> { }
    public class BeamRelease : UnityEvent<BeamSource> { }
    public class BeamSourceMoved : UnityEvent<BeamSource> { }

    public abstract class BeamSource : MonoBehaviour
    {
        public float maxBeamRange;  //The maximum distance the beam can travel to latch onto an object


        public GameObject beamEffectPrefab;
        public Transform beamPos;

        protected BeamTarget currTarget;
        protected bool shootingBeam;

        [HideInInspector]
        public GameObject beamEffectInst;

        public void Start()
        {
            currTarget = null;
            shootingBeam = false;
            if (beamEffectPrefab == null)
            {
                Debug.LogWarning("Beam source has no effect attached.");
            }
        }

        public abstract void ShootBeam(Ray sourceRay);
        public abstract void UpdateBeam(Ray sourceRay);
        public abstract void ReleaseBeam();

        protected bool CheckTargetBlocked(BeamType type)
        {
            if (currTarget != null)
            {
                Ray beamDir = new Ray(transform.position, currTarget.transform.position - transform.position);

                RaycastHit hitInfo;
                if (Physics.Raycast(beamDir, out hitInfo, currTarget.currBeamDist, GetLayerMask(type)))
                {
                    return !hitInfo.transform.gameObject.Equals(currTarget.gameObject);
                }
            }
            return false;
        }

        protected T FindTarget<T>(Ray beamRay, BeamType type, out RaycastHit lastHitInfo, out List<Ray> outputList) where T : BeamTarget
        {
            outputList = new List<Ray>();
            return FindTargetRecursive<T>(beamRay, type, out lastHitInfo, outputList, 17, 1);
        }

        private T FindTargetRecursive<T>(Ray beamRay, BeamType type, out RaycastHit lastHitInfo, List<Ray> outputList, int maxDepth, int currDepth) where T : BeamTarget
        {
            if (outputList == null)
            {
                outputList = new List<Ray>();
            }
            outputList.Add(beamRay);
            RaycastHit hitInfo;
            if (Physics.Raycast(beamRay, out hitInfo, maxBeamRange, GetLayerMask(type)))
            {
                
                if (hitInfo.collider.gameObject.tag == "Mirror")
                {
                    if (currDepth > maxDepth)
                    {
                        //safety case for infinite recursion
                        lastHitInfo = hitInfo;
                        return null;
                    }
                    Vector3 pos = hitInfo.point;
                    Vector3 dir = Vector3.Reflect(beamRay.direction, hitInfo.normal);
                    Ray r1 = new Ray(pos, dir);
                    return FindTargetRecursive<T>(r1, type, out lastHitInfo, outputList, maxDepth, currDepth+1);
                }
                
                T target = hitInfo.collider.GetComponentInParent<T>();
                lastHitInfo = hitInfo;
                return target;
            }

            lastHitInfo = hitInfo;
            return null;
        }

        protected int GetLayerMask(BeamType type)
        {
            int layerMask = UnityEngineExt.GetMaskWithout("Ignore Raycast", "Player", "Allows Both");
            switch (type)
            {
                case BeamType.Grab:
                    layerMask &= UnityEngineExt.GetMaskWithout("Allows Grab");
                    break;
                case BeamType.Swap:
                    layerMask &= UnityEngineExt.GetMaskWithout("Allows Swap");
                    break;
                default:
                    break;
            }

            return layerMask;
        }
    }

}
