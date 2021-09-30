using UnityEngine;
using UnityEngine.Events;

using Beam.Events;
using Beam.Utility;

using System.Collections;

namespace Beam.Core.Beams
{
    public class SwapBeamSource : BeamSource
    {
        public void FixedUpdate()
        {
            if (checkTargetBlocked(BeamType.Swap))
            {
                ReleaseBeam();
            }
        }

        public override void ReleaseBeam()
        {
            if (currTarget != null)
            {
                Vector3 tempPos = transform.position;
                transform.position = currTarget.transform.position;
                currTarget.transform.position = tempPos;
            }

            currTarget = null;

            if (beamEffectInst != null)
            {
                Destroy(beamEffectInst);
            }
        }

        public override void ShootBeam(Ray sourceRay)
        {
            currTarget = FindTarget(sourceRay, BeamType.Swap);
        }
    }
}
