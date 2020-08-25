using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;

    public void Select (string levelName)
    {
        fader.FadeTo(levelName);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        fader.FadeTo(SceneManager.GetActiveScene().name);
    }
}
