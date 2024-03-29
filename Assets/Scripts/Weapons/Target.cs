﻿using UnityEngine;

public class Target : MonoBehaviour
{

    [SerializeField]
    private GameObject[] transferredChildren;

    public GameObject ragdoll;
    public Camera fpsCam;
    public float range = 100f;
    private bool destroy = false; 

    //checks if gun has hit the object
    public void GetHit (bool hit)
    {
        if (hit)
        {
            Die();
        }
    }

    //if gun has hit the object, excute this code
    void Die()
    {
        GameObject g = Instantiate(ragdoll, gameObject.transform.position, gameObject.transform.rotation);
        g.GetComponentInChildren<SkinnedMeshRenderer>().material = gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material; //TODO inefficient but good if we are not doing this more than ~5 times per second
        
        foreach(GameObject c in transferredChildren)
        {
            c.transform.parent = g.transform;
        }

        Destroy(gameObject);
        destroy = true; 
    }

    void Update()
    {
        if (destroy == true) //If destroy is true, hit the raycast on to the rigid body
        {
            RaycastHit hitRagdoll;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitRagdoll))
            {
                Debug.Log(hitRagdoll.rigidbody);
            }
            destroy = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // detect if collision is by a bullet
        if (collision.gameObject.tag == "Bullet") {
            GetHit(true);
        }
    }
}
