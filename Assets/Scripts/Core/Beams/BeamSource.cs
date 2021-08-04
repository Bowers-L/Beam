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
        public bool beamActive
        {
            get;
            private set;
        }

        public void Update()
        {
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
