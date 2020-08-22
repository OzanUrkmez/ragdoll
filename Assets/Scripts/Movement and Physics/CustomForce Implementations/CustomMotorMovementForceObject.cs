using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CustomMotorMovementForceObject : ICustomForceImplementation
{
    
    [SerializeField]
    private float[] forwardSpeeds, backwardSpeeds, rightSpeeds, leftSpeeds;

    [SerializeField]
    private float[] forwardAccelerations, backwardAccelerations, rightAccelerations, leftAccelerations;

    private int currentForwardIndex, currentBackwardIndex, currentRightIndex, currentLeftIndex;
   

    [SerializeField]
    private Transform motorMovementTransform;

    private Func<bool> isGroundedCheck;

    #region Initializaiton

    //TODO this needs more and better work.

    //public void InitializeSerializedFields()
    //{
        
    //}

    public void InitializeNonSerializedFields(Func<bool> groundedCheck)
    {
        isGroundedCheck = groundedCheck;
    }

    #endregion


    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        Vector3 forwardForce = motorMovementTransform.forward * forwardAccelerations[currentForwardIndex];
        Vector3 rightForce = motorMovementTransform.right * backwardAccelerations[currentRightIndex];
        Vector3 leftForce = -motorMovementTransform.right * rightAccelerations[currentLeftIndex];
        Vector3 backwardForce = -motorMovementTransform.forward * leftAccelerations[currentBackwardIndex];

        //TODO doing it with just if might be faster becasue there is no addition with vector3.zero. Do diagnostic if releasing this code separately.

        Vector3 resultantForce = (Vector3.Project(forwardForce, objectAppliedTo.GetRecentNetSpeed()).magnitude > forwardSpeeds[currentForwardIndex] ? Vector3.zero : forwardForce)
            + (Vector3.Project(rightForce, objectAppliedTo.GetRecentNetSpeed()).magnitude > rightSpeeds[currentRightIndex] ? Vector3.zero : rightForce)
            + (Vector3.Project(leftForce, objectAppliedTo.GetRecentNetSpeed()).magnitude > leftSpeeds[currentLeftIndex] ? Vector3.zero : leftForce)
            + (Vector3.Project(backwardForce, objectAppliedTo.GetRecentNetSpeed()).magnitude > backwardSpeeds[currentBackwardIndex] ? Vector3.zero : backwardForce);

        return resultantForce;
    }

    public void UpdateCurrentForwardIndex(int index)
    {
        currentForwardIndex = index;
    }

    public void UpdateCurrentBackwardIndex(int index)
    {
        currentBackwardIndex = index;
    }

    public void UpdateCurrentRightIndex(int index)
    {
        currentRightIndex = index;
    }

    public void UpdateCurrentLeftIndex(int index)
    {
        currentLeftIndex = index;
    }

}