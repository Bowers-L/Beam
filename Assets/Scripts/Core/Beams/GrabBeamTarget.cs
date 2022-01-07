using UnityEngine;

using Beam.Utility;

using System.Collections.Generic;

namespace Beam.Core.Beams
{
    public class GrabBeamTarget : BeamTarget
    {
        public ForceMode forceMode;
        public float beamReleaseVelocityCap;
        public bool snapToBeam;
        public bool defaultUsesGravity;

        public void AttachBeam(GrabBeamSource source, Ray beam)
        {

            //snap object center to the cursor
            if (snapToBeam)
            {
                rb.MovePosition(UnityEngineExt.ProjectPointOntoLine(transform.position, beam));
                currBeamDist = (transform.position - beam.origin).magnitude;
            }

            rb.useGravity = false;
        }

        public void UpdateBeam(GrabBeamSource source, List<Ray> reflectionList)
        {
            //Stop translational and rotational velocity
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (snapToBeam)
            {
                transform.forward = source.transform.forward;
            }


            //Check how much the beam bends and detach if needed
            Vector3 sourceToTarget = transform.position - source.transform.position;
            float beamFlex = Vector3.Angle(sourceToTarget, source.transform.forward);
            beamFlex = beamFlex < 0 ? -beamFlex : beamFlex; //Make sure beamFlex is positive
            if (beamFlex > source.maxBeamFlex)
            {
                //StartCoroutine(source.beamEffectInst.GetComponent<GrabBeamEffect>().BeamBreak());
                source.ReleaseBeam();
            }
            else
            {
                Vector3 targetPos = reflectionList[reflectionList.Count-1].origin + reflectionList[reflectionList.Count-1].direction * currBeamDist;
                Vector3 targetDir = targetPos - transform.position;

                rb.AddForce(targetDir * source.beamSnapSpeed, forceMode);
            }
        }

        public void DetachBeam()
        {
            rb.useGravity = defaultUsesGravity;
            float velMag = Mathf.Min(rb.velocity.magnitude, beamReleaseVelocityCap);
            rb.velocity = rb.velocity.normalized * velMag;
        }
    }
}