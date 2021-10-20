using UnityEngine;
using UnityEngine.Events;

using Beam.Events;
using Beam.Utility;

using System.Collections.Generic;

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

        public override void ShootBeam(Ray sourceRay)
        {
            if (beamEffectInst == null)
            {
                beamEffectInst = Instantiate(beamEffectPrefab);
            }

            RaycastHit hitInfo;
            List<Ray> rayList = new List<Ray>();
            currTarget = FindTarget<SwapBeamTarget>(sourceRay, BeamType.Swap, out hitInfo, out rayList);

            if (currTarget != null)
            {
                beamEffectInst.GetComponent<SwapBeamEffect>().SetPosBezier(GetBeamRendererPositions(rayList, hitInfo).ToArray(), rayList[rayList.Count - 1].direction);
            }
            else
            {
                beamEffectInst.GetComponent<SwapBeamEffect>().SetPosLinear(GetBeamRendererPositions(rayList, hitInfo).ToArray());
            }
            beamEffectInst.GetComponent<SwapBeamEffect>().SetHasTarget(currTarget != null);

            shootingBeam = true;
        }

        public override void UpdateBeam(Ray sourceRay)
        {
            //This is really lazy ik.
            ShootBeam(sourceRay);
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
    }
}
