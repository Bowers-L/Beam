using UnityEngine;
using UnityEngine.Events;

using Beam.Events;

namespace Beam.Triggers
{
    public abstract class TriggerReceiver : MonoBehaviour
    {
        public Trigger listenee;

        public void Start()
        {
            EventManager.StartListening<TriggerActivatedEvent, Trigger>(OnActivated);
            EventManager.StartListening<TriggerDeactivatedEvent, Trigger>(OnDeactivated);
        }

        public void OnDestroy()
        {
            EventManager.StartListening<TriggerActivatedEvent, Trigger>(OnActivated);
            EventManager.StopListening<TriggerActivatedEvent, Trigger>(OnDeactivated);
        }

        public void OnActivated(Trigger trigger)
        {
            if (trigger == listenee)
            {
                HandleActivated();
            }
        }
        public void OnDeactivated(Trigger trigger)
        {
            if (trigger == listenee)
            {
                HandleDeactivated();
            }
        }

        public abstract void HandleActivated();
        public abstract void HandleDeactivated();
    }
}
