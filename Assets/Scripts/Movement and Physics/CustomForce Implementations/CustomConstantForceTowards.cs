using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomConstantForceTowards : MonoBehaviour, ICustomForceImplementation
{

    public ComponentExclusiveForceInformation componentOnlyObjectRef;

    private void Start()
    {
        new CustomForce(componentOnlyObjectRef.appliedTo, this, componentOnlyObjectRef.isPure, (componentOnlyObjectRef.infiniteTimeForce) ? float.NegativeInfinity : componentOnlyObjectRef.applyTime);
    }


    public Transform towardsTransform;

    public float magnitude;

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        return (towardsTransform.transform.position - objectAppliedTo.transform.position).normalized * magnitude;
    }
}
