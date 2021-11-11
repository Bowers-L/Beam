using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    public  GameObject[] subMenus;
    int currentMenu = 0;
    public void StartGame(string log)
    {
            Debug.Log(log);
    }

    public void LoadScene(string name) 
    {
        SceneManager.LoadScene(name);
    }
    public void DisplaySubMenu(int menuNum)
    {
        subMenus[currentMenu].SetActive(false);
        subMenus[menuNum].SetActive(true);
        currentMenu = menuNum;
    }
    public void Quit()
    {
        Application.Quit();
    }


}
