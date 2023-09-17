using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicSliderController : MonoBehaviour
{
    // all the sound setting sliders are the same
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Slider slider;

    private float sliderValue;

    // on Start
    private void Start()
    {
        // sets the value based on the values saved in the player prefs
        sliderValue = PlayerSettings.Instance.GetMusicVolume();
        valueText.text = "Music Volume: " + sliderValue;
        slider.value = sliderValue;
    }

    public void OnSliderChanged(float value)
    {
        // save the value based on the value move on the slider
        valueText.text = "Music Volume: " + value.ToString();
        sliderValue = value;
        
        PlayerSettings.Instance.SetMusicVolume(value);
    }
}
