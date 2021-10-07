using UnityEngine;
using Beam.Events;
using Beam.Utility;

namespace Beam.Core.Beams
{
    public class BeamTarget : MonoBehaviour
    {
        //How long the beam is.
        public float currBeamDist;
        protected Rigidbody rb;

        public void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("BeamTarget must have rigidbody component.");
            }
        }


    }
}
