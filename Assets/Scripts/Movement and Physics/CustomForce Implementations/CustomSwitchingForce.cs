using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CustomSwitchingForce : MonoBehaviour, ICustomForceImplementation
{

    private void Start()
    {
        new CustomForce(currentAppliedObject, this, isPure, float.NegativeInfinity);
    }

    [SerializeField]
    private ForceObject currentAppliedObject;

    [SerializeField]
    private bool isPure;

    [Serializable]
    struct SwitchingForce
    {
        public Vector3 forceVector;
        public float time;
    }

    [SerializeField]
    private SwitchingForce[] switchingForces;

    [SerializeField]
    private bool repeatForever = true;

    [SerializeField]
    private int repetitionTimes = 1;


    private int currentIndex = 0;

    private float currentTime = 0;

    private CustomForce currentForceInstance;

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        if(currentTime > switchingForces[currentIndex].time)
        {
            if (!repeatForever)
            {
                repetitionTimes--;
                if(repetitionTimes < 1)
                {
                    objectAppliedTo.QueueRemoveForce(parentForce);
                }
            }
            currentTime = 0;
            currentIndex = (currentIndex + 1) % switchingForces.Length;
        }
        currentTime += Time.fixedDeltaTime;

        return switchingForces[currentIndex].forceVector;
    }
}