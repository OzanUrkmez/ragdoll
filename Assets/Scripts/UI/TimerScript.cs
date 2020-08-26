using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public TextMesh text;
    public GameObject platform;
    public int Time = 5;
    public int Timedisplay = 5;


    void Update()
    {
        text.transform.LookAt(Camera.main.transform);
        text.transform.Rotate(0, 180, 0);
    }
    public void collided(Collision collision)
    {
        Debug.Log("Collision");
        Invoke("DisappearObject", Time);
        InvokeRepeating("TimeChanger", 1, 1);

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

    void TimeChanger()
    {
        if (Timedisplay == 0)
        {
            text.text = "5";
            CancelInvoke("TimeChanger");
        }
        else
        {
            Timedisplay -= 1;
            text.text = Timedisplay.ToString();
        }
    }
}


