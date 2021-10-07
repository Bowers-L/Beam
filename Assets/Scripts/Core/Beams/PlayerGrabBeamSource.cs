using UnityEngine;
using UnityEngine.InputSystem;
using Beam.Core.Player;

namespace Beam.Core.Beams
{
    class PlayerGrabBeamSource : GrabBeamSource
    {
        new private void Start()
        {
            base.Start();
            if (!GetComponent<Camera>())
            {
                Debug.LogWarning("Attached a player beam source to an object that is not a camera. Was this intentional?");
            }
        }

        public void OnShootGrab(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                ShootBeam(new Ray(transform.position, transform.forward));
            }

            if (ctx.canceled)
            {
                ReleaseBeam();
            }
        }
    }
}
