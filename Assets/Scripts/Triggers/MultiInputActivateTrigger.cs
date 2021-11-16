using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Beam.Events;
/*
 * This is a special type of trigger which activates/deactivates every
 * time any one of its input triggers activates/deactivates
 */
namespace Beam.Triggers
{

    public class MultiInputActivateTrigger : Trigger
    {
        public List<Trigger> triggers;
        public int numActivatedTriggers;
        public int newActivatedTriggers = 0;

        public override void Activate()
        {
            EventManager.InvokeEvent<TriggerActivatedEvent, Trigger>(this);
        }

        public override void Deactivate()
        {
                EventManager.InvokeEvent<TriggerDeactivatedEvent, Trigger>(this);
        }

        public void Start()
        {
            EventManager.StartListening<TriggerActivatedEvent, Trigger>(TriggerUpdated);
            EventManager.StartListening<TriggerDeactivatedEvent, Trigger>(TriggerUpdated);
        }

        public void OnDestroy()
        {
            EventManager.StopListening<TriggerActivatedEvent, Trigger>(TriggerUpdated);
            EventManager.StopListening<TriggerDeactivatedEvent, Trigger>(TriggerUpdated);
        }

        public void TriggerUpdated(Trigger trigger)
        {
            if (triggers.Contains(trigger))
            {
                UpdateState();
            }
        }

        public void UpdateState()
        {
            foreach (Trigger t in triggers)
            {
                if(t.activated)
                {
                    newActivatedTriggers++;
                }
                if (newActivatedTriggers > numActivatedTriggers)
                {
                    Activate();
                }
                else if (newActivatedTriggers < numActivatedTriggers)
                {
                    Deactivate();
                }
                numActivatedTriggers = newActivatedTriggers;
                newActivatedTriggers = 0;
            }
        }

    }
}
