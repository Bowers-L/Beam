using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour
{
    public string[] scenes;    
    
    public void LoadScene(int sceneNum) 
    {
        SceneManager.LoadScene(scenes[sceneNum]);
    }

    public void loadStart()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
