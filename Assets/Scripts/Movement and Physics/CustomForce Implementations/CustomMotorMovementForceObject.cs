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
        Vector3 forwardForce = motorMovementTransform.forward * forwardSpeeds[currentForwardIndex];
        Vector3 rightForce = motorMovementTransform.right * rightSpeeds[currentRightIndex];
        Vector3 leftForce = -motorMovementTransform.right * leftSpeeds[currentLeftIndex];
        Vector3 backwardForce = -motorMovementTransform.forward * backwardSpeeds[currentBackwardIndex];

        //TODO doing it with just if might be faster becasue there is no addition with vector3.zero. Do diagnostic if releasing this code separately.
        return (Vector3.Project(forwardForce, objectAppliedTo.GetRecentNetSpeed()).magnitude > forwardSpeeds[currentForwardIndex] ? Vector3.zero : forwardForce)
            + (Vector3.Project(rightForce, objectAppliedTo.GetRecentNetSpeed()).magnitude > rightSpeeds[currentRightIndex] ? Vector3.zero : rightForce)
            + (Vector3.Project(leftForce, objectAppliedTo.GetRecentNetSpeed()).magnitude > leftSpeeds[currentLeftIndex] ? Vector3.zero : leftForce)
            + (Vector3.Project(backwardForce, objectAppliedTo.GetRecentNetSpeed()).magnitude > backwardSpeeds[currentBackwardIndex] ? Vector3.zero : backwardForce);
    }

    public void UpdateCurrentForwardIndex()

}