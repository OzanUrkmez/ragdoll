using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomForceOutWithinDistance : MonoBehaviour, ICustomForceImplementation
{

    public ComponentExclusiveForceInformation componentOnlyObjectRef;

    private void Start()
    {
        new CustomForce(componentOnlyObjectRef.appliedTo, this, componentOnlyObjectRef.isPure, (componentOnlyObjectRef.infiniteTimeForce) ? float.NegativeInfinity : componentOnlyObjectRef.applyTime);
    }


    public Transform distantTransform;

    public float minDistance;

    public float magnitude;

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        Vector3 between = objectAppliedTo.transform.position - distantTransform.transform.position;

        return (between.magnitude > minDistance) ? Vector3.zero : between.normalized * magnitude;
    }
}
