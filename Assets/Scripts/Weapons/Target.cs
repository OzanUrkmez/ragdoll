using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject ragdoll;
    public Camera fpsCam;
    public float range = 100f;
    public bool destroy = false;

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
        Instantiate(ragdoll, gameObject.transform.position, gameObject.transform.rotation);
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
}
