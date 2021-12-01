using UnityEngine;

namespace Beam.Triggers
{
    public class EndingPlatformReceiver : TriggerReceiver
    {
        public float slowMoveSpeed;
        public override void HandleActivated()
        {
            GetComponent<MovingPlatformReceiver>().movespeed = slowMoveSpeed;
        }

        public override void HandleDeactivated()
        {

        }
    }
}

