using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public string NextLevel;
    public SceneFader fader;

    public string nextLevel = "Level02";
    public int levelToUnlock = 2;

    public void OnTriggerEnter()
    {
        Debug.Log("Level Won!");
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        fader.FadeTo(NextLevel);
    }
}
