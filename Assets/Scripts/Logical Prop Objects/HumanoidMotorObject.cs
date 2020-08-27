using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class HumanoidMotorObject : MonoBehaviour
{

    private float animChecksSeconds;

    [SerializeField]
    ForceObject affectedForceObject;

    [SerializeField]
    CustomMotorMovementForceObject motorMovementForceObject;

    [SerializeField]
    private float levitationTolerance = 0.1f;


    [SerializeField]
    private SerializableDictionaryBase<KeyCode, HumanoidComponentExclusiveAdjustableForceInformationWithKnownObject> acceptedInstantaniousGroundedForceInputs;

    [SerializeField]
    private SerializableDictionaryBase<KeyCode, HumanoidComponentExclusiveAdjustableForceInformationWithKnownObject> acceptedInstantaniousLevitatingForceInputs;

    private List<KeyCode> cooldownAdjustableGroundedForceKeys = new List<KeyCode>();
    private List<KeyCode> cooldownAdjustableLevitatingForceKeys = new List<KeyCode>();

    [SerializeField]
    Animator humanoidAnimator; //TODO deal with this last. pls deal with it tho.

    private CustomForce humanoidForce;

    private Collider humanoidCollider;

    private void Start()
    {

        //apply humanoid force :o
        motorMovementForceObject.InitializeNonSerializedFields(CanExertMotorForce, affectedForceObject, this);
        humanoidForce = new CustomForce(affectedForceObject, motorMovementForceObject, true, float.NegativeInfinity);

        humanoidCollider = affectedForceObject.GetComponent<Collider>();

        foreach(var key in acceptedInstantaniousGroundedForceInputs.Keys)
        {
            if(acceptedInstantaniousGroundedForceInputs[key].applyCooldown > 0)
            {
                cooldownAdjustableGroundedForceKeys.Add(key);
            }

        }
        foreach(var key in acceptedInstantaniousLevitatingForceInputs.Keys)
        {
            if (acceptedInstantaniousLevitatingForceInputs[key].applyCooldown > 0)
            {
                cooldownAdjustableLevitatingForceKeys.Add(key);
            }
        }
        //game properties
        animChecksSeconds = GameProperties.Singleton.AnimationCheckBuffer;
        StartCoroutine(AnimationCheckEnumeration(animChecksSeconds));
    }

    private IEnumerator AnimationCheckEnumeration(float wait)
    {
        while(humanoidAnimator != null)
        {
            humanoidAnimator.SetBool("isGrounded", CanExertMotorForce());

            yield return new WaitForSecondsRealtime(wait);
        }
    }

    private void Update()
    {
        foreach(var key in cooldownAdjustableGroundedForceKeys)
        {
            acceptedInstantaniousGroundedForceInputs[key].AdjustCurrentCooldown(- Time.deltaTime);
        }
        foreach (var key in cooldownAdjustableLevitatingForceKeys)
        {
            acceptedInstantaniousLevitatingForceInputs[key].AdjustCurrentCooldown(-Time.deltaTime);
        }
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
            humanoidAnimator.SetFloat("forwardSpeed", 0.5f);
        }
        else
        {
            motorMovementForceObject.UpdateCurrentForwardIndex(0);
            humanoidAnimator.SetFloat("forwardSpeed", 0);
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
            humanoidAnimator.SetFloat("forwardSpeed", 1f);
        }
        else
        {
            motorMovementForceObject.UpdateCurrentForwardIndex(0);
            humanoidAnimator.SetFloat("forwardSpeed", 0f);
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
        if (CanExertMotorForce())
        {
            if (acceptedInstantaniousGroundedForceInputs.ContainsKey(forceKeyCode))
            {
                HumanoidComponentExclusiveAdjustableForceInformationWithKnownObject info = acceptedInstantaniousGroundedForceInputs[forceKeyCode];

                if (info.GetCurrentCooldown() <= 0)
                {
                    info.SetCurrentCooldown(info.applyCooldown);
                    if(info.animationFlag != "")
                    {
                        humanoidAnimator.SetTrigger(info.animationFlag);
                    }
                    CustomTraditionalForce force = new CustomTraditionalForce(affectedForceObject.transform.localToWorldMatrix * info.v);
                    force.ApplyForce(affectedForceObject, info.isPure, info.infiniteTimeForce ? float.NegativeInfinity : info.applyTime);
                }

            }
        }else
        {
            if (acceptedInstantaniousLevitatingForceInputs.ContainsKey(forceKeyCode))
            {
                HumanoidComponentExclusiveAdjustableForceInformationWithKnownObject info = acceptedInstantaniousLevitatingForceInputs[forceKeyCode];

                if (info.GetCurrentCooldown() <= 0)
                {
                    info.SetCurrentCooldown(info.applyCooldown);
                    if (info.animationFlag != "")
                    {
                        humanoidAnimator.SetTrigger(info.animationFlag);
                    }
                    CustomTraditionalForce force = new CustomTraditionalForce(affectedForceObject.transform.localToWorldMatrix * info.v);
                    force.ApplyForce(affectedForceObject, info.isPure, info.infiniteTimeForce ? float.NegativeInfinity : info.applyTime);
                }

            }
        }

    }

    private bool CanExertMotorForce()
    {
        //return Vector3.Project(affectedForceObject.GetRecentNetAcceleration(), -motorMovementForceObject.GetGroundDir()).magnitude < levitationTolerance &&
        //     Vector3.Project(affectedForceObject.GetRecentNetSpeed(), -motorMovementForceObject.GetGroundDir()).magnitude < levitationTolerance;

        float sphereRad = levitationTolerance / 2;
        bool returned = Physics.CheckSphere(humanoidCollider.bounds.center + motorMovementForceObject.GetGroundDir() * (humanoidCollider.bounds.extents.y + sphereRad + 0.01f), sphereRad);

        return returned;
    }

    #region Getters

    public List<KeyCode> GetAcceptedInstantKeyCodes()
    {
        List<KeyCode> returned = new List<KeyCode>(acceptedInstantaniousGroundedForceInputs.Keys); //TODO a bit ineffiicent but oh well
        returned.AddRange(acceptedInstantaniousLevitatingForceInputs.Keys);
        return returned;
    }

    #endregion

    [Serializable]
    public class HumanoidComponentExclusiveAdjustableForceInformationWithKnownObject
    {
        public float applyCooldown;

        private float currentCooldown;

        public string animationFlag;

        public Vector3 v;

        public bool isPure;

        public bool infiniteTimeForce;

        public float applyTime;

        public void AdjustCurrentCooldown(float adjustment)
        {
            currentCooldown += adjustment;
        }

        public void SetCurrentCooldown(float coo)
        {
            currentCooldown = coo;
        }

        public float GetCurrentCooldown()
        {
            return currentCooldown;
        }

    }

}
