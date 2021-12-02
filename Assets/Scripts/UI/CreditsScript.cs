using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Beam.Utility;

public class CreditsScript : MonoBehaviour
{
    public float scrollSpeed = 5.0f;
    public int maxY;
    public GameObject Timer;
    public TextMeshProUGUI timerText;

    private void Start()
    {
        GameManager.StopTimer();
        timerText.text = UnityEngineExt.FormatTime(GameManager.time);
        Timer.SetActive(GameManager.showTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0) return;
        transform.Translate(Vector3.up * scrollSpeed);
        if(transform.position.y > maxY)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
