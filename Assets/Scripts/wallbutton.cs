using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallbutton : MonoBehaviour
{
    [SerializeField]
    private bool deActivateOnTouch = true;

    public GameObject buttonwall;

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
            buttonwall.SetActive(deActivateOnTouch);
        }
        else
        {
            buttonwall.SetActive(!deActivateOnTouch);
        }
    }

}
