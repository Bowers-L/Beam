using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beam.Core.Beams
{
    public class BeamSourceEffect : MonoBehaviour
    {
        public void SetPos(Vector3 start, Vector3 end, Vector3 startForward)
        {
            LineRenderer lr = GetComponentInChildren<LineRenderer>();
            ParticleSystem source = GetComponentInChildren<ParticleSystem>();

            List<Vector3> positions = sampleBezier(start, getPointB(start, end, startForward), end, 10);
            lr.positionCount = positions.Count;
            lr.SetPositions(positions.ToArray());
            source.transform.position = start;
        }

        //Curve simulates a Bezier Curve: https://en.wikipedia.org/wiki/B%C3%A9zier_curve
        private Vector3 getPointB(Vector3 A, Vector3 C, Vector3 AB)
        {
            Vector3 AC = C - A;
            float bScale = 0.9f;
            float ABMag = 1.0f / 2 * AC.magnitude * bScale;
            return A + ABMag * AB.normalized;
        }

        private Vector3 QuadraticBezierCurve(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 d = Vector3.Lerp(a, b, t);
            Vector3 e = Vector3.Lerp(b, c, t);
            return Vector3.Lerp(d, e, t);
        }

        private List<Vector3> sampleBezier(Vector3 a, Vector3 b, Vector3 c, int numSamples)
        {
            Debug.Log(b);
            List<Vector3> positions = new List<Vector3>();
            for (int i=0; i<numSamples; i++)
            {
                positions.Add(QuadraticBezierCurve(a, b, c, (float) i / (numSamples - 1)));
            }
            return positions;
        }
    }
}
