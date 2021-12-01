using UnityEngine;

namespace Beam.Triggers
{
    class DefaultTrigger : Trigger
    {
        void Start()
        {
            Debug.Log("default");
            Activate();
        }
    }
}
