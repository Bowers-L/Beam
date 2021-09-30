using UnityEngine;
using Beam.Events;
using Beam.Utility;

namespace Beam.Core.Beams
{
    public class BeamTarget : MonoBehaviour
    {
        //How long the beam is.
        public float currBeamDist;

        private Rigidbody rb;

        public void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("BeamTarget must have rigidbody component.");
            }
        }

        public void AttachGrabBeam(GrabBeamSource source, Ray beam)
        {

            //snap object center to the cursor
            rb.MovePosition(UnityEngineExt.projectPointOntoLine(transform.position, beam));
            currBeamDist = (transform.position - source.transform.position).magnitude;

            rb.useGravity = false;
        }

        public void UpdateGrabBeam(GrabBeamSource source)
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

        public void DetachGrabBeam()
        {
            rb.useGravity = true;
        }
    }
}
