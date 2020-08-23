using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CustomLinearDragForce : ICustomForceImplementation
{

    [SerializeField]
    private float dragForceCoefficient = 0.05f;

    //TODO will caching getter values change performance?

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        return -objectAppliedTo.GetRecentNetSpeed().normalized * 
            Mathf.Lerp(0, objectAppliedTo.GetObjectDragValue(), objectAppliedTo.GetRecentNetSpeed().magnitude / objectAppliedTo.GetAdjustedTrueMaximumSpeed());
    }

}