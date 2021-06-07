using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Interactables
{
    interface Beamable
    {
        OnBeamAttached(BeamSource source);
    }
}
