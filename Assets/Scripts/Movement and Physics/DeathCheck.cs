
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathCheck : MonoBehaviour
{
    public static int lives = 200;
    public Transform checkpoint;
    public GameObject Player;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Death")
        {
            Player.transform.position = checkpoint.transform.position;
            if (lives > 0)
            {
                lives -= 1;
                Debug.Log(lives);
            }
            

        }
    }
}
