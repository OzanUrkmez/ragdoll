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

    private Dictionary<ForceObject, CustomOppositeAlongNormalForce> normalAppliedOn = new Dictionary<ForceObject, CustomOppositeAlongNormalForce>();




    private void OnCollisionEnter(Collision collision)
    {
        var forceTarget = collision.collider.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;
        //TODO can improve upon this.

        Vector3 adjustment = (-Vector3.Project(forceTarget.GetRecentNetSpeed(), collision.GetContact(0).normal)) * normalForceMultiplier;

        forceTarget.DirectAdjustAddSpeed(adjustment);

        var normforce = new CustomOppositeAlongNormalForce(collision.GetContact(0).normal, normalForceMultiplier); //TODO this right now only supports one face. is only good for big platforms etc. anything further may need a deeper mesh-interacting physics though
        normforce.ApplyForce(forceTarget, true, float.NegativeInfinity);
        normalAppliedOn.Add(forceTarget, normforce);
    }

    private void OnCollisionExit(Collision collision)
    {
        var forceTarget = collision.collider.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;

        normalAppliedOn[forceTarget].CeaseForceApplication();
        normalAppliedOn.Remove(forceTarget);
    }

}
