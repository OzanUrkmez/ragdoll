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

    [SerializeField]
    private int forwardAdjustmentIndex, backwardAdjustmentIndex, rightAdjustmentIndex, leftAdjustmentIndex;

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

        //Can use adjustment force code at other places too!

        float forwardSpeedMagnitude = Vector3.Project(forwardForce, objectAppliedTo.GetRecentNetSpeed()).magnitude;
        float rightSpeedMagnitude = Vector3.Project(rightForce, objectAppliedTo.GetRecentNetSpeed()).magnitude;
        float leftSpeedMagnitude = Vector3.Project(leftForce, objectAppliedTo.GetRecentNetSpeed()).magnitude;
        float backwardSpeedMagnitude = Vector3.Project(backwardForce, objectAppliedTo.GetRecentNetSpeed()).magnitude;

        //TODO doing it with just if might be faster becasue there is no addition with vector3.zero. Do diagnostic if releasing this code separately.
        Vector3 resultantForce = (forwardSpeedMagnitude > forwardSpeeds[currentForwardIndex] ? Vector3.zero : forwardForce)
            + (rightSpeedMagnitude > rightSpeeds[currentRightIndex] ? Vector3.zero : rightForce)
            + (leftSpeedMagnitude > leftSpeeds[currentLeftIndex] ? Vector3.zero : leftForce)
            + (backwardSpeedMagnitude > backwardSpeeds[currentBackwardIndex] ? Vector3.zero : backwardForce);

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