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
    private CustomTraditionalForce activeGravityForce;

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
        if (activeRigidBody != null && !activeRigidBody.isKinematic)
            isRigid = true;

        characterController = GetComponent<CharacterController>();

        activeGravityForce = new CustomTraditionalForce(GameProperties.Singleton.BaseGravity * gravityMultiplier);

        new CustomForce(this, activeGravityForce, true, float.NegativeInfinity);

        objectDragValue = GameManager.Singleton.GetLevelDefaultDragValue();
        adjustedTrueMaximumSpeed = objectDragValue * dragResistanceMultiplier;

        InitializeAppropriateForceCoroutine();


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

    private List<CustomForce> appliedForcesLast = new List<CustomForce>(); //TODO can in fact have more than one. must make sure that user knows that. Also maybe come up with better system about priority of being last etc among last forces.
    private List<CustomForce> appliedConstantForcesLast = new List<CustomForce>();

    private void InitializeAppropriateForceCoroutine()
    {
        if (isRigid)
        {
            StartCoroutine(RigidForceEnumeration());
            return;
        }

        if (characterController == null)
        {
            StartCoroutine(TransformForceEnumeration());
        }
        else
        {
            StartCoroutine(ControllerForceEnumeration());
        }
    }

    private IEnumerator RigidForceEnumeration()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate(); //fixed update is used for physics calculations by convention. it makes things less buggy in low FPS and makes sure collisions etc. occur properly.
            onBeforeSpeedApplied?.Invoke(this);

            CalculateProcesAcceleration(Time.fixedDeltaTime);
            if (netSpeedForFrame.magnitude > minimumSpeedToMove)
                activeRigidBody.velocity = netSpeedForFrame;
            else
                activeRigidBody.velocity = Vector3.zero;

            onAfterSpeedApplied?.Invoke(this);
        }
    }

    //this is run for objects with character contorllers
    private IEnumerator ControllerForceEnumeration()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate(); //fixed update is used for physics calculations by convention. it makes things less buggy in low FPS and makes sure collisions etc. occur properly.
            onBeforeSpeedApplied?.Invoke(this);

            CalculateProcesAcceleration(Time.fixedDeltaTime);
            if (netSpeedForFrame.magnitude > minimumSpeedToMove)
                characterController.Move(netSpeedForFrame * Time.fixedDeltaTime);

            onAfterSpeedApplied?.Invoke(this);
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
                transform.Translate(netSpeedForFrame * Time.fixedDeltaTime, Space.World);
        }
    }

    private List<CustomForce> appliedForcesToBeRemoved = new List<CustomForce>();
    private List<CustomForce> appliedLastForcesToBeRemoved = new List<CustomForce>();
    private List<CustomForce> queuedRemovals = new List<CustomForce>();

    /// <summary>
    /// Calculates acceleration and then applies it to speed variable for frame time. Also sets calculated global variables and subtracts times from forces.
    /// </summary> 
    private void CalculateProcesAcceleration(float frameTime)
    {

        //TODO does using foreach lead to lower performance?

        appliedForcesToBeRemoved.Clear();
        appliedLastForcesToBeRemoved.Clear();
        netAccelerationForFrame = Vector3.zero;
        foreach (var force in appliedForces)
        {
            force.AppliedFor -= frameTime;
            if (force.AppliedFor < frameTime)
            {
                //only increase by amount left.
                netAccelerationForFrame += force.GetCurrentAppliedForce() * ((force.AppliedFor + frameTime) / frameTime);
                appliedForcesToBeRemoved.Add(force);
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

        //now for last forces. again some priority system might be better. Last forces can depend on current acceleration etc.
        foreach (var force in appliedForcesLast)
        {
            force.AppliedFor -= frameTime;
            if (force.AppliedFor < frameTime)
            {
                //only increase by amount left.
                netAccelerationForFrame += force.GetCurrentAppliedForce() * ((force.AppliedFor + frameTime) / frameTime);
                appliedLastForcesToBeRemoved.Add(force);
            }
            else
            {
                netAccelerationForFrame += force.GetCurrentAppliedForce();
            }
        }

        foreach (var force in appliedConstantForcesLast)
        {
            netAccelerationForFrame += force.GetCurrentAppliedForce();
        }

        netSpeedForFrame += netAccelerationForFrame * frameTime;
        if (netSpeedForFrame.magnitude > adjustedTrueMaximumSpeed)
        {
            netSpeedForFrame = netSpeedForFrame.normalized * adjustedTrueMaximumSpeed;
        }

        //remove expired forces
        foreach(var force in appliedForcesToBeRemoved)
        {
            appliedForces.Remove(force);
        }

        foreach(var force in appliedLastForcesToBeRemoved)
        {
            appliedForcesLast.Remove(force);
        }

        //remove queued removals.
        foreach(var force in queuedRemovals)
        {
            RemoveForce(force);
        }
    }

    #endregion

    #region Force Application and Removal

    public void ApplyNewForce(CustomForce f)
    {
        if (f.AppliedFor == float.NegativeInfinity)
        {
            if (f.IsLastForce)
                appliedConstantForcesLast.Add(f);
            else
                appliedConstantForces.Add(f);
        }
        else
        {
            if (f.IsLastForce)
                appliedForcesLast.Add(f);
            else
                appliedForces.Add(f);
        }
        f.SetParentForceObject(this);

        onNewForceAdded?.Invoke(this, f);

    }

    public void RemoveForce(CustomForce f)
    {
        if (f.AppliedFor == float.NegativeInfinity)
        {
            if (appliedConstantForces.Remove(f) || appliedConstantForcesLast.Remove(f))
            {
                f.RemoveParentForceObject(this);
            }
            else
            {
                Debug.LogError("ForceObject.RemoveForce() was called with a reference to a force that it does not own.");
            }
        }
        else
        {
            if (appliedForces.Remove(f) || appliedForcesLast.Remove(f))
            {
                f.RemoveParentForceObject(this);
            }
            else
            {
                Debug.LogError("ForceObject.RemoveForce() was called with a reference to a force that it does not own.");
            }
        }

        onForceRemoved?.Invoke(this, f);
    }

    /// <summary>
    /// remove force after current physics call is complete.
    /// </summary>
    public void QueueRemoveForce(CustomForce f)
    {
        queuedRemovals.Add(f);
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

    #region Gravity

    public float GetGravityMultiplier()
    {
        return gravityMultiplier;
    }

    public Vector3 GetGravityForce()
    {
        return activeGravityForce.Force;
    }

    public void SetGravityBaseForce(Vector3 newForce)
    {
        newForce *= gravityMultiplier;

        activeGravityForce.Force = newForce;

        onGravityAdjusted?.Invoke(this, newForce);
    }

    #endregion

    #region EventSystem

    public Action<ForceObject> onBeforeSpeedApplied;

    public Action<ForceObject> onAfterSpeedApplied;

    public Action<ForceObject, CustomForce> onNewForceAdded;

    public Action<ForceObject, CustomForce> onForceRemoved;

    public Action<ForceObject, Vector3> onGravityAdjusted;

    #endregion

}
