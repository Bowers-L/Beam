using UnityEngine;
using UnityEngine.Events;

using Beam.Core.Player;

using System.Collections.Generic;

namespace Beam.Triggers
{

    public class PressurePlateTrigger : Trigger
    {
        public Animator anim;
        public Transform upPos;
        public Transform downPos;

        public List<string> tags;
        
        //First time? Thought so.
        private bool buttonTriggered;

        public void Start()
        {
            anim = GetComponent<Animator>();
            if (anim == null)
            {
                Debug.LogError("PressurePlateTrigger requires an animator component");
            }

            buttonTriggered = this.transform.localPosition.y <= 0.51;
            anim.SetBool("ButtonTriggered", buttonTriggered);
            if (buttonTriggered)
            {
                activate();
            } else
            {
                deactivate();
            }


        }

        public void Update()
        {
            //I want to put buttonTriggered here, but C# isn't C++ and doesn't support static local variables. Waaaah, waaaah.
            if (this.transform.localPosition.y <= 0.51)
            {
                if (!buttonTriggered)
                {
                    buttonTriggered = true;
                    anim.SetBool("ButtonTriggered", true);
                    activate();
                }
            } else if (buttonTriggered)
            {
                buttonTriggered = false;
                anim.SetBool("ButtonTriggered", false);
                deactivate();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            foreach (string tag in tags) {
                if (other.CompareTag(tag))
                {
                    anim.SetBool("BoxOn", true);
                    if (tag.CompareTo("Player") == 0)
                    {
                        PlayerMoveOnPlat pmp = other.GetComponent<PlayerMoveOnPlat>();
                        pmp.UpdatePlatform(this.transform);
                    }
                    else
                    {
                        other.transform.parent = this.transform;
                    }
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            foreach (string tag in tags) {
                if (other.CompareTag(tag))
                {
                    anim.SetBool("BoxOn", false);
                    if (tag.CompareTo("Player") == 0)
                    {
                        PlayerMoveOnPlat pmp = other.GetComponent<PlayerMoveOnPlat>();
                        pmp.UpdatePlatform(null);
                    }
                    else
                    {
                        other.transform.parent = null;
                    }
                }
            }
        }
    }
}
