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
        public float maxBeamRange;
        public bool beamActive
        {
            get;
            private set;
        }

        public void Update()
        {
            if (beamActive)
            {   //Useless event bc anything that cares can just check if the beam is active.
                //EventManager.InvokeEvent<BeamSourceMoved, BeamSource>(this);
            }
        }

        public void activateBeam(Ray beamRay)
        {
            EventManager.InvokeEvent<BeamShot, Ray>(beamRay);

            RaycastHit hitInfo;
            UnityEngineExt.GetMaskWithout("Ignore Raycast");
            if (Physics.Raycast(beamRay, out hitInfo, maxBeamRange))
            {
                BeamTarget target = hitInfo.collider.GetComponent<BeamTarget>();
                if (target != null)
                {
                    beamActive = true;
                    target.attachBeam(this, beamRay);
                }
            }
        }

        public void deactivateBeam()
        {
            beamActive = false;

            EventManager.InvokeEvent<BeamRelease, BeamSource>(this);
        }
    }

}
