using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSwing : MonoBehaviour
{ 
    void Update() {
        // causes cubes to casually rotate around over time
        transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
    }
    // float rotationSpeed = 45;
    // Vector3 currentEulerAngles;
    // float x;
    // float y;
    // float z;

    // // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.X)) x = 1 - x;
    //     if (Input.GetKeyDown(KeyCode.Y)) y = 1 - y;
    //     if (Input.GetKeyDown(KeyCode.Z)) z = 1 - z;

    //     //modifying the Vector3, based on input multiplied by speed and time
    //     currentEulerAngles += new Vector3(x, y, z) * Time.deltaTime * rotationSpeed;

    //     //apply the change to the gameObject
    //     transform.localEulerAngles = currentEulerAngles;

    // }
}
