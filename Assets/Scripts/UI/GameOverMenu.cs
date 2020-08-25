using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;


    // Update is called once per frame
    void Update()
    {
        if (DeathCheck.lives == 0) //Checks if death count under 0
        {
            GameOver();
            Debug.Log("Restart");

        }
    }

    //Resumes the game by starting time and setting GameIsPaused to false
    public void GameOver ()
    {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    //Starts time again and brings player out of play mode
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
    }
}
