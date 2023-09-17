using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectSliderController : MonoBehaviour
{
    // all the sound setting sliders are the same

    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Slider slider;

    private float sliderValue;

    // on Start
    private void Start()
    {
        // sets the value based on the values saved in the player prefs
        sliderValue = PlayerSettings.Instance.GetSoundEffectsVolume();
        valueText.text = "Sound Effects Volume: " + sliderValue;
        slider.value = sliderValue;
    }

    public void OnSliderChanged(float value)
    {
        // save the value based on the value move on the slider
        valueText.text = "Sound Effects Volume: " + value.ToString();
        sliderValue = value;
        
        PlayerSettings.Instance.SetSoundEffectsVolume(value);
    }
}

