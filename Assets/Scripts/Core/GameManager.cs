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

    public bool debug;
}
