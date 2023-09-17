using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedSensitivitySliderController : MonoBehaviour
{
    // both sensitivity slider controllers work the same
    
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Slider slider;

    private float sliderValue;

    // on Start
    private void Start()
    {
        // set the value and text based off player prefs
        sliderValue = PlayerSettings.Instance.GetUnlockedSensitivity();
        valueText.text = "Unlocked Camera Sensitivity: " + sliderValue;
        slider.value = sliderValue;
    }

    // when the value is changed, the value and text is changed and sent to player settings
    public void OnSliderChanged(float value)
    {
        valueText.text = "Unlocked Camera Sensitivity: " + value.ToString();
        sliderValue = value;
        
        PlayerSettings.Instance.SetUnlockedSensitivity(value);
    }
}
