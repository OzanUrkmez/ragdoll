using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathCheck : MonoBehaviour
{
    public static int lives = 10;
    public Transform checkpoint;
    public GameObject Player;
    public Transform firstcheckpoint;
    public GameObject ragdoll;
    public int heightrespawn = 1;
    public GameOverMenu gameOverMenu;
    public Vector3 stop;

    private ForceObject fo1;

    private void Start()
    {
        fo1 = Player.GetComponent<ForceObject>();
    }

    void Update()
    {
        if (Input.GetKeyDown("k"))
        {

            if (lives > 0)
            {
                Instantiate(ragdoll, Player.transform.position, Player.transform.rotation);
                fo1.DirectSetSpeed(stop);
                Player.transform.position = new Vector3(checkpoint.position.x, checkpoint.position.y + heightrespawn, firstcheckpoint.position.z);
                lives -= 1;
            }

            if (lives == 0)
            {
                Player.transform.position = new Vector3(firstcheckpoint.position.x, firstcheckpoint.position.y + heightrespawn, firstcheckpoint.position.z);
                fo1.DirectSetSpeed(stop);
                Player.transform.position = new Vector3(firstcheckpoint.transform.position.x, firstcheckpoint.transform.position.y + heightrespawn, firstcheckpoint.transform.position.z);
                gameOverMenu.GameOver();
                lives = 10;
            }
            
            FindObjectOfType<AudioManager>().Play("PlayerDeath"); //Plays death sound after death
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
            Instantiate(ragdoll, Player.transform.position, Player.transform.rotation);
            fo1.DirectSetSpeed(stop);
            Player.transform.position = new Vector3(checkpoint.position.x, checkpoint.position.y+ heightrespawn, checkpoint.position.z);
            if (lives > 0)
            {
                lives -= 1;
            }
            if (lives == 0)
            {
                checkpoint = firstcheckpoint;
                fo1.DirectSetSpeed(stop);
                Player.transform.position = new Vector3(firstcheckpoint.position.x, firstcheckpoint.position.y + heightrespawn, firstcheckpoint.position.z);
                Player.transform.position = new Vector3(firstcheckpoint.transform.position.x, firstcheckpoint.transform.position.y + heightrespawn, firstcheckpoint.transform.position.z);
                SceneManager.LoadScene("DevAggregate");
                lives = 10;

            }

            FindObjectOfType<AudioManager>().Play("PlayerDeath"); //Plays death sound after death
        }
    }
}
