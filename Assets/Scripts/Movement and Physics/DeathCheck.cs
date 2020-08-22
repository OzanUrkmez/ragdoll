
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathCheck : MonoBehaviour
{
    public static int lives = 200;
    public AudioSource audiodata;

    void Update()
    {
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Death")
        {
            if (lives > 0)
            {
                lives -= 1;
                Debug.Log(lives);
            }
            else
            {
                audiodata.Play(0);
            }

        }
    }
}
