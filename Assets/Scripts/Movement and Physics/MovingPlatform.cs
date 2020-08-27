using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3[] points;
    public int pointNum = 0;
    private Vector3 currentTarget;
    public float tolerance;
    public int timeDelay = 2;
    public float speed;
    private float delayStart;
    public bool automatic;
    public GameObject platform;
    // Start is called before the first frame update
    void Start()
    {
        platform.SetActive(false);
        if (points.Length > 0) {
            currentTarget = points[0];
        }

        tolerance = speed * Time.deltaTime;
        
    }

    void Update() {
        if (transform.position != currentTarget) {
            MovePlatform();
        } 
        else {
            UpdateTarget();
        }

    }

    void MovePlatform()
    {   
        // moves the platform when user is on it
        Vector3 heading = currentTarget - transform.position;
        // normalizes heading vector into a unit vector and then multiplies it by speed
        transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
        if (heading.magnitude < tolerance) {
            // snaps platform into place as it reaches end
            transform.position = currentTarget;
            // starts timer
            delayStart = Time.time;
        }
    }

    void UpdateTarget()
    {
        // if set to automatic
        if (automatic) {
            // check if it's time for platform to move
            if (Time.time - delayStart > timeDelay) {
                NextTarget();
            }
        }
        // if set to activate when user steps on it
        else {

        }
    }

    public void NextTarget() {
        pointNum += 1;
        if (pointNum >= points.Length) {
            pointNum = 0;
        }
        currentTarget = points[pointNum];
    }

    private void OnTriggerEnter(Collider other) {
        other.transform.parent = transform;
    }
    private void OnTriggerExit(Collider other) {
        other.transform.parent = null;
    }
}

