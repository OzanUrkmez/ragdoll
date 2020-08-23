using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CustomLinearDragForce : MonoBehaviour, ICustomForceImplementation
{

    [SerializeField]
    private IndividualComponentExclusiveDirectForceApplicationInformation componentOnlyObjectRef;

    private void Start()
    {
        //TODO this is only run if the force is attached as its own component. I was originally going to expose interfaces in the editor directly but Unity was an asshole. 

        new CustomForce(componentOnlyObjectRef.appliedTo, this, componentOnlyObjectRef.isPure, (componentOnlyObjectRef.infiniteTimeForce) ? float.NegativeInfinity : componentOnlyObjectRef.applyTime);
    }


    [SerializeField]
    private float dragForceCoefficient = 0.05f;

    //TODO will caching getter values change performance?

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        return -objectAppliedTo.GetRecentNetSpeed().normalized * 
            Mathf.Lerp(0, objectAppliedTo.GetObjectDragValue(), objectAppliedTo.GetRecentNetSpeed().magnitude / objectAppliedTo.GetAdjustedTrueMaximumSpeed());
    }

}