using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance{get; private set;}

    [SerializeField] private MusicManager backgroundMusic;
    
    private float masterVolume;
    private float soundEffectsVolume;

    // on Awake
    private void Awake()
    {
        // declares this as the singleton
        Instance = this;
    }

    // on Start
    private void Start()
    {
        // listens to the events that it needs to
        PlayerSettings.Instance.masterVolumeChanged += PlayerSettings_MasterSoundChanged;
        PlayerSettings.Instance.musicVolumeChanged += PlayerSettings_MusicVolumeChanged;
        PlayerSettings.Instance.soundEffectsVolumeChanged += PlayerSettings_SoundEffectsVolumeChanged;
        
        // sets the master and sound effect volumes (music volume is handled by the music manager)
        masterVolume = PlayerSettings.Instance.GetMasterVolume();
        soundEffectsVolume = PlayerSettings.Instance.GetSoundEffectsVolume();
    }

    // when MusicVolumeChanged is active, the background music's volume is changed
    private void PlayerSettings_MusicVolumeChanged(object sender, System.EventArgs e)
    {
        backgroundMusic.ChangeVolume();
    }
    
    // when MasterSoundChanged is active, the master volume is changed
    private void PlayerSettings_MasterSoundChanged(object sender, System.EventArgs e)
    {
        masterVolume = PlayerSettings.Instance.GetMasterVolume();

        // the background music's volume is changed as master and music volume go into the volume
        backgroundMusic.ChangeVolume();
    }

    // when SoundEffectsVolumeChanged is active, the sound effects volume is changed
    private void PlayerSettings_SoundEffectsVolumeChanged(object sender, System.EventArgs e)
    {
        soundEffectsVolume = PlayerSettings.Instance.GetSoundEffectsVolume();
    }

    // private void PlayMusic(AudioClip audioClip, Vector3 position)
    // {
    //     AudioSource.PlayClipAtPoint(audioClip, position,  musicSlider.value * masterVolume);
    // }

    // private void PlaySound(AudioClip[] audioClipArray, Vector3 position)
    // {
    //     PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position);
    // }

    // plays a sound at a position (mainly used for sound effects)
    public void PlaySound(AudioClip audioClip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, (masterVolume/10) * (soundEffectsVolume/10));
    }
}
