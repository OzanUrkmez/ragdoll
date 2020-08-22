using UnityEngine;
using System;

[Serializable]
public class CustomTraditionalForce: ICustomForceImplementation
{

    [SerializeField]
    private Vector3 constantForce;

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        return constantForce;
    }

    public CustomTraditionalForce(Vector3 _constantForce)
    {
        constantForce = _constantForce;
    }

}

