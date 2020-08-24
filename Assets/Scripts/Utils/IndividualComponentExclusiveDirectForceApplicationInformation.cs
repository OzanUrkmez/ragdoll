using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ComponentExclusiveForceInformation 
{

    public ForceObject appliedTo;

    public bool isPure;

    public bool infiniteTimeForce;

    public float applyTime;

}

