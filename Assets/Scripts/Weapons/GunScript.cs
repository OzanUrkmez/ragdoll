
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public bool damage = false;
    public float range = 100f;
    public int power = 100;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    // temp
    public GameObject Player;
    public GameObject bullet;
    CustomCarolForce fire;


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
            // generates a bullet
            // GameObject bullet2;
            // bullet2 = Instantiate(bullet, hit.point, fpsCam.transform.rotation);
            // bullet2 = transform.TransformDirection(Vector3.forward * 10);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) //If target is hit, turn damage on and send bool to GetHit under Target
            {
                if (hit.rigidbody != null) {
                    hit.rigidbody.AddForceAtPosition(fpsCam.transform.forward * power, hit.point);
                }
                damage = true;
                target.GetHit(damage);
            }
        }
    }
}
