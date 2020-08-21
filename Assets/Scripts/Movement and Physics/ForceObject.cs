using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class handles in-game forces for rigidbodies, character controllers, and regular transforms as a whole. 
/// </summary>
public class ForceObject : MonoBehaviour
{

    [SerializeField]
    private float gravityMultiplier = 1f;

    private bool isRigid = false;
    private Rigidbody activeRigidBody;
    private ConstantForce activeConstantGravityAdjustmentForce;


    private void Start()
    {
        activeRigidBody = GetComponent<Rigidbody>();
        if(activeRigidBody != null)
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

            cachedGravityConstant = GameProperties.Singleton.GravityConstant * gravityMultiplier;

            if(cachedGravityConstant != Vector3.zero) //TODO if it gets out of 0 then we shall have to start coroutines ourselves. just call  InitializeAppropriateGravityCoroutine(). also in that case if the constant becomes 0 then we shall have to stop gravity execution.
            {
                InitializeAppropriateGravityCoroutine();
            }

        }
    }

    private CharacterController characterController;


    private void OnDestroy()
    {
        //ensure coroutines are stopped.
        StopAllCoroutines();
    }

    #region Gravity

    private void InitializeAppropriateGravityCoroutine()
    {
        if (isRigid)
            return;

        if (characterController == null)
        {
            StartCoroutine(TransformGravityEnumeration());
        }
        else
        {
            StartCoroutine(ControllerGravityEnumeration());
        }
    }

    private Vector3 cachedGravityConstant;

    //this is run for objects with character contorllers
    private IEnumerator ControllerGravityEnumeration()
    {
        while(true) 
        {
            yield return new WaitForFixedUpdate(); //fixed update is used for physics calculations by convention. it makes things less buggy in low FPS and makes sure collisions etc. occur properly.
            characterController.Move(cachedGravityConstant * Time.fixedDeltaTime);
        }
    }

    //this is run for objects with regular tranforms.
    private IEnumerator TransformGravityEnumeration()
    {
        while (true) 
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(cachedGravityConstant * Time.fixedDeltaTime);
        }
    }

    #endregion

}
