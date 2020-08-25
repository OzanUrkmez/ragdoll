using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;
    public GameObject pause;


    //Resumes the game by starting time and setting GameIsPaused to false
    public void GameOver ()
    {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pause.SetActive(false);

    }

    //Starts time again and brings player out of play mode
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
    }
}
