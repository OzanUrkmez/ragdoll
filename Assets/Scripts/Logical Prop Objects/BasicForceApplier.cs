using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicForceApplier : MonoBehaviour
{

    [SerializeField, TypeConstraint(typeof(ICustomForceImplementation))]
    private Object force;

    [SerializeField]
    private ForceObject appliedTo;

    [SerializeField]
    private bool isPure;

    [SerializeField]
    private bool infiniteTimeForce;

    [SerializeField]
    private float applyTime;

    private CustomForce createdForce;

    private void Start()
    {
        createdForce = new CustomForce(appliedTo, force as ICustomForceImplementation, isPure, infiniteTimeForce? float.NegativeInfinity : applyTime);
    }

    public CustomForce GetCreatedForce()
    {
        return createdForce;
    }

}
