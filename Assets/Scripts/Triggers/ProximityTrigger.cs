using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Beam.Triggers
{
    public class ProximityTrigger : Trigger
    {
        public List<string> tags; //List of all tags which trigger the sensor
      
        public void OnTriggerEnter(Collider other)
        {
            foreach(string tag in tags)
            {
                if(other.CompareTag(tag))
                {
                    Activate();
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            foreach (string tag in tags)
            {
                if (other.CompareTag(tag))
                {
                    Deactivate();
                }
            }
        }
    }
}
