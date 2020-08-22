using UnityEngine.UI;
using UnityEngine;

public class tester : MonoBehaviour
{
    public GameObject knife;
    // Update is called once per frame 
    void Update()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            Debug.Log(knife.GetComponent<Sprite>());
        }
            
    }
}
