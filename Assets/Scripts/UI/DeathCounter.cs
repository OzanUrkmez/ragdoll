using UnityEngine.UI;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
    public Text TextChange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextChange.text = "Lives: " + DeathCheck.Singleton.lives.ToString();
    }
}
