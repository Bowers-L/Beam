using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

namespace Beam.Utility
{
    //Namespace for various utilities extending the Unity API.
    public class UnityEngineExt
    {
        //Retrieves a layer mask without the given names.
        public static int GetMaskWithout(params string[] layerNames)
        {
            return ~LayerMask.GetMask(layerNames);
        }

        //Outputs the closest point on the given line to the given point.
        public static Vector3 ProjectPointOntoLine(Vector3 point, Ray line)
        {
            /*
             * Math:
             * Let point P and line L represent the arguments
             * L has an origin point O and a direction V
             * Let S be the closest point to P on the line. Mathematically, this means that S is a point on L such that V is orthogonal to PS.
             * 
             * The line segment OP is the hypotenuse of right triangle OPS
             * By projecting OP onto line L, the result will be OS.
             * Writing OS in the form S-O, the point S = O + OS
             * 
             * This concludes the math.
             */

            Vector3 OP = point - line.origin;
            return line.origin + Vector3.Project(OP, line.direction);
        }

        public static string FormatTime(float time)
        {
            int minutes = (int)time / 60;
            int seconds = (int)time - 60 * minutes;
            int milliseconds = (int)(time * 1000) - 1000 * seconds;
            return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }
    }
}
