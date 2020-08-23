using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Looks for the 'ESC' key input
        {
            if (GameIsPaused) //If game is paused, resume the game again
            {
                Resume();
            }
            else //If not already paused, pause the game
            {
                Pause();
            }
        }
    }

    //Resumes the game by starting time and setting GameIsPaused to false
    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Pauses the game by stopping time and setting GameIsPaused to true
    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //Starts time again and brings player out of play mode
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        GameIsPaused = false;
    }
}
