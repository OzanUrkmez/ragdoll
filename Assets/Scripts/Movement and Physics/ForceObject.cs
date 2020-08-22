using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// This class handles in-game forces for rigidbodies, character controllers, and regular transforms as a whole. 
/// </summary>
public class ForceObject : MonoBehaviour
{

    [SerializeField]
    private float gravityMultiplier = 1f;
    [SerializeField]
    private float dragResistanceMultiplier = 1f;
    [SerializeField]
    private float mass = 1f;

    private bool isRigid = false;
    private Rigidbody activeRigidBody;
    private ConstantForce activeConstantGravityAdjustmentForce;

    //if smaller than 0, then it is disregarded.
    //TODO add dampening effect for drag. make it more applied over time.
    private float objectDragValue = -1;

    #region Initialization

    private void Start()
    {
        activeRigidBody = GetComponent<Rigidbody>();
        if(activeRigidBody != null || !activeRigidBody.isKinematic) //kinematic rigidbodies do not interact with physics system so we will use this setting to enable and disable rigid body behaviour.
        {
            //GRAVITY

            //make sure to not do gravity ourselves but modify rigidbody gravity accordingly with a second force.
            isRigid = true;
            if (activeRigidBody.useGravity)
            {
                activeConstantGravityAdjustmentForce = gameObject.AddComponent<ConstantForce>();
                activeConstantGravityAdjustmentForce.force = (gravityMultiplier - 1) * GameProperties.Singleton.GravityConstant;
            }
        }
        else
        {
            //GRAVITY

            //not rigid body! we deal with gravity ourselves.
            characterController = GetComponent<CharacterController>();

            CustomForce gravityForce = new CustomForce(true, GameProperties.Singleton.GravityConstant * gravityMultiplier, float.NegativeInfinity);
            appliedConstantForces.Add(gravityForce);

            if(gravityForce.ForceVector != Vector3.zero) //TODO if it gets out of 0 then we shall have to start coroutines ourselves. just call  InitializeAppropriateGravityCoroutine(). also in that case if the constant becomes 0 then we shall have to stop gravity execution.
            {
                InitializeAppropriateForceCoroutine();
            }

        }

        //DRAG

        objectDragValue = GameManager.Singleton.GetLevelDefaultDragValue();

    }



    private void OnDestroy()
    {
        //ensure coroutines are stopped.
        StopAllCoroutines();
    }

    #endregion

    #region Force Enumeration

    private CharacterController characterController;

    [SerializeField]
    private Vector3 netSpeedForFrame = Vector3.zero;
    [SerializeField]
    private Vector3 netAcceleration = Vector3.zero;

    //private List<Vector3> appliedSpeed = new List<Vector3>(); //TODO implement pure speed.
    private List<CustomForce> appliedForces = new List<CustomForce>();
    private List<CustomForce> appliedConstantForces = new List<CustomForce>();

    private void InitializeAppropriateForceCoroutine()
    {
        if (isRigid)
            return;

        if (characterController == null)
        {
            StartCoroutine(TransformForceEnumeration());
        }
        else
        {
            StartCoroutine(ControllerForceEnumeration());
        }
    }

    //this is run for objects with character contorllers
    private IEnumerator ControllerForceEnumeration()
    {
        while(true) 
        {
            yield return new WaitForFixedUpdate(); //fixed update is used for physics calculations by convention. it makes things less buggy in low FPS and makes sure collisions etc. occur properly.
            CalculateProcesAcceleration(Time.fixedDeltaTime);
            characterController.Move(netSpeedForFrame * Time.fixedDeltaTime);
        }
    }

    //this is run for objects with regular tranforms.
    private IEnumerator TransformForceEnumeration()
    {
        while (true) 
        {
            yield return new WaitForFixedUpdate();
            CalculateProcesAcceleration(Time.fixedDeltaTime);
            transform.Translate(netSpeedForFrame * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Calculates acceleration and then applies it to speed variable for frame time. Also sets calculated global variables and subtracts times from forces.
    /// </summary> 
    private void CalculateProcesAcceleration(float frameTime)
    {
        netAcceleration = Vector3.zero;
        foreach(var force in appliedForces)
        {
            force.AppliedFor -= frameTime;
            if (force.AppliedFor < frameTime)
            {
                //only increase by amount left.
                netAcceleration += force.ForceVector * ((force.AppliedFor + frameTime) / frameTime);
            }
            else
            {
                netAcceleration += force.ForceVector;
            }
        }
        foreach (var force in appliedConstantForces)
        {
            netAcceleration += force.ForceVector;
        }

        netSpeedForFrame += netAcceleration * frameTime;
        if(netSpeedForFrame.magnitude * dragResistanceMultiplier > objectDragValue)
        {
            netSpeedForFrame = netSpeedForFrame.normalized * objectDragValue;
        }
    }

    #endregion

    #region Force Application and Removal

    public void ApplyNewForce(CustomForce f)
    {
        //Implying unchanging mass. if mass does change then adjust impure accordingly!
        if (!f.IsPure)
        {
            f.ModifyForceVectorByMass(mass);
        }
        if (f.AppliedFor == float.NegativeInfinity)
        {
            appliedConstantForces.Add(f);
        }
        else
        {
            appliedForces.Add(f);
        }
        f.SetParentForceObject(this);
    }

    public void RemoveForce(CustomForce f)
    {
        if(f.AppliedFor == float.NegativeInfinity)
        {
            if (appliedConstantForces.Remove(f))
            {
                f.RemoveParentForecObject(this);
            }
            else
            {
                Debug.LogError("ForceObject.RemoveForce() was called with a reference to a force that it does not own.");
            }
        }
        else
        {
            if(appliedForces.Remove(f))
            {
                f.RemoveParentForecObject(this);
            }
            else
            {
                Debug.LogError("ForceObject.RemoveForce() was called with a reference to a force that it does not own.");
            }
        }
    }

    #endregion

    #region Getters

    public 

    #endregion

}
