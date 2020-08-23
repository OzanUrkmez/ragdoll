using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInstantSimpleTriggerNormalForceApplier : MonoBehaviour
{

    private Collider ownCollider;

    private void Start()
    {
        ownCollider = GetComponent<Collider>();
    }


    [SerializeField]
    private float normalForceMultiplier = 1f;


    private void OnTriggerEnter(Collider other)
    {
        var forceTarget = other.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;
        //TODO can improve upon this.
        Vector3 approximateContactPoint = other.ClosestPointOnBounds(ownCollider.ClosestPointOnBounds(other.transform.position));
        RaycastHit[] hits;

        hits = Physics.RaycastAll(other.transform.position, approximateContactPoint - other.transform.position);

        bool hitSuccess = false;

        foreach(var hit in hits)
        {
            if (hit.transform == other.transform)
            {
                hitSuccess = true;

                Debug.DrawRay(hit.point, hit.normal, Color.blue, 2);

                Vector3 adjustment = (-Vector3.Project(forceTarget.GetRecentNetSpeed(), hit.normal)) * normalForceMultiplier;

                Debug.Log(adjustment);

                forceTarget.DirectAdjustAddSpeed(adjustment );
            }
        }

        if (!hitSuccess)
        {
            Debug.LogError("Wetefe are you not using convex or whats going on normal force nani.");
        }
    }

}
