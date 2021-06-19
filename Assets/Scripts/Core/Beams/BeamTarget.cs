using UnityEngine;
using Beam.Events;
using Beam.Utility;

namespace Beam.Core.Beams
{
    public class BeamTarget : MonoBehaviour
    {
        //What beam this target is attached to. 
        private BeamSource currSource;
        private Rigidbody rb;

        //How long the beam is.
        private float currBeamDist;

        public void Start()
        {
            currSource = null;
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("BeamTarget must have rigidbody component.");
            }
        }

        public void attachBeam(BeamSource source, Ray beam)
        {
            currSource = source;
            EventManager.StartListening<BeamSourceMoved, BeamSource>(handleBeamMoved);
            EventManager.StartListening<BeamRelease, BeamSource>(detachBeam);

            //snap object to the cursor focus

            rb.MovePosition(UnityEngineExt.projectPointOntoLine(transform.position, beam));
            currBeamDist = (transform.position - source.transform.position).magnitude;


            //Change sprite to highlight change.
        }

        public void detachBeam(BeamSource source)
        {
            if (currSource == source)
            {
                currSource = null;
                EventManager.StopListening<BeamSourceMoved, BeamSource>(handleBeamMoved);
                EventManager.StopListening<BeamRelease, BeamSource>(detachBeam);
            }
        }

        //The Callee is responsible for checking if the source is the same as currSource!
        public void handleBeamMoved(BeamSource source)
        {
            if (currSource == source)
            {
                rb.MovePosition(source.transform.position + source.transform.forward * currBeamDist);
            }
        }
    }
}
