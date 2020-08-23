
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public bool damage = false;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;


    // Update is called once per frame
    void Update()
    {
        //Checks if player hits the fire button (right mouse click), if so execute Shoot()
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    //Shoots the gun
    //Plays muzzleFlash animation and uses Raycast to hit object
    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) //If target is hit, turn damage on and send bool to GetHit under Target
            {
                damage = true;
                target.GetHit(damage);
            }
        }
    }
}
