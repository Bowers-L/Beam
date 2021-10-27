using System.Collections.Generic;

using UnityEngine;

using Beam.Events;

namespace Beam.Triggers
{
    /*
     * A trigger that is set off by multiple other triggers combined with boolean logic.
     */
    class ComplexTrigger : Trigger
    {
        public enum Gate
        {
            NOT,
            AND,
            OR
        }

        public Gate root;
        public List<Trigger> triggers;

        public void Start()
        {
            if (root == Gate.NOT && triggers.Count != 1)
            {
                Debug.LogError("NOT gate can only have one trigger");
            }
            EventManager.StartListening<TriggerActivatedEvent, Trigger>(TriggerUpdated);
            EventManager.StartListening<TriggerDeactivatedEvent, Trigger>(TriggerUpdated);
        }

        public void TriggerUpdated(Trigger trigger)
        {
            if (triggers.Contains(trigger))
            {
                bool activated;
                switch (root)
                {
                    case Gate.NOT:
                        if (trigger.activated == false)
                        {
                            Activate();
                        } else
                        {
                            Deactivate();
                        }
                        break;
                    case Gate.AND:
                        activated = true;
                        foreach (Trigger t in triggers)
                        {
                            activated = activated && t.activated;
                        }
                        if (activated)
                        {
                            Activate();
                        } else
                        {
                            Deactivate();
                        }
                        break;
                    case Gate.OR:
                        activated = false;
                        foreach (Trigger t in triggers)
                        {
                            activated = activated || t.activated;
                        }
                        if (activated)
                        {
                            Activate();
                        }
                        else
                        {
                            Deactivate();
                        }
                        break;
                }
            }
        }
    }
}
