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
                target.AttachBeam(this, sourceRay);
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosBezier(beamPos.position, currTarget.transform.position, transform.forward);
            } else if (hitInfo.collider != null)
            {
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosLinear(beamPos.position, hitInfo.point, transform.forward);
            } else
            {
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosLinear(beamPos.position, transform.position + transform.forward * maxBeamRange, transform.forward);
            }

            shootingBeam = true;
        }

        public override void UpdateBeam(Ray sourceRay)
        {

            if (currTarget != null)
            {
                GrabBeamTarget target = currTarget as GrabBeamTarget;
                target.UpdateBeam(this);
                beamEffectInst.GetComponent<GrabBeamEffect>().SetPosBezier(beamPos.position,
                currTarget.transform.position,
                transform.forward);
            } else
            {
                ShootBeam(sourceRay);
            }
        }
    }
}
