using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathCheck : MonoBehaviour
{
    public int lives = 10;
    public Transform checkpoint;
    public GameObject Player;
    public Transform firstcheckpoint;
    public GameObject ragdoll;
    public int heightrespawn = 1;
    public GameOverMenu gameOverMenu;
    public Vector3 stop;
    private Collider pCollider;

    private ForceObject fo1;

    public static DeathCheck Singleton;

    private void Start()
    {
        pCollider = GetComponent<Collider>();
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }

        Singleton = this;

        fo1 = Player.GetComponent<ForceObject>();
    }

    void Update()
    {
        if (Input.GetKeyDown("k"))
        {

            if (lives > 0)
            {
                Instantiate(ragdoll, Player.transform.position, Player.transform.rotation);
                Invoke("Checkpointer1", 0);
                lives -= 1;
            }

            if (lives == 0)
            {
                Player.transform.position = new Vector3(firstcheckpoint.position.x,
                    firstcheckpoint.position.y + heightrespawn, firstcheckpoint.position.z);
                fo1.DirectSetSpeed(stop);
                Player.transform.position = new Vector3(firstcheckpoint.transform.position.x,
                    firstcheckpoint.transform.position.y + heightrespawn, firstcheckpoint.transform.position.z);
                gameOverMenu.GameOver();
                lives = 10;
            }

            FindObjectOfType<AudioManager>().Play("PlayerDeath"); //Plays death sound after death
        }
    }

    private void OnCollisionEnter(Collision hit)
    {
        OnDeathCollision(hit.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnDeathCollision(other);
    }

    private void OnDeathCollision(Collider hit)
    {
        if (hit.tag == "Checkpoint")
        {
            checkpoint = hit.transform;
        }

        if (hit.tag == "Death")
        {

            if (lives > 0)
            {
                Instantiate(ragdoll, Player.transform.position, Player.transform.rotation);
                Invoke("Checkpointer1", 0);
                lives -= 1;
            }

            if (lives == 0)
            {
                checkpoint = firstcheckpoint;
                fo1.DirectSetSpeed(stop);
                Player.transform.position = new Vector3(firstcheckpoint.position.x,
                    firstcheckpoint.position.y + heightrespawn, firstcheckpoint.position.z);
                Player.transform.position = new Vector3(firstcheckpoint.transform.position.x,
                    firstcheckpoint.transform.position.y + heightrespawn, firstcheckpoint.transform.position.z);
                SceneManager.LoadScene("DevAggregate");
                lives = 10;

            }

            FindObjectOfType<AudioManager>().Play("PlayerDeath"); //Plays death sound after death
        }
    }

    public void Checkpointer1()
    {
        fo1.DirectSetSpeed(stop);
        Player.transform.position = new Vector3(checkpoint.position.x, checkpoint.position.y + heightrespawn,
            checkpoint.position.z);
    }

}
