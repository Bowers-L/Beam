using UnityEngine;
using UnityEngine.Events;

using Beam.Events;
using Beam.Utility;


namespace Beam.Core.Beams
{
    public class BeamShot : UnityEvent<Ray> { }
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

        public void ActivateBeam(Ray beamRay)
        {
            EventManager.InvokeEvent<BeamShot, Ray>(beamRay);
            BeamTarget target = FindTarget(beamRay);
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

        public virtual void TravelBeam(Ray beamRay)
        {
            BeamTarget target = currTarget != null ? currTarget : FindTarget(beamRay);

            if (target != null)
            {
                Vector3 tempPos = transform.position;
                transform.position = currTarget.transform.position;
                currTarget.transform.position = tempPos;
                DeactivateBeam();
            }

        }

        protected BeamTarget FindTarget(Ray beamRay)
        {
            RaycastHit hitInfo;
            UnityEngineExt.GetMaskWithout("Ignore Raycast");
            if (Physics.Raycast(beamRay, out hitInfo, maxBeamRange))
            {
                BeamTarget target = hitInfo.collider.GetComponent<BeamTarget>();
                return target;
            }

            return null;
        }
    }

}
