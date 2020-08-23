using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathCheck : MonoBehaviour
{
    public static int lives = 10;
    public Transform checkpoint;
    public GameObject Player;
    public Transform firstcheckpoint;
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            Player.transform.position = checkpoint.transform.position;
            if (lives > 0)
            {
                lives -= 1;
            }

            if (lives == 0)
            {
                checkpoint = firstcheckpoint;
                Player.transform.position = checkpoint.transform.position;
            }
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Checkpoint")
        {
            checkpoint = hit.collider.transform;
        }
        if (hit.collider.tag == "Death")
        {
            Player.transform.position = checkpoint.transform.position;
            if (lives > 0)
            {
                lives -= 1;
            }
            if (lives == 0)
            {
                checkpoint = firstcheckpoint;
                Player.transform.position = checkpoint.transform.position;
            }
        }
    }
}
