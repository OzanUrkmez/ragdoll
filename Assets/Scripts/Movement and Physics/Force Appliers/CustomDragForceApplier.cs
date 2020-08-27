using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDragForceApplier : MonoBehaviour
{

    private void Start()
    {
        //DELETE THIS IN FUTURE? MAYBE CAN STAY
        if((GetComponent<Rigidbody>() == null && GetComponent<Collider>() == null) || GetComponents<CustomInstantNormalForceApplier>().Length != 1)
        {
            Destroy(this);
        }
    }


    [SerializeField]
    private float normalForceStableMultiplier = 1f;

    [SerializeField]
    private float normalForceInstantBounceMultiplier = 1f;

    private Dictionary<ForceObject, CustomOppositeAlongNormalForce> normalAppliedOn = new Dictionary<ForceObject, CustomOppositeAlongNormalForce>();

    private Dictionary<ForceObject, List<Transform>> allCollidingComponents = new Dictionary<ForceObject, List<Transform>>();

    #region Unity Collision Detectors




    private void OnCollisionEnter(Collision collision)
    {
        var forceTarget = collision.collider.transform.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;

        if (!allCollidingComponents.ContainsKey(forceTarget))
        {
            allCollidingComponents.Add(forceTarget, new List<Transform>());
            GeneralCollisionEnter(forceTarget, collision.GetContact(0).normal);
        }

        allCollidingComponents[forceTarget].Add(collision.transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        var forceTarget = collision.collider.transform.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;

        allCollidingComponents[forceTarget].Remove(collision.transform);
        if(allCollidingComponents[forceTarget].Count < 1)
        {
            allCollidingComponents.Remove(forceTarget);
            GeneralCollisionExit(forceTarget);
        }
    }

    private void OnDisable()
    {
        List<ForceObject> keys = new List<ForceObject>(normalAppliedOn.Keys);
        foreach (var force in keys)
        {
            GeneralCollisionExit(force);
        }

        allCollidingComponents.Clear();
    }

    #endregion

    private void GeneralCollisionEnter(ForceObject forceTarget, Vector3 contactFaceNormal)
    {
        Vector3 adjustment = (-Vector3.Project(forceTarget.GetRecentNetSpeed(), contactFaceNormal)) * normalForceInstantBounceMultiplier;

        forceTarget.DirectAdjustAddSpeed(adjustment);

        var normforce = new CustomOppositeAlongNormalForce(contactFaceNormal, normalForceStableMultiplier); //TODO this right now only supports one face. is only good for big platforms etc. anything further may need a deeper mesh-interacting physics though
        normforce.ApplyForce(forceTarget, true, float.NegativeInfinity);
        normalAppliedOn.Add(forceTarget, normforce);
    }

    private void GeneralCollisionExit(ForceObject forceTarget)
    {
        normalAppliedOn[forceTarget].CeaseForceApplication();
        normalAppliedOn.Remove(forceTarget);
    }

}
