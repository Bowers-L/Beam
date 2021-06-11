using UnityEngine;
using UnityEngine.InputSystem;

namespace Beam.Core.Beams
{
    public class PlayerBeamSource : BeamSource
    {

        private void Start()
        {
            if (!GetComponent<Camera>())
            {
                Debug.LogWarning("Attached a player beam source to an object that is not a camera. Was this intentional?");
            }
        }

        public void OnShoot(InputAction.CallbackContext ctx)
        {
            Debug.Log("Shooting");
            activateBeam(new Ray(transform.position, transform.forward));
        }
    }
}