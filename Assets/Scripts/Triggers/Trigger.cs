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
            Deactivate();
        }

        public virtual void Activate()
        {
            activated = true;
            EventManager.InvokeEvent<TriggerActivatedEvent, Trigger>(this);
        }

        public virtual void Deactivate()
        {
            activated = false;
            EventManager.InvokeEvent<TriggerDeactivatedEvent, Trigger>(this);
        }
    }
}
