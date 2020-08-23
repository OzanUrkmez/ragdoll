using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// This class handles in-game forces for rigidbodies, character controllers, and regular transforms as a whole. 
/// </summary>
public class ForceObject : MonoBehaviour
{

    [SerializeField]
    private float gravityMultiplier = 1f;
    [SerializeField]
    private float dragResistanceMultiplier = 1f; //TODO if can change this, then update adjusted true maximum speed etc. too
    [SerializeField]
    private float mass = 1f;

    [SerializeField]
    private float minimumSpeedToMove = 0.2f;

    private bool isRigid = false;
    private Rigidbody activeRigidBody;
    private ConstantForce activeConstantGravityAdjustmentForce;

    private float adjustedTrueMaximumSpeed;

    //if smaller than 0, then it is disregarded.
    //TODO add dampening effect for drag. make it more applied over time.
    //TODO if can change this, then do event system! 
    private float objectDragValue = -1; 

    //TODO In future move drag etc. to modular outside modifiable behaviour effecting system. have this system be called by events etc. in here to adjust speeds etc etc. so yeah modular behaviour etc. system! we can then truly have customizable physics

    #region Initialization

    private void Start()
    {
        activeRigidBody = GetComponent<Rigidbody>();
        if(activeRigidBody != null && !activeRigidBody.isKinematic) //kinematic rigidbodies do not interact with physics system so we will use this setting to enable and disable rigid body behaviour.
        {
            //GRAVITY

            //make sure to not do gravity ourselves but modify rigidbody gravity accordingly with a second force.
            isRigid = true;
            if (activeRigidBody.useGravity)
            {
                activeConstantGravityAdjustmentForce = gameObject.AddComponent<ConstantForce>();
                activeConstantGravityAdjustmentForce.force = (gravityMultiplier - 1) * GameProperties.Singleton.GravityConstant;
            }
        }
        else
        {
            //GRAVITY

            //not rigid body! we deal with gravity ourselves.
            characterController = GetComponent<CharacterController>();

            CustomForce gravityForce = new CustomForce(this, new CustomTraditionalForce(GameProperties.Singleton.GravityConstant * gravityMultiplier), true, float.NegativeInfinity);

          
            InitializeAppropriateForceCoroutine();
            

        }

        //DRAG

        objectDragValue = GameManager.Singleton.GetLevelDefaultDragValue();
        adjustedTrueMaximumSpeed = objectDragValue * dragResistanceMultiplier;


    }



    private void OnDestroy()
    {
        //ensure coroutines are stopped.
        StopAllCoroutines();
    }

    #endregion

    #region Force Enumeration

    private CharacterController characterController;

    [SerializeField]
    private Vector3 netSpeedForFrame = Vector3.zero;
    [SerializeField]
    private Vector3 netAccelerationForFrame = Vector3.zero;

    //private List<Vector3> appliedSpeed = new List<Vector3>(); //TODO implement pure speed.
    private List<CustomForce> appliedForces = new List<CustomForce>();
    private List<CustomForce> appliedConstantForces = new List<CustomForce>();

    private void InitializeAppropriateForceCoroutine()
    {
        if (isRigid)
            return; //TODO do not return my friend! we shall play even with rigid bodies! none shall escape the wrath of the custom physics system

        if (characterController == null)
        {
            StartCoroutine(TransformForceEnumeration());
        }
        else
        {
            StartCoroutine(ControllerForceEnumeration());
        }
    }

    //this is run for objects with character contorllers
    private IEnumerator ControllerForceEnumeration()
    {
        while(true) 
        {
            yield return new WaitForFixedUpdate(); //fixed update is used for physics calculations by convention. it makes things less buggy in low FPS and makes sure collisions etc. occur properly.
            CalculateProcesAcceleration(Time.fixedDeltaTime);
            if(netSpeedForFrame.magnitude > minimumSpeedToMove)
                characterController.Move(netSpeedForFrame * Time.fixedDeltaTime);
        }
    }

    //this is run for objects with regular tranforms.
    private IEnumerator TransformForceEnumeration()
    {
        while (true) 
        {
            yield return new WaitForFixedUpdate();
            CalculateProcesAcceleration(Time.fixedDeltaTime);
            if (netSpeedForFrame.magnitude > minimumSpeedToMove)
                transform.Translate(netSpeedForFrame * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Calculates acceleration and then applies it to speed variable for frame time. Also sets calculated global variables and subtracts times from forces.
    /// </summary> 
    private void CalculateProcesAcceleration(float frameTime)
    {
        netAccelerationForFrame = Vector3.zero;
        foreach(var force in appliedForces)
        {
            force.AppliedFor -= frameTime;
            if (force.AppliedFor < frameTime)
            {
                //only increase by amount left.
                netAccelerationForFrame += force.GetCurrentAppliedForce() * ((force.AppliedFor + frameTime) / frameTime);
            }
            else
            {
                netAccelerationForFrame += force.GetCurrentAppliedForce();
            }
        }
        foreach (var force in appliedConstantForces)
        {
            netAccelerationForFrame += force.GetCurrentAppliedForce();
        }

        netSpeedForFrame += netAccelerationForFrame * frameTime;
        if(netSpeedForFrame.magnitude > adjustedTrueMaximumSpeed)
        {
            netSpeedForFrame = netSpeedForFrame.normalized * adjustedTrueMaximumSpeed;
        }
    }

    #endregion

    #region Force Application and Removal

    public void ApplyNewForce(CustomForce f)
    {
        if (f.AppliedFor == float.NegativeInfinity)
        {
            appliedConstantForces.Add(f);
        }
        else
        {
            appliedForces.Add(f);
        }
        f.SetParentForceObject(this);

        onNewForceAdded?.Invoke(f);

    }

    public void RemoveForce(CustomForce f)
    {
        if(f.AppliedFor == float.NegativeInfinity)
        {
            if (appliedConstantForces.Remove(f))
            {
                f.RemoveParentForecObject(this);
            }
            else
            {
                Debug.LogError("ForceObject.RemoveForce() was called with a reference to a force that it does not own.");
            }
        }
        else
        {
            if(appliedForces.Remove(f))
            {
                f.RemoveParentForecObject(this);
            }
            else
            {
                Debug.LogError("ForceObject.RemoveForce() was called with a reference to a force that it does not own.");
            }
        }

        onForceRemoved?.Invoke(f);
    }

    #endregion

    #region Getters

    public Vector3 GetRecentNetSpeed()
    {
        return netSpeedForFrame;
    }

    public Vector3 GetRecentNetAcceleration()
    {
        return netAccelerationForFrame;
    }

    public float GetMass()
    {
        return mass;
    }

    public float GetAdjustedTrueMaximumSpeed()
    {
        return adjustedTrueMaximumSpeed;
    }

    public float GetObjectDragValue()
    {
        return objectDragValue;
    }

    #endregion

    #region Direct Adjustment

    public void DirectAdjustAddSpeed(Vector3 speed)
    {
        netSpeedForFrame += speed;
    }

    public void DirectSetSpeed(Vector3 speed)
    {
        netSpeedForFrame = speed;
    }

    #endregion

    #region EventSystem

    public Action<CustomForce> onNewForceAdded;

    public Action<CustomForce> onForceRemoved;

    #endregion

}
