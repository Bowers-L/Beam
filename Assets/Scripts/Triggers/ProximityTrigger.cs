using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Beam.Triggers
{
    public class ProximityTrigger : Trigger
    {
        public List<string> tags; //List of all tags which trigger the sensor
        void Start()
        {
        
        }
        void Update()
        {
        
        }
        public void OnTriggerEnter(Collider other)
        {
            foreach(string tag in tags)
            {
                if(other.CompareTag(tag))
                {
                    activate();
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            foreach (string tag in tags)
            {
                if (other.CompareTag(tag))
                {
                    deactivate();
                }
            }
        }
    }
}
