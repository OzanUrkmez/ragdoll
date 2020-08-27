using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectsInvisible : MonoBehaviour
{
    public GameObject gravity, slope;
    // Simple script to set objects invisible until buttons are pressed
    void Start()
    {
        gravity.SetActive(false);
        slope.SetActive(false);
    }

}
