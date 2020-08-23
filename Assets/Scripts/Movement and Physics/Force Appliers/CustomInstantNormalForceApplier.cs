using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInstantNormalForceApplier : MonoBehaviour
{
    private Collider ownCollider;

    private void Start()
    {
        ownCollider = GetComponent<Collider>();
    }


    [SerializeField]
    private float normalForceMultiplier = 1f;


    private void OnCollisionEnter(Collision collision)
    {
        var forceTarget = collision.collider.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;
        //TODO can improve upon this.

        Vector3 adjustment = (-Vector3.Project(forceTarget.GetRecentNetSpeed(), collision.GetContact(0).normal)) * normalForceMultiplier;

        Debug.Log(adjustment);

        forceTarget.DirectAdjustAddSpeed(adjustment);
    }
}
