using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public Transform textTransform;
    public GameObject platform;
    public int Time = 5;
    public int Timedisplay = 5;


    void Update()
    {
        textTransform.LookAt(Camera.main.transform);
        textTransform.Rotate(0, 180, 0);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Invoke("DisappearObject", Time);
        }

    }

    void DisappearObject()
    {
        platform.SetActive(false);
        Invoke("ReappearObject",Time);

    }

    void ReappearObject()
    {
        platform.SetActive(true);
        Timedisplay = Time;
    }
}


