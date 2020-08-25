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

    //TODO PROPER GARBAGE COLLECTION ETC. AROUND EVENT SYSTEM

    //Does not concern itself with mass. is pure acceleration.
    public bool IsPure { get; private set; }

    //negative infinity implies constant force until removed by outside code.
    public float AppliedFor { get; set; }

    public bool IsLastForce { get; private set; }

    private ICustomForceImplementation customForceImplementation;

    private ForceObject parentForceObject;

    /// <summary>
    /// Creates a custom force and applies it to the appliedTo force object.
    /// </summary>
    /// <param name="appliedTo"> the object the force is applied to. </param>
    /// <param name="forceApplierImplementation"> an interface through which logic for a particular custom force logic can communicate the force for the current frame. See implementations. </param>
    /// <param name="isPure"> if it is pure, then the force will not be affected by mass. </param>
    /// <param name="appliedFor"> the time for which the force is applied. if set to float.NegativeInfinity, then the force will be applied until outside code stops it. </param>
    public CustomForce(ForceObject appliedTo, ICustomForceImplementation forceApplierImplementation, bool isPure, float appliedFor, bool isLastForce = false)
    {
        IsPure = isPure;
        AppliedFor = appliedFor;
        customForceImplementation = forceApplierImplementation;
        IsLastForce = isLastForce;
        appliedTo.ApplyNewForce(this);
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

    public void RemoveParentForceObject(ForceObject obj)
    {
        if (parentForceObject == obj)
            parentForceObject = null;
        else
            throw new Exception("The Custom Force object's parent was tried to be removed by reference from a different force object");
    }

    #endregion

    public Vector3 GetCurrentAppliedForce()
    {
        return customForceImplementation.GetCurrentForceVector(this, parentForceObject) * (IsPure? 1 : 1/parentForceObject.GetMass());
    }

}


//BEFORE INHERITANCE SYSTEM:

/////<summary> creates a traditional force. </summary>
///// <param name="isPure"></param>
///// <param name="forceVector"></param>
///// <param name="appliedFor">set to negative infinity to be applied until stated otherwise.</param>
//public CustomForce(bool isPure, Vector3 forceVector, float appliedFor)
//{
//    ForceInstanceModifiedForMass = 1;
//    IsPure = isPure;
//    ForceVector = forceVector;
//    AppliedFor = appliedFor;
//    maximumSpeedAlongForceDirection = float.NegativeInfinity;
//}

///// <summary>
///// creates a custom force that is only applied if the speed of the object along the force direction is smaller than a set value.
///// </summary>
///// <param name="isPure"></param>
///// <param name="forceVector"></param>
///// <param name="appliedFor">set to negative infinity to be applied until stated otherwise.</param>
///// <param name="maximumForceAlongDir">must be higher than 0</param>
//public CustomForce(bool isPure, Vector3 forceVector, float appliedFor, float maximumForceAlongDir)
//{
//    ForceInstanceModifiedForMass = 1;
//    IsPure = isPure;
//    ForceVector = forceVector;
//    AppliedFor = appliedFor;
//    maximumSpeedAlongForceDirection = maximumForceAlongDir;
//}

///// <summary>
///// copy constructor
///// </summary>
//public CustomForce(CustomForce f)
//{
//    IsPure = f.IsPure;
//    ForceVector = f.ForceVector;
//    AppliedFor = f.AppliedFor;
//    ForceInstanceModifiedForMass = f.ForceInstanceModifiedForMass;
//    maximumSpeedAlongForceDirection = f.maximumSpeedAlongForceDirection;
//}