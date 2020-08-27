using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectsInvisible : MonoBehaviour
{
    public GameObject obj;
    // Simple script to set objects invisible until buttons are pressed
    void Start()
    {
        obj.SetActive(false); //Hello! Aurik has already implemented this. You can use his script -Ozan
    }

}
