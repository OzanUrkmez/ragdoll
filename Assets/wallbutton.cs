using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallbutton : MonoBehaviour
{
    public GameObject buttonwall;
    void OnCollisionEnter(Collision collision)
    {
        buttonwall.SetActive(false);
    }

    void OnCollisionExit(Collision collision)
    {
        buttonwall.SetActive(true);
    }
}
