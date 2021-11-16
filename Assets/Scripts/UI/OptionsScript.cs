using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionsScript : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;
     private void Start() 
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 1.0f);    
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 1.0f);    
    }
    public void updateVolume() 
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
    }

    public void updateSensitivity()
    {
        PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
    }
}
