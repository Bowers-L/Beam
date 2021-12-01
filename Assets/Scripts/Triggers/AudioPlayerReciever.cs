using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beam.Triggers
{
    public class AudioPlayerReciever : TriggerReceiver
    {
        public AudioSource activatedAudioSource;
        public AudioSource deactivatedAudioSource;
        public bool playAudioOnDeactivate;
        public override void HandleActivated()
        {
            if (activatedAudioSource != null && !activatedAudioSource.isPlaying)
            {
                if (deactivatedAudioSource != null)
                {
                    deactivatedAudioSource.Stop();
                }
                activatedAudioSource.Play();
            }
        }

        public override void HandleDeactivated()
        {
            if (playAudioOnDeactivate && deactivatedAudioSource != null && !deactivatedAudioSource.isPlaying)
            {
                if (activatedAudioSource != null)
                {
                    activatedAudioSource.Stop();
                }
                deactivatedAudioSource.Play();
            }
        }
    }
}

