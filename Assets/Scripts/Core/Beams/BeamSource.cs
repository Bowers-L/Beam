using UnityEngine;
using UnityEngine.Events;

using Beam.Events;
using Beam.Utility;


namespace Beam.Core.Beams
{

    public class BeamEvent : UnityEvent<Transform> { }

    public abstract class BeamSource : MonoBehaviour
    {
        public float maxBeamRange;
        public bool beamActive
        {
            get;
            private set;
        }

        public class BeamShotEvent : UnityEvent<Ray> { }

        public void activateBeam(Ray beamRay)
        {
            beamActive = true;
            EventManager.InvokeEvent<BeamShotEvent, Ray>(beamRay);

            RaycastHit hitInfo;
            UnityEngineExt.GetMaskWithout("Ignore Raycast");
            if (Physics.Raycast(beamRay, out hitInfo, maxBeamRange))
            {
                Beamable beamTarget = hitInfo.collider.GetComponent<Beamable>();
                if (beamTarget != null)
                {
                    beamTarget.attachBeam();
                }
            }
        }

        public void deactivateBeam()
        {
            beamActive = false;
        }

        protected Ray getBeamRay()
        {
            return new Ray(transform.position, transform.forward);
        }
    }

}
