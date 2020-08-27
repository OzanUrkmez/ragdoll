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
    private float gravityReorientSpeed = 1;

    [SerializeField]
    private float[] forwardAccelerations, backwardAccelerations, rightAccelerations, leftAccelerations;

    [SerializeField]
    private float maximumNonGroundedSpeedAdjustment;

    private Vector3 priorAdjustment;


    [SerializeField]
    private float[] adjustmentAccelerations; //TODO can play with these to create interesting effects?

    private int currentForwardIndex, currentBackwardIndex, currentRightIndex, currentLeftIndex;

    [SerializeField]
    private Transform motorMovementTransform;

    private Func<bool> canExertMotorForceCheck;

    private Vector3 groundDir = new Vector3(0, -1, 0); //TODO make this adjustable to walk on walls. maybe. can just use down direction from player too, if going for such a thing.

    private MonoBehaviour enumeratorObject;

    private int currentMaxIndex = 0;

    private bool isOrienting = false;


    [SerializeField]
    private float NoMovementCutoff = 1;

    #region Initializaiton

    //TODO this needs more and better work.

    //public void InitializeSerializedFields()
    //{
        
    //}

    public void InitializeNonSerializedFields(Func<bool> groundedCheck, ForceObject orientationHandled = null, MonoBehaviour unityEnumeratorObject = null)
    {
        canExertMotorForceCheck = groundedCheck;
        if (orientationHandled)
        {
            orientationHandled.onGravityAdjusted += OnMotorObjectGravityChanged;
            enumeratorObject = unityEnumeratorObject;
        }

    }

    #endregion

    #region Motor Object Orientation

    private void OnMotorObjectGravityChanged(ForceObject obj, Vector3 newGravity)
    {

        isOrienting = true;

        groundDir = newGravity.normalized;

        Quaternion lookQuaternion = Quaternion.LookRotation(motorMovementTransform.forward, -newGravity);

        enumeratorObject.StartCoroutine(ReOrientEnumeration(lookQuaternion));
    } 

    private IEnumerator ReOrientEnumeration(Quaternion lookQuaternion)
    {
        float t = 0;
        while (t < 1)
        {
            t += gravityReorientSpeed * Time.fixedDeltaTime;
            motorMovementTransform.rotation = Quaternion.Lerp(motorMovementTransform.rotation, lookQuaternion, t);
            yield return new WaitForFixedUpdate();
        }

        motorMovementTransform.rotation = lookQuaternion;

        motorMovementTransform.transform.up = -groundDir;

        isOrienting = false;
    }

    #endregion


    private bool pureSpeedDirty = false;

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        if (!canExertMotorForceCheck())
        {
            //NOT GROUNDED

            //do pure speed efforts

            //Vector3 currentSpeed 

            return Vector3.zero;
        }

        //GROUNDED

        if (pureSpeedDirty)
        {
            UndirtyPureSpeed();
        }

        Vector3 forwardForce = motorMovementTransform.forward * forwardAccelerations[currentForwardIndex];
        Vector3 rightForce = motorMovementTransform.right * backwardAccelerations[currentRightIndex];
        Vector3 leftForce = -motorMovementTransform.right * rightAccelerations[currentLeftIndex];
        Vector3 backwardForce = -motorMovementTransform.forward * leftAccelerations[currentBackwardIndex];

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

    private void UndirtyPureSpeed()
    {
        pureSpeedDirty = false;
        priorAdjustment = Vector3.zero;
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

    public bool IsOrienting()
    {
        return isOrienting;
    }

    #endregion

}