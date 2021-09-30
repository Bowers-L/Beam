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
            if (CheckTargetBlocked(BeamType.Swap))
            {
                ReleaseBeam();
            }

            if (shootingBeam)
            {
                UpdateBeam(new Ray(transform.position, transform.forward));
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

            shootingBeam = false;
        }

        public override void ShootBeam(Ray sourceRay)
        {
            if (beamEffectInst == null)
            {
                beamEffectInst = Instantiate(beamEffectPrefab);
            }
            RaycastHit hitInfo;
            currTarget = FindTarget(sourceRay, BeamType.Swap, out hitInfo);

            if (currTarget != null)
            {
                beamEffectInst.GetComponent<SwapBeamEffect>().SetPos(beamPos.position, currTarget.transform.position, transform.forward);
                beamEffectInst.GetComponent<SwapBeamEffect>().SetHasTarget(true);
            } else
            {
                beamEffectInst.GetComponent<SwapBeamEffect>().SetPos(beamPos.position, hitInfo.point, transform.forward);
                beamEffectInst.GetComponent<SwapBeamEffect>().SetHasTarget(false);
            }

            shootingBeam = true;
        }

        public override void UpdateBeam(Ray sourceRay)
        {
            ShootBeam(sourceRay);
        }
    }
}
