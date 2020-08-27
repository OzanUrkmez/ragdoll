using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;

    public Button[] levelButtons;

    void Start ()
    { 
        int levelReached = PlayerPrefs.GetInt ("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached) //If the level is more than level reached, make the button non-interactable
            {
                levelButtons[i].interactable = false;
            }
        }
    }

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
