using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    public  GameObject[] subMenus;
    int currentMenu = -1;   //Start with no subMenus active.
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
        if(currentMenu != menuNum)
        {
           // subMenus[currentMenu].SetActive(false);
            subMenus[menuNum].SetActive(true);
            if (currentMenu != -1)
            {
                subMenus[currentMenu].SetActive(false);
            }
            currentMenu = menuNum;
        }
        else
        {
            subMenus[currentMenu].SetActive(false);
            //subMenus[0].SetActive(true);
            currentMenu = -1;
        }
    }
    public void Quit()
    {
        Application.Quit();
    }


}
