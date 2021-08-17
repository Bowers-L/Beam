using UnityEngine;
using UnityEngine.Events;

using Beam.Events;
using Beam.Utility;


namespace Beam.Core.Beams
{
    public enum BeamType
    {
        Grab,
        Swap
    }

    public class BeamShot : UnityEvent<BeamSource, Ray> { }
    public class BeamRelease : UnityEvent<BeamSource> { }
    public class BeamSourceMoved : UnityEvent<BeamSource> { }

    public abstract class BeamSource : MonoBehaviour
    {
        public float maxBeamRange;  //The maximum distance the beam can travel to latch onto an object
        public float maxBeamFlex;   //The maximum angle between an object and the player's cursor before the beam breaks.
        public float beamSnapSpeed;

        public BeamTarget currTarget;

        public void Update()
        {
        }

        public void GrabBeam(Ray beamRay)
        {
            EventManager.InvokeEvent<BeamShot, BeamSource, Ray>(this, beamRay);
            BeamTarget target = FindTarget(beamRay, BeamType.Grab);
            if ( target != null)
            {
                target.attachBeam(this, beamRay);
                currTarget = target;
            }

        }

        public void DeactivateBeam()
        {
            if (currTarget != null)
            {
                currTarget.detachBeam();
                currTarget = null;
                EventManager.InvokeEvent<BeamRelease, BeamSource>(this);
            }
        }

        public virtual void SwapBeam(Ray beamRay)
        {
            BeamTarget target = FindTarget(beamRay, BeamType.Swap);

            if (target != null)
            {
                Vector3 tempPos = transform.position;
                transform.position = currTarget.transform.position;
                currTarget.transform.position = tempPos;
                DeactivateBeam();
            }

        }

        protected BeamTarget FindTarget(Ray beamRay, BeamType type)
        {

            int layerMask = UnityEngineExt.GetMaskWithout("Ignore Raycast");
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
            RaycastHit hitInfo;
            if (Physics.Raycast(beamRay, out hitInfo, maxBeamRange, layerMask))
            {
                BeamTarget target = hitInfo.collider.GetComponentInParent<BeamTarget>();
                return target;
            }

            return null;
        }
    }

}
