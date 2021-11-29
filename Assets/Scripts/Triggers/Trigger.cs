using UnityEngine;
using UnityEngine.Events;

using Beam.Events;

namespace Beam.Triggers
{

    public class TriggerActivatedEvent : UnityEvent<Trigger> { }

    public class TriggerDeactivatedEvent : UnityEvent<Trigger> { }

    public abstract class Trigger : MonoBehaviour
    {
        public bool activated
        {
            get;
            private set;
        }

        private void Awake()
        {
            activated = false;
        }

        public virtual void Activate()
        {
            //Only trigger on edge (mainly so that audio cues don't play constantly)
            if (!activated)
            {
                activated = true;
                EventManager.InvokeEvent<TriggerActivatedEvent, Trigger>(this);
            }
        }

        public virtual void Deactivate()
        {
            //Only trigger on edge (mainly so that audio cues don't play constantly)
            if (activated)
            {
                activated = false;
                EventManager.InvokeEvent<TriggerDeactivatedEvent, Trigger>(this);
            }
        }
    }
}
