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
            beamEffectInst.GetComponent<GrabBeamEffect>().SetAttached(currTarget != null);
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

        //These need to be separated out because visually the beam starts from the player's gun rather than directly in front of the camera.
        private List<Vector3> GetBeamRendererPositions(List<Ray> beamRayList, RaycastHit hitInfo)
        {
            List<Vector3> positionsForEffect = new List<Vector3>();
            positionsForEffect.Add(beamPos.position);
            for (int i = 1; i < beamRayList.Count; i++)
            {
                positionsForEffect.Add(beamRayList[i].origin);
            }

            if (currTarget != null)
            {
                positionsForEffect.Add(currTarget.transform.position);
            } else
            {
                //Either go in a straight line until the beam hits something or it runs out of range.
                Vector3 finalPoint = hitInfo.collider != null ?
                    hitInfo.point :
                    beamRayList[beamRayList.Count - 1].origin + beamRayList[beamRayList.Count - 1].direction * maxBeamRange;

                positionsForEffect.Add(finalPoint);
            }

            return positionsForEffect;
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
