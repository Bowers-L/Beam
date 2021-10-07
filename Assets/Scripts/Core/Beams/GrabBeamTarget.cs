using UnityEngine;

using Beam.Utility;

namespace Beam.Core.Beams
{
    public class GrabBeamTarget : BeamTarget
    {
        public void AttachBeam(GrabBeamSource source, Ray beam)
        {

            //snap object center to the cursor
            rb.MovePosition(UnityEngineExt.projectPointOntoLine(transform.position, beam));
            currBeamDist = (transform.position - source.transform.position).magnitude;

            rb.useGravity = false;
        }

        public void UpdateBeam(GrabBeamSource source)
        {
            //Stop translational and rotational velocity
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.forward = source.transform.forward;

            //Check how much the beam bends and detach if needed
            Vector3 sourceToTarget = transform.position - source.transform.position;
            float beamFlex = Vector3.Angle(sourceToTarget, source.transform.forward);
            beamFlex = beamFlex < 0 ? -beamFlex : beamFlex; //Make sure beamFlex is positive
            if (beamFlex > source.maxBeamFlex)
            {
                StartCoroutine(source.beamEffectInst.GetComponent<GrabBeamEffect>().BeamBreak());
                source.ReleaseBeam();
            }
            else
            {
                Vector3 targetPos = source.transform.position + source.transform.forward * currBeamDist;
                Vector3 targetDir = targetPos - transform.position;

                rb.AddForce(targetDir * source.beamSnapSpeed, ForceMode.VelocityChange);
            }
        }

        public void DetachBeam()
        {
            rb.useGravity = true;
        }
    }
}
