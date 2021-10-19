using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Beam.Utility;

namespace Beam.Core.Beams
{
    public class GrabBeamEffect : MonoBehaviour
    {
        public GameObject beamBreakPrefab;

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

        //The idea is to create a dissolve effect where the beam breaks in the middle and then dissolves towards the ends.
        //To do this, the beam is split up into two separate line renderers and points are deleted until the lines are completely gone.
        /*
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
                if (sourcePositions.Count > 0)
                {
                    sourcePositions[sourcePositions.Count - 1] = Bezier.QuadraticCurve(a, b, c, t);
                }

                if (targetPositions.Count > 0)
                {
                    targetPositions[0] = Bezier.QuadraticCurve(a, b, c, 1.0f - t);
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
        */
    }
}
