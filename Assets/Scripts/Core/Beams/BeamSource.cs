

using UnityEngine;
using UnityEngine.Events;

using Beam.Events;
using Beam.Utility;

using System.Collections;

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

        protected BeamTarget FindTarget(Ray sourceRay, BeamType type, out RaycastHit raycastHitInfo, out bool hit)
        {
            RaycastHit hitInfo;
            hit = Physics.Raycast(sourceRay, out hitInfo, maxBeamRange, GetLayerMask(type));
            raycastHitInfo = hitInfo;
            if (hit)
            {
                BeamTarget target = hitInfo.collider.GetComponentInParent<BeamTarget>();
                return target;
            }
            return null;
        }

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

        protected int GetLayerMask(BeamType type)
        {
            int layerMask = UnityEngineExt.GetMaskWithout("Ignore Raycast", "Player");
            switch (type)
            {
                case BeamType.Grab:
                    layerMask &= UnityEngineExt.GetMaskWithout("Allows Grab", "Allows Both");
                    break;
                case BeamType.Swap:
                    layerMask &= UnityEngineExt.GetMaskWithout("Allows Swap", "Allows Both");
                    break;
                default:
                    break;
            }

            return layerMask;
        }
    }

}
