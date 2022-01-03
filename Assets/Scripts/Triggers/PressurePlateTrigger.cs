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
        
        public bool buttonTriggered;

        private List<GameObject> objectsOn;

        public void Start()
        {
            anim = GetComponent<Animator>();
            if (anim == null)
            {
                Debug.LogError("PressurePlateTrigger requires an animator component");
            }

            buttonTriggered = this.transform.localPosition.y <= 0.51;
            anim.SetBool("ButtonTriggered", buttonTriggered);

            /* Messes with audio cues
            if (buttonTriggered)
            {
                Activate();
            } else
            {
                Deactivate();
            }
            */

            objectsOn = new List<GameObject>();

        }

        public void Update()
        {
            if (this.transform.localPosition.y <= 0.51)
            {
                if (!buttonTriggered)
                {
                    buttonTriggered = true;
                    anim.SetBool("ButtonTriggered", true);
                    Activate();
                }
            } else if (buttonTriggered)
            {
                buttonTriggered = false;
                anim.SetBool("ButtonTriggered", false);
                Deactivate();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            foreach (string tag in tags) {
                if (other.CompareTag(tag))
                {
                    if (!objectsOn.Contains(other.gameObject))
                    {
                        objectsOn.Add(other.gameObject);
                        anim.SetBool("ObjectOn", true);
                    }

                    //Updating object to move with button
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
                    if (objectsOn.Contains(other.gameObject))
                    {
                        objectsOn.Remove(other.gameObject);
                    }

                    if (objectsOn.Count == 0)
                    {
                        anim.SetBool("ObjectOn", false);
                    }

                    //Updating object to not move with button
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
