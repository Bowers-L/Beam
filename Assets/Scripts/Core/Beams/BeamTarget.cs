using UnityEngine;
using Beam.Events;
using Beam.Utility;

namespace Beam.Core.Beams
{
    public class BeamTarget : MonoBehaviour
    {
        //How long the beam is.
        public float currBeamDist;

        //What beam this target is attached to. 
        protected BeamSource currSource;
        private Rigidbody rb;
        private GameObject beamEffectInst;



        public void Start()
        {
            currSource = null;
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("BeamTarget must have rigidbody component.");
            }
        }

        public void FixedUpdate()
        {

            if (currSource != null)                //Beam is attached to something
            {
                //Stop translational and rotational velocity
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                transform.forward = currSource.transform.forward;

                //Check how much the beam bends and detach if needed
                Vector3 sourceToTarget = transform.position - currSource.transform.position;
                float beamFlex = Vector3.Angle(sourceToTarget, currSource.transform.forward);
                beamFlex = beamFlex < 0 ? -beamFlex : beamFlex; //Make sure beamFlex is positive
                if (beamFlex > currSource.maxBeamFlex)
                {
                    currSource.DeactivateBeam();
                } else
                {
                    Vector3 targetPos = currSource.transform.position + currSource.transform.forward * currBeamDist;
                    Vector3 targetDir = targetPos - transform.position;

                    rb.AddForce(targetDir * currSource.beamSnapSpeed, ForceMode.VelocityChange);
                    currSource.beamEffectInst.GetComponent<BeamSourceEffect>().SetPos(currSource.beamEffectPos.transform.position, transform.position);
                }
            }
        }

        public void attachBeam(BeamSource source, Ray beam)
        {
            currSource = source;

            //snap object center to the cursor
            rb.MovePosition(UnityEngineExt.projectPointOntoLine(transform.position, beam));
            currBeamDist = (transform.position - source.transform.position).magnitude;

            rb.useGravity = false;

            //Change sprite to highlight change.
        }

        public void detachBeam()
        {
            currSource = null;
            rb.useGravity = true;
        }

        //The Callee is responsible for checking if the source is the same as currSource!
        /*
        public void handleBeamMoved(BeamSource source)
        {
            if (currSource == source)
            {
                Vector3 targetPos = source.transform.position + source.transform.forward * currBeamDist;
                rb.MovePosition(targetPos);
            }
        }
        */
    }
}
