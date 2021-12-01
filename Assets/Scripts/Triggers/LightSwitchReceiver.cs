using UnityEngine;

namespace Beam.Triggers
{
    public class LightSwitchReceiver : TriggerReceiver
    {
        public Material activatedMaterial;
        public Material deactivatedMaterial;

        private MeshRenderer renderer;
        new public void Start()
        {
            base.Start();

            renderer = GetComponentInChildren<MeshRenderer>();
            if (renderer == null)
            {
                Debug.LogError("Glass must have renderer");
            }
        }

        public override void HandleActivated()
        {
            renderer.material = activatedMaterial;
        }

        public override void HandleDeactivated()
        {
            renderer.material = deactivatedMaterial;
        }
    }
}

