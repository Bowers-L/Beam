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
            {
                EventManager.InvokeEvent<BeamSourceMoved, BeamSource>(this);
            }
        }

        public void activateBeam(Ray beamRay)
        {
            beamActive = true;
            EventManager.InvokeEvent<BeamShot, Ray>(beamRay);

            RaycastHit hitInfo;
            UnityEngineExt.GetMaskWithout("Ignore Raycast");
            if (Physics.Raycast(beamRay, out hitInfo, maxBeamRange))
            {
                BeamTarget target = hitInfo.collider.GetComponent<BeamTarget>();
                if (target != null)
                {
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
