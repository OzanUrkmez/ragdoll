using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    #region Singleton Architecture

    public static GameManager Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
    }

    private void OnDestroy()
    {
        if (Singleton == this)
            Singleton = null;
    }

    #endregion

    #region Initialization

    void Start()
    {
        //enforce things like gravity settings put out by the game designer.
        GameProperties.EnforceGameProperties();   


    }

    #endregion

    [SerializeField]
    private float levelDefaultDragValue = 25f;

    public float GetLevelDefaultDragValue()
    {
        return levelDefaultDragValue;
    }
}
