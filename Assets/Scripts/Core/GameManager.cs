using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beam.Core.Player;

public class GameManager : MonoBehaviour
{

    #region Singleton Code
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (this == _instance) { _instance = null; }
    }
    #endregion

    public static bool debug;
    public static bool showTimer = false;
    public static bool runTimer = false;
    public static float time;

    public static void StartTimer()
    {
        Debug.Log("Started Timer");
        time = 0f;
        showTimer = true;
        runTimer = true;
    }

    void Start()
    {
        Application.targetFrameRate = 60;   //Target 60 fps  
    }

    void Update()
    {
        if (runTimer)
        {
            time += Time.deltaTime;
        }
    }

    public static void ResumeTimer()
    {
        runTimer = true;
    }

    public static void StopTimer()
    {
        runTimer = false;
    }
}
