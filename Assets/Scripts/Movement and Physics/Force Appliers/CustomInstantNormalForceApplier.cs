using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInstantNormalForceApplier : MonoBehaviour
{

    private void Start()
    {
    }


    [SerializeField]
    private float normalForceMultiplier = 1f;


    private void OnTriggerEnter(Collider other)
    {
        var forceTarget = other.GetComponent<ForceObject>();
        if (forceTarget == null)
            return;
            
    }

}
