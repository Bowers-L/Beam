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

        public override void ShootBeam(Ray sourceRay)
        {
            if (beamEffectInst == null)
            {
                beamEffectInst = Instantiate(beamEffectPrefab);
            }

            RaycastHit hitInfo;
            List<Ray> rayList = new List<Ray>();
            currTarget = FindTarget<GrabBeamTarget>(sourceRay, BeamType.Grab, out hitInfo, out rayList);

            if (currTarget != null)
            {
                GrabBeamTarget target = currTarget as GrabBeamTarget;
                target.AttachBeam(this, rayList[rayList.Count-1]);
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosBezier(rayList, currTarget.transform.position);
            } else if (hitInfo.collider != null)
            {
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosLinear(rayList, hitInfo.point);
            } else
            {
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosLinear(rayList, rayList[rayList.Count-1].origin + rayList[rayList.Count-1].direction * maxBeamRange);
            }

            shootingBeam = true;
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
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosBezier(rayList, currTarget.transform.position);
            } else
            {
                ShootBeam(sourceRay);
            }
        }
    }
}
