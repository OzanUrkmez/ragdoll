using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    [SerializeField]

    public GameObject platform;

    private List<Transform> collidingTransforms = new List<Transform>();

    void OnCollisionEnter(Collision collision)
    {
        collidingTransforms.Add(collision.transform);
        ExecuteButtonWallLogic();
    }

    void OnCollisionExit(Collision collision)
    {
        collidingTransforms.Remove(collision.transform);
        ExecuteButtonWallLogic();
    }

    private void ExecuteButtonWallLogic()
    {
        if(collidingTransforms.Count == 0)
        {
            platform.SetActive(false);
        }
        else
        {
            platform.SetActive(true);
        }
    }

}
