using System;
using System.Collections.Generic;

using UnityEngine;

namespace Beam.Triggers
{
    public class GlassSwitchReceiver : TriggerReceiver
    {
        public GameObject deactivatedPrefab;
        public GameObject activatedPrefab;

        private MeshRenderer renderer;
        private Material activatedMaterial;
        private Material deactivatedMaterial;

        new public void Start()
        {
            base.Start();
            if (!CompareTag("Glass"))
            {
                Debug.LogError("GlassSwitchReceiver attached to non glass object.");
            }

            renderer = GetComponentInChildren<MeshRenderer>();
            if (renderer == null)
            {
                Debug.LogError("Glass must have renderer");
            }

            activatedMaterial = activatedPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            deactivatedMaterial = deactivatedPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterial;
        }

        public override void HandleActivated()
        {
            gameObject.layer = activatedPrefab.layer;
            renderer.material = activatedMaterial;
        }

        public override void HandleDeactivated()
        {
            gameObject.layer = deactivatedPrefab.layer;
            renderer.material = deactivatedMaterial;
        }
    }
}
