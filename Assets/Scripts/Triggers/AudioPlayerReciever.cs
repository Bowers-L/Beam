using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beam.Triggers
{
    public class AudioPlayerReciever : TriggerReceiver
    {
        public AudioSource audioSource;
        public bool stopOnDeactivate;
 
        public override void HandleActivated()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();

            }
        }

        public override void HandleDeactivated()
        {
            if (stopOnDeactivate)
            {
                audioSource.Stop();
            }
        }
    }
}

