using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        //enforce things like gravity settings put out by the game designer.
        GameProperties.EnforceGameProperties();   


    }
}
