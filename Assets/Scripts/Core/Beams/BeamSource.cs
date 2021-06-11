using UnityEngine;
using Beam.Utility;

namespace Beam.Core.Beams
{
    public abstract class BeamSource : MonoBehaviour
    {
        public float maxBeamRange;
        public bool beamActive
        {
            get;
            private set;
        }

        public void activateBeam(Ray beamRay)
        {
            beamActive = true;

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
