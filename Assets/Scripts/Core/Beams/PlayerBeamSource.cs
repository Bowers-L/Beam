using UnityEngine;
using UnityEngine.InputSystem;
using Beam.Core.Player;

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
            if (ctx.performed)
            {
                ActivateBeam(new Ray(transform.position, transform.forward));
            } 
            
            if (ctx.canceled)
            {
                DeactivateBeam();
            }
        }

        public void OnTravel(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Debug.Log("Beam Traveled");
                TravelBeam(new Ray(transform.position, transform.forward));
            }
        }

        public override void TravelBeam(Ray beamRay)
        {
            BeamTarget target = currTarget != null ? currTarget : FindTarget(beamRay);
            if (target != null)
            {
                CharacterController controller = GetComponentInParent<CharacterController>();
                PlayerMovement player = GetComponentInParent<PlayerMovement>();
                controller.enabled = false; //Disable player's collisions.

                //Teleport the player (probably want to start some kind of coroutine/animation here)
                Vector3 tempPos = controller.transform.position;
                controller.transform.position = target.transform.position;
                target.transform.position = tempPos;

                Rigidbody targetRb = target.GetComponent<Rigidbody>();
                if (targetRb != null)
                {
                    Vector3 tempVel = player.Velocity;
                    player.Velocity = targetRb.velocity;
                    targetRb.velocity = tempVel;
                }

                DeactivateBeam();
                controller.enabled = true;
            }
        }
    }
}