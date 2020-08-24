using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CustomMotorMovementForceObject : ICustomForceImplementation
{

    [SerializeField]
    private float[] maximumSpeedsPerIndex;

    [SerializeField]
    private float[] forwardAccelerations, backwardAccelerations, rightAccelerations, leftAccelerations;

    [SerializeField]
    private float[] adjustmentAccelerations; //TODO can play with these to create interesting effects?

    private int currentForwardIndex, currentBackwardIndex, currentRightIndex, currentLeftIndex;

    [SerializeField]
    private Transform motorMovementTransform;

    private Func<bool> canExertMotorForceCheck;

    private Vector3 groundDir = new Vector3(0, -1, 0); //TODO make this adjustable to walk on walls. maybe. can just use down direction from player too, if going for such a thing.



    private int currentMaxIndex = 0;

    [SerializeField]
    private float NoMovementCutoff = 1;

    #region Initializaiton

    //TODO this needs more and better work.

    //public void InitializeSerializedFields()
    //{
        
    //}

    public void InitializeNonSerializedFields(Func<bool> groundedCheck)
    {
        canExertMotorForceCheck = groundedCheck;
    }

    #endregion


    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        if (!canExertMotorForceCheck())
            return Vector3.zero;

        Vector3 forwardForce = motorMovementTransform.forward * forwardAccelerations[currentForwardIndex];
        Vector3 rightForce = motorMovementTransform.right * backwardAccelerations[currentRightIndex];
        Vector3 leftForce = -motorMovementTransform.right * rightAccelerations[currentLeftIndex];
        Vector3 backwardForce = -motorMovementTransform.forward * leftAccelerations[currentBackwardIndex];

        //TODO add grounded check

        Vector3 adjustedWalkPlaneSpeed = Vector3.ProjectOnPlane(objectAppliedTo.GetRecentNetSpeed(), groundDir);

        Vector3 resultantForce = forwardForce + rightForce + leftForce + backwardForce;

        if(adjustedWalkPlaneSpeed.magnitude < NoMovementCutoff && resultantForce.magnitude < NoMovementCutoff)
        {
            objectAppliedTo.DirectAdjustAddSpeed(-adjustedWalkPlaneSpeed);
            return Vector3.zero;
        }

        if (adjustedWalkPlaneSpeed.magnitude > maximumSpeedsPerIndex[currentMaxIndex])
        {
            resultantForce += -adjustedWalkPlaneSpeed.normalized * adjustmentAccelerations[currentMaxIndex];
        }

        //TODO doing it with just if might be faster becasue there is no addition with vector3.zero. Do diagnostic if releasing this code separately.

        return resultantForce;
    }

    #region Setters

    public void UpdateCurrentForwardIndex(int index)
    {
        currentForwardIndex = index;
        RecalculateMaxIndex();
    }

    public void UpdateCurrentBackwardIndex(int index)
    {
        currentBackwardIndex = index;
        RecalculateMaxIndex();
    }

    public void UpdateCurrentRightIndex(int index)
    {
        currentRightIndex = index;
        RecalculateMaxIndex();
    }

    public void UpdateCurrentLeftIndex(int index)
    {
        currentLeftIndex = index;
        RecalculateMaxIndex();
    }

    #endregion

    #region Housekeeping Logic

    private void RecalculateMaxIndex()
    {
        currentMaxIndex = Mathf.Max(currentForwardIndex, currentRightIndex, currentBackwardIndex, currentLeftIndex);
    }

    #endregion

    #region Getters

    public Vector3 GetGroundDir()
    {
        return groundDir;
    }

    #endregion

}