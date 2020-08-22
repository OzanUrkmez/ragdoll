using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CustomMotorMovementForceObject : ICustomForceImplementation
{
    
    [SerializeField]
    private float[] forwardSpeeds, backwardSpeeds, rightSpeeds, leftSpeeds;

    private int currentForwardIndex, currentBackwardIndex, currentRightIndex, currentLeftIndex;

    [SerializeField]
    private Transform motorMovementTransform;

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        return motorMovementTransform.forward.normalized * forwardSpeeds[currentForwardIndex]
            + motorMovementTransform.right * rightSpeeds[currentRightIndex] 
            - motorMovementTransform.right * leftSpeeds[currentLeftIndex]
            - motorMovementTransform.forward * backwardSpeeds[currentBackwardIndex];
    }

}