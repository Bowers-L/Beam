using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beam.Events;

namespace Beam.Triggers
{
    public class TimeTrigger : Trigger
    {
        public float time;

        public void Start() 
        {
            StartCoroutine(wait());   
        }
        public IEnumerator wait()
        {
            yield return new WaitForSeconds(time);
            Debug.Log("here");
            Activate();
        }
    }
}

