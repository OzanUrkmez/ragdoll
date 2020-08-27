using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public string NextLevel;
    public void OnTriggerEnter()
    {
        SceneManager.LoadScene(NextLevel);
    }
}
