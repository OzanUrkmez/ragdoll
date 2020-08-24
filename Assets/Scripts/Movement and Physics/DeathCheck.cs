﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathCheck : MonoBehaviour
{
    public static int lives = 10;
    public Transform checkpoint;
    public GameObject Player;
    public Transform firstcheckpoint;
    public GameObject ragdoll;
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            ragdoll.transform.position = Player.transform.position;
            ragdoll.SetActive(true);
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

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.collider.tag == "Checkpoint")
        {
            checkpoint = hit.collider.transform;
        }
        if (hit.collider.tag == "Death")
        {
            ragdoll.transform.position = Player.transform.position;
            ragdoll.SetActive(true);
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
