using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChangerNPC : MonoBehaviour
{
    public Vector3 newGravity;
    void Start()
    {
        ForceObject fo = GetComponent<ForceObject>();
        fo.SetGravityBaseForce(newGravity);
    }

}
