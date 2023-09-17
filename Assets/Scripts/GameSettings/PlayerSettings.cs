using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    // Settings for the game that are set by the user
    // these are shared by all the characters, i.e. why it's a different file
    // this does not include camera distance and current camera

    // Creates the PlayerSettings as a singleton to be used across scripts
    public static PlayerSettings Instance {get; private set;}

    // Volume data
    private const string PLAYER_PREFS_MASTER_VOLUME = "MasterVolume";
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    private float masterVolume;
    private float musicVolume;
    private float soundEffectsVolume;
    public event EventHandler masterVolumeChanged;
    public event EventHandler musicVolumeChanged;
    public event EventHandler soundEffectsVolumeChanged;

    // Camera Sensitivity data
    private const string PLAYER_PREFS_LOCKED_CAMERA_SENSITIVITY = "LockedCameraSensitivity";
    private const string PLAYER_PREFS_UNLOCKED_CAMERA_SENSITIVITY = "UnlockedCameraSensitivity";
    private float unlockedSensitivity;
    private float lockedSensitivity;
    public event EventHandler unlockedSensitivityChanged;
    public event EventHandler lockedSensitivityChanged;

    // Locked Camera Side data
    private const string PLAYER_PREFS_LOCKED_CAMERA_SIDE = "Right";
    private string lockedCameraSide;
    public event EventHandler lockedCameraSideChanged;

    // on Awake, gets the data or sets the default values
    private void Awake()
    {
        // set this as a singleton
        Instance = this;

        // gets all the volume level
        masterVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MASTER_VOLUME, 10f);
        musicVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 5f);
        soundEffectsVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 5f);

        // get the sensitivity of the locked and unlocked camera
        unlockedSensitivity = PlayerPrefs.GetFloat(PLAYER_PREFS_UNLOCKED_CAMERA_SENSITIVITY, 100f);
        lockedSensitivity = PlayerPrefs.GetFloat(PLAYER_PREFS_LOCKED_CAMERA_SENSITIVITY, 75f);

        // get the locked camera side
        lockedCameraSide = PlayerPrefs.GetString(PLAYER_PREFS_LOCKED_CAMERA_SIDE, "Right");
    }

    // returns the master volume
    public float GetMasterVolume()
    {
        return masterVolume;
    }

    // sets the master volume
    public void SetMasterVolume(float newMasterVolume)
    {
        // sets the master volume
        masterVolume = newMasterVolume;

        // saves the data to player prefs
        PlayerPrefs.SetFloat(PLAYER_PREFS_MASTER_VOLUME, masterVolume);
        PlayerPrefs.Save();

        // lets the other scripts know the master volume was changed
        masterVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    // returns the music volume
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    // sets the music volume
    public void SetMusicVolume(float newMusicVolume)
    {
        // sets the music volume
        musicVolume = newMusicVolume;

        // saves the data to player prefs
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, musicVolume);
        PlayerPrefs.Save();

        // lets the other scripts know the music volume was changed
        musicVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    // returns the sound effects volume
    public float GetSoundEffectsVolume()
    {
        return soundEffectsVolume;
    }

    public void SetSoundEffectsVolume(float newSoundEffectsVolume)
    {
        // sets the sound effects volume
        soundEffectsVolume = newSoundEffectsVolume;

        // saves the data to player prefs
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, soundEffectsVolume);
        PlayerPrefs.Save();

        // lets the other scripts know the sound effects volume was changed
        soundEffectsVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    // returns the unlocked camera sensitivity
    public float GetUnlockedSensitivity()
    {
        return unlockedSensitivity;
    }

    // sets the new unlocked camera sensitivity
    public void SetUnlockedSensitivity(float newUnlockedSens)
    {
        // changes the unlocked camera sensitivity
        unlockedSensitivity = newUnlockedSens;

        // saves the data to player prefs
        PlayerPrefs.SetFloat(PLAYER_PREFS_UNLOCKED_CAMERA_SENSITIVITY, unlockedSensitivity);
        PlayerPrefs.Save();

        // lets the other scripts know the unlocked camera sensitivity was changed
        unlockedSensitivityChanged?.Invoke(this, EventArgs.Empty);
    }

    // returns the locked camera sensitivity
    public float GetLockedSensitivity()
    {
        return lockedSensitivity;
    }

    // sets the new locked camera sensitivity
    public void SetLockedSensitivity(float newLockedSens)
    {
        // changed the locked camera sensitivity
        lockedSensitivity = newLockedSens;

        // saves the data to player prefs
        PlayerPrefs.SetFloat(PLAYER_PREFS_LOCKED_CAMERA_SENSITIVITY, lockedSensitivity);
        PlayerPrefs.Save();

        // lets the other scripts know the locked camera sensitivity was changed
        lockedSensitivityChanged?.Invoke(this, EventArgs.Empty);
    }

    // set the side of the locked camera
    public void SetLockedCameraSide(bool side)
    {
        // side is a bool so it lines up with the checkbox in the setting window
        // if side is true, the check box is set to the left side
        if(side)
        {
            lockedCameraSide = "Left";
        }
        else
        {
            lockedCameraSide = "Right";
        }

        // saves the side of the camera
        PlayerPrefs.SetString(PLAYER_PREFS_LOCKED_CAMERA_SIDE, lockedCameraSide);
        PlayerPrefs.Save();

        // lets the other scripts know the camera side was changed
        lockedCameraSideChanged?.Invoke(this, EventArgs.Empty);
    }

    // returns the locked camera side
    public string GetLockedCameraSide()
    {
        return lockedCameraSide;
    }

    // returns the locked camera side as a bool, following the same definition as SetLockedCameraSide()
    public bool GetLockedCameraSideBool()
    {
        if(lockedCameraSide == "Left")
        {
            return true;
        }
        return false;
    }
}