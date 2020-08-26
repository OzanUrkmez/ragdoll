using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colidenotification : MonoBehaviour
{
    private TimerScript ts;
    public GameObject TimerBoi;

    void Start()
    {
        ts = TimerBoi.GetComponent<TimerScript>();
    }
    void OnCollisionEnter(Collision collision)
    {
        ts.collided(collision);
    }


    void Update()
    {
        
    }
}
