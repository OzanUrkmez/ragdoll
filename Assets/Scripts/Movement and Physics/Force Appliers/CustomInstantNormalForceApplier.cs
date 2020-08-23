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

    private List<ForceObject> normalAppliedOn = new List<ForceObject>();


    private void OnCollisionEnter(Collision collision)
    {
        var forceTarget = collision.collider.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;
        //TODO can improve upon this.

        Vector3 adjustment = (-Vector3.Project(forceTarget.GetRecentNetSpeed(), collision.GetContact(0).normal)) * normalForceMultiplier;

        Debug.Log(adjustment);

        forceTarget.DirectAdjustAddSpeed(adjustment);

        forceTarget.onNewForceAdded += OnForceAdded;
        forceTarget.onForceRemoved += OnForceRemoved;

        normalAppliedOn.Add(forceTarget);
    }

    private void OnCollisionExit(Collision collision)
    {
        var forceTarget = collision.collider.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;

        forceTarget.onNewForceAdded -= OnForceAdded;
        forceTarget.onForceRemoved -= OnForceRemoved;
        normalAppliedOn.Remove(forceTarget);
    }

    private void FixedUpdate()
    {
        //assuming force is run every fixed update.
    }

    private void OnForceAdded(CustomForce f)
    {

    }

    private void OnForceRemoved(CustomForce f)
    {

    }
}
