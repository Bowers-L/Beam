using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beam.Triggers
{
    public class TextDisplayReciever : TriggerReceiver
    {
        public string info;
        public GameObject textBox; //PSA: Do NOT put this script on the textbox or it will deactivate itself.
        public bool displayOnce; 
        public float timer; //set to 0 to display until deactivated, or set to a value to have the text automatically vanish after this time
        public bool display = true;
    // Start is called before the first frame update
        void Awake()
        {
            textBox.SetActive(false);
        }

        // Update is called once per frame
        public override void HandleActivated() 
        {
            Debug.Log("activated");
            if (display) 
            {
                textBox.GetComponent<UnityEngine.UI.Text>().text = info;
                textBox.SetActive(true);
                if(timer > 0)
                {
                    Debug.Log(timer);
                     StartCoroutine(removeText());
                }
            }
        }

        public override void HandleDeactivated() 
        {
            if(timer == 0)
            {
                textBox.SetActive(false);
                if(displayOnce)
                {
                    display = false;
                }
            }
            
        }

        private IEnumerator removeText() 
        {
            Debug.Log(timer);
            yield return new WaitForSeconds(timer);
            Debug.Log(timer);
            textBox.SetActive(false);
            if(displayOnce)
            {
                display = false;
            }
        }
    
    }
}