using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidMotorObject : MonoBehaviour
{

    [SerializeField]
    CharacterController characterController;

    [SerializeField]
    ForceObject affectedForceObject;

    [SerializeField]
    CustomMotorMovementForceObject motorMovementForceObject;

    [SerializeField]
    Animator humanoidAnimator; //TODO deal with this last. pls deal with it tho.

    private CustomForce humanoidForce;

    private void Start()
    {
        //apply humanoid force :o
        humanoidForce = new CustomForce(affectedForceObject, motorMovementForceObject, true, float.NegativeInfinity);
    }


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
}
