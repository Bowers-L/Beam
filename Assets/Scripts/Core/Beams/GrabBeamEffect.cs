using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Beam.Core.Beams
{
    public class GrabBeamEffect : MonoBehaviour
    {
        public GameObject beamBreakPrefab;

        private LineRenderer lr;
        private VisualEffect source;
        private List<Vector3> positions;

        //Set a straight line from start to end.
        public void SetPosLinear(Vector3 start, Vector3 end, Vector3 startForward)
        {
            lr = GetComponentInChildren<LineRenderer>();
            source = GetComponentInChildren<VisualEffect>();

            positions = new List<Vector3>();
            positions.Add(start);
            positions.Add(end);
            lr.positionCount = 2;
            lr.SetPositions(positions.ToArray());
            source.transform.position = start;
            source.transform.forward = startForward;
        }

        //Set a curved line from start to end starting in the direction of startForward.
        public void SetPosBezier(Vector3 start, Vector3 end, Vector3 startForward)
        {
            lr = GetComponentInChildren<LineRenderer>();
            source = GetComponentInChildren<VisualEffect>();

            positions = sampleBezier(start, getPointB(start, end, startForward), end, 11);
            lr.positionCount = positions.Count;
            lr.SetPositions(positions.ToArray());
            source.transform.position = start;
            source.transform.forward = startForward;
        }

        //The idea is to create a dissolve effect where the beam breaks in the middle and then dissolves towards the ends.
        //To do this, the beam is split up into two separate line renderers and points are deleted until the lines are completely gone.
        public IEnumerator BeamBreak()
        {

            //Get rid of the original beam effect
            Destroy(lr.gameObject);
            Destroy(source.gameObject);

            //Create the "Beam Break" effect
            GameObject breakInst = Instantiate(beamBreakPrefab);

            //dt - change in interpolation each iteration,
            //timeForBreak - How much total time (in seconds) the animation takes)
            //t - The current interpolation for the sourceLine (1-t for the targetLine)
            const float dt = 0.01f;
            const float timeForBreak = 0.3f;
            float t = 0.5f;

            //The parameters for the bezier curve
            Vector3 a = positions[0];
            Vector3 b = positions[positions.Count / 2];
            Vector3 c = positions[positions.Count - 1];

            //The list of positions for each line
            List<Vector3> sourcePositions = positions.GetRange(0, positions.Count / 2);
            List<Vector3> targetPositions = positions.GetRange(positions.Count / 2, positions.Count - positions.Count / 2);

            LineRenderer[] lines = breakInst.GetComponentsInChildren<LineRenderer>();
            LineRenderer sourceLine = lines[0].name.CompareTo("SourceLaser") == 0 ? lines[0] : lines[1];
            LineRenderer targetLine = sourceLine == lines[0] ? lines[1] : lines[0];
            while (t > 0f)
            {
                Debug.Log(t);
                if (sourcePositions.Count > 0)
                {
                    sourcePositions[sourcePositions.Count - 1] = QuadraticBezierCurve(a, b, c, t);
                }

                if (targetPositions.Count > 0)
                {
                    targetPositions[0] = QuadraticBezierCurve(a, b, c, 1.0f - t);
                }

                sourceLine.positionCount = sourcePositions.Count;
                targetLine.positionCount = targetPositions.Count;
                sourceLine.SetPositions(sourcePositions.ToArray());
                targetLine.SetPositions(targetPositions.ToArray());

                float sourceTOfPoint = (sourcePositions.Count - 1.0f) / positions.Count;
                if (sourcePositions.Count > 0 && t < sourceTOfPoint)
                {
                    sourcePositions.RemoveAt(sourcePositions.Count - 1);
                }
                if (targetPositions.Count > 0 && 1-t > 1-sourceTOfPoint) 
                {
                    targetPositions.RemoveAt(0);
                }

                t -= dt;
                yield return new WaitForSeconds(2 * dt * timeForBreak);
            }

            Destroy(breakInst);
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
            List<Vector3> positions = new List<Vector3>();
            for (int i=0; i<numSamples; i++)
            {
                positions.Add(QuadraticBezierCurve(a, b, c, (float) i / (numSamples - 1)));
            }
            return positions;
        }
    }
}
