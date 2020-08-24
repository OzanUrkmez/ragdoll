using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RotaryHeart.Lib.SerializableDictionary;

public class HumanoidMotorObject : MonoBehaviour
{

    [SerializeField]
    ForceObject affectedForceObject;

    [SerializeField]
    CustomMotorMovementForceObject motorMovementForceObject;

    [SerializeField]
    private float levitationTolerance = 0.1f;


    [SerializeField]
    private SerializableDictionaryBase<KeyCode, ComponentExclusiveAdjustableForceInformationWithKnownObject> acceptedInstantaniousForceInputs;

    [SerializeField]
    Animator humanoidAnimator; //TODO deal with this last. pls deal with it tho.

    private CustomForce humanoidForce;

    private void Start()
    {
        //apply humanoid force :o
        motorMovementForceObject.InitializeNonSerializedFields(CanExertMotorForce);
        humanoidForce = new CustomForce(affectedForceObject, motorMovementForceObject, true, float.NegativeInfinity);
    }


    #region Walk and Run Updates

    //TODO this is kind of inefficient right now, as we do projections, but it is needed for abstraction to single vector. in future remove this if too bad for performance.

    /// <summary>
    /// this wont make sense if you dont do octagonal vectors. Also a player cannot walk and run at the same time.
    /// </summary>
    public void OctagonalWalkUpdate(Vector2 v) //Can make instantanious animations with this. for now. or actually might look good from the getgo.
    {

        //up
        if(v.y > 0)
        {
            motorMovementForceObject.UpdateCurrentForwardIndex(1);
        }
        else
        {
            motorMovementForceObject.UpdateCurrentForwardIndex(0);
        }

        //right
        if (v.x > 0)
        {
            motorMovementForceObject.UpdateCurrentRightIndex(1);
        }
        else
        {
            motorMovementForceObject.UpdateCurrentRightIndex(0);
        }

        //down
        if (v.y < 0)
        {
            motorMovementForceObject.UpdateCurrentBackwardIndex(1);
        }
        else
        {
            motorMovementForceObject.UpdateCurrentBackwardIndex(0);
        }

        //left
        if (v.x < 0)
        {
            motorMovementForceObject.UpdateCurrentLeftIndex(1);
        }
        else
        {
            motorMovementForceObject.UpdateCurrentLeftIndex(0);
        }
    }


    /// <summary>
    /// this wont make sense if you dont do octagonal vectors. Also a player cannot walk and run at the same time.
    /// </summary>
    public void OctagonalRunUpdate(Vector2 v) //Can make instantanious animations with this. for now. or actually might look good from the getgo.
    {
        //up
        if (v.y > 0)
        {
            motorMovementForceObject.UpdateCurrentForwardIndex(2);
        }
        else
        {
            motorMovementForceObject.UpdateCurrentForwardIndex(0);
        }

        //right
        if (v.x > 0)
        {
            motorMovementForceObject.UpdateCurrentRightIndex(2);
        }
        else
        {
            motorMovementForceObject.UpdateCurrentRightIndex(0);
        }

        //down
        if (v.y < 0)
        {
            motorMovementForceObject.UpdateCurrentBackwardIndex(2);
        }
        else
        {
            motorMovementForceObject.UpdateCurrentBackwardIndex(0);
        }

        //left
        if (v.x < 0)
        {
            motorMovementForceObject.UpdateCurrentLeftIndex(2);
        }
        else
        {
            motorMovementForceObject.UpdateCurrentLeftIndex(0);
        }
    }

    #endregion

    public void ExecuteInstantaniousForce(KeyCode forceKeyCode)
    {
        if (acceptedInstantaniousForceInputs.ContainsKey(forceKeyCode))
        {
            ComponentExclusiveAdjustableForceInformationWithKnownObject info = acceptedInstantaniousForceInputs[forceKeyCode];
            CustomTraditionalForce force = new CustomTraditionalForce(info.v);
            force.ApplyForce(affectedForceObject, info.isPure, info.infiniteTimeForce ? float.NegativeInfinity : info.applyTime);
        }
    }

    private bool CanExertMotorForce()
    {
        return Vector3.Project(affectedForceObject.GetRecentNetAcceleration(), -motorMovementForceObject.GetGroundDir()).magnitude < levitationTolerance &&
             Vector3.Project(affectedForceObject.GetRecentNetSpeed(), -motorMovementForceObject.GetGroundDir()).magnitude < levitationTolerance;
    }

    #region Getters

    public List<KeyCode> GetAcceptedInstantKeyCodes()
    {
        return new List<KeyCode>(acceptedInstantaniousForceInputs.Keys);
    }

    #endregion

}
