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



    //When the game ends
    //

    //public GameObject winGame; //change the GameObject to the file name of the checkpoint that wins the game later, put this into the
    
    //public string nextLevel = "Level02";
    //public int levelToUnlock = 2;

    //public SceneFader sceneFader;

    //public void LevelWon ()
    //{
        //Debug.Log("Level Won!");
        //PlayerPrefs.SetInt("levelReached", levelToUnlock);
        //sceneFader.FadeTo(nextLevel);

    //}
}
