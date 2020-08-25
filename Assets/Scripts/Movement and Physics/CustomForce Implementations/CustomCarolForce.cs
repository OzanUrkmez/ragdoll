using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCarolForce : MonoBehaviour, ICustomForceImplementation
{

    private void Start()
    {
        new CustomForce(gameObject.GetComponent<ForceObject>(), this, true, float.NegativeInfinity);
    }

    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        return Vector3.up * 12;
    }


}
