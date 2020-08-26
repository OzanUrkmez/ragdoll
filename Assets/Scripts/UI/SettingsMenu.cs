using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioManager audioManager;

    public void SetVolume (float volumeMultiplier)
    {
        for(int i = 0; i < audioManager.sounds.Length; i++) {
            Sound sound = audioManager.sounds[i];
            sound.source.volume = sound.volume * volumeMultiplier;
        }
    }

    public void SetMusic(float musicVolumeMultiplier)
    {

    }
}
