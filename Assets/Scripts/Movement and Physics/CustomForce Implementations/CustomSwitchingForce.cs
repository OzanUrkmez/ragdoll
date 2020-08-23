using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CustomSwitchingForce : ICustomForceImplementation
{

    [SerializeField]
    private ICustomForceImplementation[] forces;

    [SerializeField]
    private float[] switchTimes;

    private int currentIndex = 0;

    private float currentTime = 0;

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {

        

        currentTime += Time.fixedDeltaTime;
        if(currentTime > switchTimes[currentIndex])
        {
            currentTime = 0;
            currentIndex = (currentIndex + 1) % switchTimes.Length;
        }

        return forces[currentIndex].GetCurrentForceVector(parentForce, objectAppliedTo);
    }
}