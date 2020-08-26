using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameProperties : MonoBehaviour
{

    #region Singleton Architecture

    public static GameProperties Singleton { get; private set; }

    private void Start()
    {
        if(Singleton != null)
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


    #region Utilities

    public static void EnforceGameProperties()
    {
        Physics.gravity = Singleton.GravityConstant;
    }

    #endregion

    #region Properties

    [SerializeField]
    private Vector3 _gravityConstant = new Vector3(0,-9.81f,0);
    public Vector3 GravityConstant {
        get
        {
            return _gravityConstant;
        }
        private set
        {
            _gravityConstant = value;
            if (OnGravityConstantChanged != null)
                OnGravityConstantChanged(value);
        }
    }

    [SerializeField]
    private float _animationCheckBuffer;

    public float AnimationCheckBuffer
    {
        get
        {
            return _animationCheckBuffer;
        }
        private set
        {
            _animationCheckBuffer = value;
        }
    }
    #endregion

    #region Setters of Properties

    //If making a setter for an existing property, then you must ensure that you create an event that all users subscribe to. Otherwise they might not automatically adjust as they might have cached the values.

    #endregion

    #region Events

    public Action<Vector3> OnGravityConstantChanged;

    #endregion

}
