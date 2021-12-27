using UnityEngine;
using UnityEngine.InputSystem;
using Beam.Core.Player;
using System.Collections.Generic;

namespace Beam.Core.Beams
{
    public class PlayerSwapBeamSource : SwapBeamSource
    {

        new private void Start()
        {
            base.Start();
            if (!GetComponent<Camera>())
            {
                Debug.LogWarning("Attached a player beam source to an object that is not a camera. Was this intentional?");
            }
        }

        public void OnShootSwap(InputAction.CallbackContext ctx)
        {
            if (Time.timeScale == 0.0f)
            {
                return;
            }
            if (ctx.performed)
            {
                ShootBeam(new Ray(transform.position, transform.forward));
            }

            if (ctx.canceled)
            {
                ReleaseBeam();
            }
        }

        public override void ReleaseBeam()
        {
            //Need overridden implementation for this since it involves the character controller.
            if (currTarget != null)
            {
                CharacterController controller = GetComponentInParent<CharacterController>();
                PlayerMovement player = GetComponentInParent<PlayerMovement>();
                controller.enabled = false; //Disable player's collisions.

                Vector3 tempPos = currTarget.transform.position;
                currTarget.transform.position = controller.transform.position;
                controller.transform.position = tempPos + Vector3.down * (controller.height / 4);
                
                controller.transform.forward = currTarget.transform.position - controller.transform.position;   //Face the thing the player just swapped with.
                controller.transform.rotation = Quaternion.Euler(0, controller.transform.rotation.eulerAngles.y, 0);    //zero out the x and z rotation
                Rigidbody targetRb = currTarget.GetComponent<Rigidbody>();
                if (targetRb != null)
                {
                    Vector3 tempVel = player.Velocity;
                    player.Velocity = targetRb.velocity;
                    targetRb.velocity = tempVel;
                }

                controller.enabled = true;

                GetComponent<AudioSource>().Play();
            }

            currTarget = null;

            if (beamEffectInst != null)
            {
                Destroy(beamEffectInst);
            }

            shootingBeam = false;
        }
    }
}