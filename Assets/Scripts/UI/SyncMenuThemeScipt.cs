using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SyncMenuThemeScipt : MonoBehaviour
{
    public static float mainThemeTime = 1.0f;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (mainThemeTime != 0)
        {
            /*if(SceneManager.GetActiveScene().name == "TheEnd")     //reset theme for end
            {
                mainThemeTime = 0.0f;
            }*/
            audioSource.time = mainThemeTime;
        }
    }
    private void Update() {
        SyncMenuThemeScipt.mainThemeTime = audioSource.time;
    }
   /* private void OnDestroy() {
        Debug.Log(audioSource.time);
        SyncMenuThemeScipt.mainThemeTime = audioSource.time;
        Debug.Log(mainThemeTime);
    }*/
}
