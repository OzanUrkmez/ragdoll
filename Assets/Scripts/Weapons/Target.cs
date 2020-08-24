using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject ragdoll;
    public Camera fpsCam;
    public GameObject ragdoll2;
    
    

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
        ragdoll.transform.position = gameObject.transform.position;
        Destroy(gameObject);
        ragdoll.SetActive(true);
        ragdoll2.SetActive(true);
        ragdoll.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * 15000f);
    }
}
