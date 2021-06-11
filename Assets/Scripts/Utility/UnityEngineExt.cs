using UnityEngine;

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
    }
}
