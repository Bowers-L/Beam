using UnityEngine;
using UnityEngine.Events;

using Beam.Events;
using Beam.Utility;

using System.Collections;

namespace Beam.Core.Beams
{
    public class GrabBeamSource : BeamSource
    {

        public float maxBeamFlex;   //The maximum angle between an object and the player's cursor before the beam breaks.
        public float beamSnapSpeed;
        public void FixedUpdate()
        {
            
            if (checkTargetBlocked(BeamType.Grab))
            {
                ReleaseBeam();
            }
        }

        public override void ReleaseBeam()
        {
            if (currTarget != null)
            {
                currTarget.DetachBeam();
                currTarget = null;
            }

            if (beamEffectInst != null)
            {
                Destroy(beamEffectInst);
            }
        }

        public override void ShootBeam(Ray sourceRay)
        {
            BeamTarget target = FindTarget(sourceRay, BeamType.Grab);
            if (target != null)
            {
                currTarget = target;

                beamEffectInst = Instantiate(beamEffectPrefab);
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPos(beamPos.position, currTarget.transform.position, transform.forward);

                target.AttachBeam(this, sourceRay);
            }
        }
    }
}
