using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;

using Beam.Events;
using Beam.Utility;

namespace Beam.Core.Beams
{
    //Things that beams can (potentially) pass through
    public abstract class BeamMedium : MonoBehaviour
    {
        public List<BeamType> allowedTypes;
    }
}
