using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beam.Events;

namespace Beam.Triggers
{
    public class NarrationAudioReceiver : MonoBehaviour
    {
        public AudioSource audioSource;
        public Dictionary<Trigger, AudioClip> clipDictionary = new Dictionary<Trigger, AudioClip>();
        public List<Trigger> triggers;
        public List<AudioClip> clips;
        public List<Trigger> activated = new List<Trigger>();

        public Queue<AudioClip> clipQueue = new Queue<AudioClip>();
        //public AudioClip clip;

        private void Awake() 
        {
            activated.Clear();
            EventManager.StartListening<TriggerActivatedEvent, Trigger>(OnActivated);
            if(clips.Count != triggers.Count)
            {
                Debug.LogWarning("You must have the same number of triggers and audio clips");
                return;
            }
            for(int i = 0; i < clips.Count; i++)
            {
                clipDictionary.Add(triggers[i], clips[i]);
            }
        }
        private void Start() 
        {
            
        }
        public void OnDestroy()
        {
            EventManager.StopListening<TriggerActivatedEvent, Trigger>(OnActivated);
        }
        private void Update() 
        {
            if(!audioSource.isPlaying && !(clipQueue.Count == 0))
            {
                audioSource.clip = clipQueue.Dequeue();
                audioSource.Play();
            }
        }
        public void OnActivated(Trigger trigger)
        {
            Debug.Log(trigger);
            if (triggers.Contains(trigger) && !activated.Contains(trigger))
            {
                clipQueue.Enqueue(clipDictionary[trigger]);
                if(!(trigger is DefaultTrigger))
                {
                    activated.Add(trigger);
                }
            }
        }

    }
}
