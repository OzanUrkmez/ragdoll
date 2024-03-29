﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float mouseSensitivity = 100f;

    //[SerializeField]
    //private float playerWalkBaseSpeed = 5f;
    //[SerializeField]
    //private float playerBaseRunSpeed = 8f;
    //[SerializeField]
    //private float playerBackwardsSpeed = -3f;

    //[SerializeField]
    //private float batSlowdown = 2f; //TODO IMPLEMENT THISS


    [SerializeField]
    private GameObject batObject;
    
    [SerializeField]
    private GameObject gunObject;

    [SerializeField]
    private GameObject knifeObject;

    [SerializeField]
    private GameObject characterObject;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private HumanoidMotorObject humanoidMotorObject;

    float xRotation = 0;

    [SerializeField]
    float animationDecreaseConstant = 2;
    [SerializeField]
    float animationIncreaseConstant = 1;
    [SerializeField]
    float animationCutOffConstant = 0.5f;

    private List<KeyCode> possibleInstantInputs = new List<KeyCode>();

	void Start () {
        //hide cursor and lock it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameManager.Singleton.AssignPlayer(characterObject.gameObject);

        WeaponChooser.onNewWeaponChosen += OnWeaponChanged;

        possibleInstantInputs = humanoidMotorObject.GetAcceptedInstantKeyCodes();
    }

    //TODO We do this in Update for now. If doing professionally, it is best practice to put controls in a separate controls.
    //interface/class so that keybinds can easily be modified and I/O is easily tracked. 
    //Unity also has a new system for doing this.
	void Update () {
    
        //MOUSE CAMERA MOVEMENT

        //get mouse delta
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md *= mouseSensitivity * Time.deltaTime;

        //clamp such that can only look up and down and not behind.
        xRotation -= md.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //camera looks up and down, but player/parent is static. player only turns left and right.

        if (!humanoidMotorObject.IsOrienting())
        {
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            characterObject.transform.Rotate(Vector3.up * md.x, Space.Self);

        }

        //KEYBOARD POSITION MOVEMENT

        //get keyboard WASD movement. Vertical is WS and Horizontal is AD keys.
        Vector2 moveDelta = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //change animation depending on player input

        float animChangeCoeff = (moveDelta.magnitude - animationCutOffConstant);

        playerAnimator.SetFloat("Speed", Mathf.Clamp(playerAnimator.GetFloat("Speed") + ((animChangeCoeff > 0)? animChangeCoeff * animationIncreaseConstant * Time.fixedDeltaTime:
            animChangeCoeff * animationDecreaseConstant * Time.fixedDeltaTime)
            , 0,1));

        if (Input.GetKey(KeyCode.LeftShift))
            humanoidMotorObject.OctagonalRunUpdate(moveDelta);
        else
            humanoidMotorObject.OctagonalWalkUpdate(moveDelta);

        //OTHER INPUT
        foreach(var key in possibleInstantInputs)
        {
            if (Input.GetKeyDown(key))
            {
                humanoidMotorObject.ExecuteInstantaniousForce(key);
            }
        }
        
    }

    private void OnWeaponChanged(WeaponType ChosenWeapon)
    {
        switch (ChosenWeapon)
        {
            case WeaponType.Bat:
                batObject.SetActive(true);
                gunObject.SetActive(false);
                knifeObject.SetActive(false);
                break;
            case WeaponType.Gun:
                gunObject.SetActive(true);
                batObject.SetActive(false);
                knifeObject.SetActive(false);
                break;
            case WeaponType.Knife:
                gunObject.SetActive(false);
                batObject.SetActive(false);
                knifeObject.SetActive(true);
                break;
            default:
                batObject.SetActive(false); //TODO fix
                gunObject.SetActive(false);
                knifeObject.SetActive(false);

                break;
        }
    }
}

