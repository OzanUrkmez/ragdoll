using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //Loads the next scene from the current scene
    public void GameLoader()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Quits the Game
    public void GameQuit()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}
