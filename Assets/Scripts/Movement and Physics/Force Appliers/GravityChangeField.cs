using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChangeField : MonoBehaviour
{

    [SerializeField]
    private bool disableOnExit;

    [SerializeField]
    private Vector3 newGravity;

    private void OnTriggerEnter(Collider other)
    {
        ForceObject forceObject = other.GetComponent<ForceObject>();
        if (forceObject == null)
            return;

        forceObject.SetGravityBaseForce(newGravity);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!disableOnExit)
            return;

        ForceObject forceObject = other.GetComponent<ForceObject>();
        if (forceObject == null)
            return;

        forceObject.SetGravityBaseForce(GameProperties.Singleton.BaseGravity);

    }

}
