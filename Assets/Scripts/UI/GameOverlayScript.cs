using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Beam.Utility;

public class GameOverlayScript : MonoBehaviour
{
    public GameObject timer;
    public TextMeshProUGUI timerText;

    void Start()
    {
        timer.SetActive(GameManager.showTimer);
    }
    // Update is called once per frame
    void Update()
    {
        timerText.text = UnityEngineExt.FormatTime(GameManager.time);
    }
}
