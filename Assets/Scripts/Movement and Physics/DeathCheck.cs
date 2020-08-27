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
    public GameObject frozen;
    public int heightrespawn = 1;
    public GameOverMenu gameOverMenu;
    public Vector3 stop;
    private Collider pCollider;
    public Vector3 gravity;

    private ForceObject fo1;

    public static DeathCheck Singleton;

    private void Start()
    {
        gravity = GameProperties.Singleton.BaseGravity;

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
        if (Input.GetKeyDown("q"))
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
        }else if (Input.GetKeyDown("e"))
        {
            if (lives > 0)
            {
                Instantiate(frozen, Player.transform.position, Player.transform.rotation);
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
            ForceObject fo = GetComponent<ForceObject>();
            gravity = fo.GetGravityForce();
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                lives = 10;

            }

            FindObjectOfType<AudioManager>().Play("PlayerDeath"); //Plays death sound after death
        }
    }

    public void Checkpointer1()
    {
        fo1.DirectSetSpeed(stop);
        fo1.SetGravityBaseForce(gravity);
        Player.transform.position = new Vector3(checkpoint.position.x, checkpoint.position.y + heightrespawn,
            checkpoint.position.z);
    }

}
