using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beam.Core.Beams
{
    public class BeamSourceEffect : MonoBehaviour
    {
        public void SetPos(Vector3 start, Vector3 end)
        {
            LineRenderer lr = GetComponentInChildren<LineRenderer>();
            ParticleSystem source = GetComponentInChildren<ParticleSystem>();
            Vector3[] positions = { start, end };
            lr.SetPositions(positions);
            source.transform.position = start;
        }
    }
}
