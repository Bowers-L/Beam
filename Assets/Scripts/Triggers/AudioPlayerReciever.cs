using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beam.Triggers
{
    public class AudioPlayerReciever : TriggerReceiver
    {
        public AudioSource activatedAudioSource;
        public AudioSource deactivatedAudioSource;
 
        public override void HandleActivated()
        {
            if (activatedAudioSource != null && !activatedAudioSource.isPlaying)
            {
                deactivatedAudioSource.Stop();
                activatedAudioSource.Play();
            }
        }

        public override void HandleDeactivated()
        {
            if (deactivatedAudioSource != null && !deactivatedAudioSource.isPlaying)
            {
                activatedAudioSource.Stop();
                deactivatedAudioSource.Play();
            }
        }
    }
}

