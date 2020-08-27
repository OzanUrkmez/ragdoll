using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject platform;
    public int timedelay = 2;
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DisappearObject", 2 + timedelay, timedelay*2);
        InvokeRepeating("ReappearObject", 2, timedelay*2);
        
    }

    void Update() {
        if (platform.transform.position.z > 100) {
            platform.transform.position += platform.transform.right * speed * Time.deltaTime;
        } else if (platform.transform.position.z < 0) {
            platform.transform.position -= platform.transform.right * speed * Time.deltaTime;
        }
    }

    void DisappearObject()
    {   
        platform.transform.position += platform.transform.right * speed * Time.deltaTime;
        platform.SetActive(false); //TODO bruh you gotta send notification to force object ;_;. on collision exit ;) 
    }

    void ReappearObject()
    {
        platform.SetActive(true);
        platform.transform.position += -platform.transform.right * speed * Time.deltaTime;
    }
}

