using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    public static MusicManager Instance{get; private set;}
    
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicSliderText;

    private AudioSource audioSource;
    private float volume;

    // on Awake
    private void Awake()
    {
        // declares this as the singleton
        Instance = this;
    }

    // on Start
    private void Start()
    {
        // set sets the volume based on the player settings
        volume = PlayerSettings.Instance.GetMusicVolume();
        
        // gets the attached audio source
        audioSource = GetComponent<AudioSource>();
        // sets the audio source volume to be volume/10 because volume is a normalized value
        audioSource.volume = volume/10;
    }

    // when the volume is changed
    public void ChangeVolume()
    {
        // volume is adjsuted
        audioSource.volume = (PlayerSettings.Instance.GetMasterVolume()/10) * (PlayerSettings.Instance.GetMusicVolume()/10);
        
        // saves the music volume
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, PlayerSettings.Instance.GetMusicVolume());
        PlayerPrefs.Save();
    }
}

