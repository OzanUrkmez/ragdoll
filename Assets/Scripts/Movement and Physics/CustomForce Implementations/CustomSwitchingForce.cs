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

    [SerializeField]
    private ICustomForceImplementation[] forces;

    [SerializeField]
    private float[] switchTimes;

    [SerializeField]
    private bool repeatForever = true;

    [SerializeField]
    private int repetitionTimes = 1;


    private int currentIndex = 0;

    private float currentTime = 0;

    private CustomForce currentForceInstance;

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        currentTime += Time.fixedDeltaTime;
        if(currentTime > switchTimes[currentIndex])
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
            currentIndex = (currentIndex + 1) % switchTimes.Length;
        }

        return forces[currentIndex].GetCurrentForceVector(parentForce, objectAppliedTo);
    }
}