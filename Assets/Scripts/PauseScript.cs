using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PauseScript : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    //public string menuName;
    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)        
        {
            Debug.Log("Toggle");
            if (gameIsPaused)
            {
                Resume();
                return;
            }
            else
            {
                Pause();
                return;
            }
        }
    }
    public void Resume()
    {
        Debug.Log("Resume");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Pause()
    {
        Debug.Log("Pause");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUI.SetActive(true);
        //Time.timeScale = 0.0f;
        gameIsPaused = true;
    }
}
