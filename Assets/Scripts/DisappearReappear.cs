using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearReappear : MonoBehaviour
{
    public GameObject wall;
    public int timedelay = 5;

    void Start()
    {
        InvokeRepeating("DisappearObject",2 + timedelay,timedelay*2);
        InvokeRepeating("ReappearObject",2,timedelay*2);
    }
    void DisappearObject()
    {
        wall.SetActive(false);
    }

    void ReappearObject()
    {
        wall.SetActive(true);
    }
}
