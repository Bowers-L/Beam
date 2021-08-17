using UnityEngine;
using UnityEngine.Events;

using Beam.Events;

namespace Beam.Triggers
{

    public class TriggerActivatedEvent : UnityEvent<Trigger> { }

    public class TriggerDeactivatedEvent : UnityEvent<Trigger> { }

    public abstract class Trigger : MonoBehaviour
    {
        public virtual void activate()
        {
            EventManager.InvokeEvent<TriggerActivatedEvent, Trigger>(this);
        }

        public virtual void deactivate()
        {
            EventManager.InvokeEvent<TriggerDeactivatedEvent, Trigger>(this);
        }
    }
}
