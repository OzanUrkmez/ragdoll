using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInsantVelocitySetter : MonoBehaviour
{

    private void Start()
    {
        //DELETE THIS IN FUTURE? MAYBE CAN STAY
        if((GetComponent<Rigidbody>() == null && GetComponent<Collider>() == null) || GetComponents<CustomInsantVelocitySetter>().Length != 1)
        {
            Destroy(this);
        }
    }


    [SerializeField]
    private Vector3 v;

    #region Unity Collision Detectors


    private void OnCollisionEnter(Collision collision)
    {
        var forceTarget = collision.collider.transform.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;

        forceTarget.DirectSetSpeed(v);

    }


    #endregion

}
