using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Each custom force must be applied to one force object. create new instances if you want to do it 
/// </summary>
[Serializable]
public class CustomForce
{

    //Does not concern itself with mass. is pure acceleration.
    public bool IsPure { get; private set; }

    private Vector3 _forceVector = Vector3.zero;
    public Vector3 ForceVector
    {
        get
        {
            if (maximumSpeedAlongForceDirection > 0 && )
                return Vector3.zero;
            return _forceVector;
        }
        private set
        {
            _forceVector = value;
        }
    }

    public float AppliedFor { get; set; }

    public float ForceInstanceModifiedForMass { get; private set; }

    //inactive if < 0
    private float maximumSpeedAlongForceDirection = float.NegativeInfinity;

    private ForceObject parentForceObject;

    ///<summary> creates a traditional force. </summary>
    /// <param name="isPure"></param>
    /// <param name="forceVector"></param>
    /// <param name="appliedFor">set to negative infinity to be applied until stated otherwise.</param>
    public CustomForce(bool isPure, Vector3 forceVector, float appliedFor)
    {
        ForceInstanceModifiedForMass = 1;
        IsPure = isPure;
        ForceVector = forceVector;
        AppliedFor = appliedFor;
        maximumSpeedAlongForceDirection = float.NegativeInfinity;
    }

    /// <summary>
    /// creates a custom force that is only applied if the speed of the object along the force direction is smaller than a set value.
    /// </summary>
    /// <param name="isPure"></param>
    /// <param name="forceVector"></param>
    /// <param name="appliedFor">set to negative infinity to be applied until stated otherwise.</param>
    /// <param name="maximumForceAlongDir">must be higher than 0</param>
    public CustomForce(bool isPure, Vector3 forceVector, float appliedFor, float maximumForceAlongDir)
    {
        ForceInstanceModifiedForMass = 1;
        IsPure = isPure;
        ForceVector = forceVector;
        AppliedFor = appliedFor;
        maximumSpeedAlongForceDirection = maximumForceAlongDir;
    }

    /// <summary>
    /// copy constructor
    /// </summary>
    public CustomForce(CustomForce f)
    {
        IsPure = f.IsPure;
        ForceVector = f.ForceVector;
        AppliedFor = f.AppliedFor;
        ForceInstanceModifiedForMass = f.ForceInstanceModifiedForMass;
        maximumSpeedAlongForceDirection = f.maximumSpeedAlongForceDirection;
    }

    public void ModifyForceVectorByMass(float mass)
    {
        ForceVector *= ForceInstanceModifiedForMass;
        ForceVector /= mass;
        ForceInstanceModifiedForMass = mass;
    }

    #region Parent Object Management

    //THESE TWO FUNCTIONS WORK TO PREVENT MULTIPLE FORCE OBJECTS TRYING TO BE PARENTS AT ONCE BY HAVING SUCH BEHAVIOR LEAD TO ILLOGICAL PROGRAMMING
    public void SetParentForceObject(ForceObject obj)
    {
        if(parentForceObject != null)
        {
            throw new Exception("The Custom Force already has a parent object!");
        }
        parentForceObject = obj;
    }

    public void RemoveParentForecObject(ForceObject obj)
    {
        if (parentForceObject == obj)
            parentForceObject = null;
        else
            throw new Exception("The Custom Force object's parent was tried to be removed by reference from a different force object");
    }

    #endregion
}
