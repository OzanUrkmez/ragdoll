using UnityEngine;
using System;

[Serializable]
public class CustomTraditionalForce : MonoBehaviour, ICustomForceImplementation
{

    [SerializeField]
    private ComponentExclusiveForceInformation editorSetApply;


    private void Start()
    {
        if (parentObject != null)
            return;
        Force = _force;
        parentForce = new CustomForce(editorSetApply.appliedTo, this, editorSetApply.isPure, editorSetApply.infiniteTimeForce ? float.NegativeInfinity : editorSetApply.applyTime);
        parentObject = editorSetApply.appliedTo;
    }


    [SerializeField]
    private Vector3 _force;

    public Vector3 Force
    {
        get
        {
            return _force;
        }
        set
        {
            _force = value;
        }
    }

    public CustomForce parentForce { get; private set; }
    public ForceObject parentObject { get; private set; }
    public Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo)
    {
        return Force;
    }

    public CustomTraditionalForce(Vector3 f)
    {
        Force = f;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appliedTo"></param>
    /// <param name="isPure"></param>
    /// <param name="appliedFor"> set to negative infinity to make constant force.</param>
    public void ApplyForce(ForceObject appliedTo, bool isPure, float appliedFor)
    {
        parentForce = new CustomForce(appliedTo, this, isPure, appliedFor);
        parentObject = appliedTo;
    }

}

