
using System.Runtime.CompilerServices;
using UnityEngine;

public class ColliderDetectorObject : MonoBehaviour
{
    public CharacterController CharacterController;
    void Update()
    {
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Death")
        {
            Debug.Log("Ouch");
        }
    }
}
