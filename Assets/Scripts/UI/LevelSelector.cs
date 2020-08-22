using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;
    
    public void Select (string levelName)
    {
        fader.FadeTo(levelName);
    }
}
