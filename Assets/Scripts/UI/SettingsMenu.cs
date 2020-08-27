using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioMixer audioMixer;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public bool menuActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Looks for the 'ESC' key input
        {
            if (gameObject.activeSelf)//Checks if the options menu is open
            {
                menuActive = true;
            }

            if (menuActive) //If ooptions menu is open, close the options menu
            {
                settingsMenu.SetActive(false);
                pauseMenu.SetActive(true);
            }
        }
    }

    public void SetVolume (float volumeMultiplier) //Sets volume of other sound effects in audio manager
    {
        for(int i = 0; i < audioManager.sounds.Length; i++) {
            Sound sound = audioManager.sounds[i];
            if (sound.soundType != SoundType.VFX)
                continue;
            sound.source.volume = sound.volume * volumeMultiplier;
        }
    }

    public void SetVolumeOther (float volume) //Sets volume of sound sources attached to mixer
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetMusic(float musicVolumeMultiplier) //Sets volume of music
    {
        for (int i = 0; i < audioManager.sounds.Length; i++)
        {
            Sound sound = audioManager.sounds[i];
            if (sound.soundType != SoundType.Music)
                continue;
            sound.source.volume = sound.volume * musicVolumeMultiplier;
        }
    }

    
}
