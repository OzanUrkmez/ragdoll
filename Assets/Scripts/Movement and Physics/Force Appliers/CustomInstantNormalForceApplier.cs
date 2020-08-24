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


    #region Unity Collision Detectors

    private void OnCollisionEnter(Collision collision)
    {
        var forceTarget = collision.collider.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;

        GeneralCollisionEnter(forceTarget, collision.GetContact(0).normal);
    }

    private void OnCollisionExit(Collision collision)
    {
        var forceTarget = collision.collider.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;
        GeneralCollisionExit(forceTarget);
    }

    #endregion

    private void GeneralCollisionEnter(ForceObject forceTarget, Vector3 contactFaceNormal)
    {
        Vector3 adjustment = (-Vector3.Project(forceTarget.GetRecentNetSpeed(), contactFaceNormal)) * normalForceMultiplier;

        forceTarget.DirectAdjustAddSpeed(adjustment);

        var normforce = new CustomOppositeAlongNormalForce(contactFaceNormal, normalForceMultiplier); //TODO this right now only supports one face. is only good for big platforms etc. anything further may need a deeper mesh-interacting physics though
        normforce.ApplyForce(forceTarget, true, float.NegativeInfinity);
        normalAppliedOn.Add(forceTarget, normforce);
    }

    private void GeneralCollisionExit(ForceObject forceTarget)
    {
        normalAppliedOn[forceTarget].CeaseForceApplication();
        normalAppliedOn.Remove(forceTarget);
    }

}
