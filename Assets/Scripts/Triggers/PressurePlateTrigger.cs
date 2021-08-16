using UnityEngine;
using UnityEngine.Events;

using Beam.Events;

namespace Beam.Triggers
{

    public class PressurePlateTrigger : Trigger
    {
        public Animator anim;
        public Transform upPos;
        public Transform downPos;
        
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
            if (this.transform.localPosition.y <= 0.51 && !buttonTriggered)
            {
                buttonTriggered = true;
                anim.SetBool("ButtonTriggered", true);
                activate();
            } else if (buttonTriggered)
            {
                buttonTriggered = false;
                anim.SetBool("ButtonTriggered", false);
                deactivate();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PressurePlateTarget"))
            {
                anim.SetBool("BoxOn", true);
                other.transform.parent = this.transform;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PressurePlateTarget"))
            {
                anim.SetBool("BoxOn", false);
                other.transform.parent = null;
            }
        }
    }
}
