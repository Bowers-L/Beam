using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VFX;

using Beam.Utility;

namespace Beam.Core.Beams
{
    public class BeamLineEffect : MonoBehaviour
    {
        public Gradient attachedGradient;
        public Gradient detachedGradient;

        private LineRenderer lr;
        private VisualEffect source;
        private List<Vector3> positions;

        //Set straight lines through each of the rays ending at end.
        public void SetPosLinear(Vector3[] inPositions)
        {
            lr = GetComponentInChildren<LineRenderer>();
            source = GetComponentInChildren<VisualEffect>();

            positions = new List<Vector3>(inPositions);
            lr.positionCount = inPositions.Length;
            lr.SetPositions(inPositions);
            source.transform.position = inPositions[0];
            source.transform.forward = inPositions[1] - inPositions[0];
        }

        //Sets a straight line through rays to the last ray, then curves towards end.
        public void SetPosBezier(Vector3[] inPositions, Vector3 lastRayDir)
        {
            lr = GetComponentInChildren<LineRenderer>();
            source = GetComponentInChildren<VisualEffect>();

            List<Vector3> positions = new List<Vector3>(inPositions);
            positions.RemoveAt(inPositions.Length - 1); //Remove last position and add bezier curve to it.
            Vector3 bezierStart = inPositions[inPositions.Length - 2];
            Vector3 bezierEnd = inPositions[inPositions.Length - 1];
            positions.AddRange(Bezier.QuadraticSample(bezierStart, Bezier.QuadraticApproximateB(bezierStart, bezierEnd, lastRayDir), bezierEnd, 11));

            lr.positionCount = positions.Count;
            lr.SetPositions(positions.ToArray());
            source.transform.position = inPositions[0];
            source.transform.forward = inPositions[1] - inPositions[0];
        }

        public void SetHasTarget(bool hasTarget)
        {
            lr.colorGradient = hasTarget ? attachedGradient : detachedGradient;
        }
    }
}
