using UnityEngine;
using UnityEngine.Events;

using Beam.Events;
using Beam.Utility;

using System.Collections.Generic;


namespace Beam.Core.Beams
{
    public class GrabBeamSource : BeamSource
    {

        public float maxBeamFlex;   //The maximum angle between an object and the player's cursor before the beam breaks.
        public float beamSnapSpeed;

        public void FixedUpdate()
        {
            
            if (CheckTargetBlocked(BeamType.Grab))
            {
                ReleaseBeam();
            }

            if (shootingBeam)
            {
                UpdateBeam(new Ray(this.transform.position, this.transform.forward));
            }
        }

        public override void ShootBeam(Ray sourceRay)
        {
            if (beamEffectInst == null)
            {
                beamEffectInst = Instantiate(beamEffectPrefab);
            }

            List<Ray> rayList;
            RaycastHit hitInfo;
            currTarget = FindTarget<GrabBeamTarget>(sourceRay, BeamType.Grab, out hitInfo, out rayList);

            shootingBeam = true;

            if (currTarget != null)
            {
                GrabBeamTarget target = currTarget as GrabBeamTarget;
                target.AttachBeam(this, rayList[rayList.Count-1]);
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosBezier(GetBeamRendererPositions(rayList, hitInfo).ToArray(), rayList[rayList.Count-1].direction);
            } else {
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosLinear(GetBeamRendererPositions(rayList, hitInfo).ToArray());
            }
            beamEffectInst.GetComponent<GrabBeamEffect>().SetHasTarget(currTarget != null);
        }

        public override void UpdateBeam(Ray sourceRay)
        {

            if (currTarget != null)
            {
                GrabBeamTarget target = currTarget as GrabBeamTarget;

                List<Ray> rayList;
                RaycastHit hitInfo;
                FindTarget<GrabBeamTarget>(sourceRay, BeamType.Grab, out hitInfo, out rayList);
                target.UpdateBeam(this, rayList);
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosBezier(GetBeamRendererPositions(rayList, hitInfo).ToArray(), rayList[rayList.Count - 1].direction);
            } else
            {
                //Again, this is lazy lol. Sorry.
                ShootBeam(sourceRay);
            }
        }

        public override void ReleaseBeam()
        {

            if (currTarget != null)
            {
                GrabBeamTarget target = currTarget as GrabBeamTarget;
                target.DetachBeam();
                currTarget = null;
            }

            if (beamEffectInst != null)
            {
                Destroy(beamEffectInst);
            }

            shootingBeam = false;
        }
    }
}
