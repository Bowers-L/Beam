using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VFX;

namespace Beam.Core.Beams {
    public class SwapBeamEffect : MonoBehaviour
    {
        [SerializeField]
        private VisualEffect sourceToDest;
        [SerializeField]
        private VisualEffect destToSource;
        [SerializeField]
        private GameObject beamSource;

        void Start()
        {
            if (sourceToDest == null || destToSource == null)
            {
                Debug.LogError("Visual Effects for SwapBeam need to be set in inspector.");
            }
        }
        public void SetPos(Vector3 source, Vector3 target, Vector3 sourceForward)
        {
            sourceToDest.SetVector3("StartPos", source);
            sourceToDest.SetVector3("EndPos", target);

            destToSource.SetVector3("StartPos", target);
            destToSource.SetVector3("EndPos", source);

            beamSource.transform.position = source;
            beamSource.transform.forward = sourceForward;
        }

        public void SetHasTarget(bool value)
        {
            destToSource.enabled = value;

            //Create indicator that tells player a target exists.
        }
    }
}

