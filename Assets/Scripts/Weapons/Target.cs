using UnityEngine;

public class Target : MonoBehaviour
{
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
        Destroy(gameObject);
    }
}
