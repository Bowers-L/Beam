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
            OR,
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

            InitState();
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

        public void InitState()
        {
            foreach (Trigger t in triggers)
            {
                //Force all triggers below to initialize their state before doing it here.
                if (t is ComplexTrigger)
                {
                    (t as ComplexTrigger).InitState();
                }
            }

            UpdateState();
        }

        public void UpdateState()
        {
            bool newActivated;
            switch (root)
            {
                case Gate.NOT:
                    if (!triggers[0].activated)
                    {
                        Activate();
                    }
                    else
                    {
                        Deactivate();
                    }
                    break;
                case Gate.AND:
                    newActivated = true;
                    foreach (Trigger t in triggers)
                    {
                        newActivated = newActivated && t.activated;
                    }
                    if (newActivated)
                    {
                        Activate();
                    }
                    else
                    {
                        Deactivate();
                    }
                    break;
                case Gate.OR:
                    newActivated = false;
                    foreach (Trigger t in triggers)
                    {
                        newActivated = newActivated || t.activated;
                    }
                    if (newActivated)
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
