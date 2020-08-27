using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLatePlayer : MonoBehaviour
{
    [SerializeField]
    private float secondDelay;

    private void Start()
    {
        Invoke("PlayAudio", secondDelay);
    }

    void PlayAudio()
    {
        this.GetComponent<AudioSource>().Play();
    }

}
