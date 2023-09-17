using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockedSensitivitySliderController : MonoBehaviour
{
    // both sensitivity slider controllers work the same

    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Slider slider;

    private float sliderValue;

    // on Start
    private void Start()
    {
        // set the value and text based off player prefs
        sliderValue = PlayerSettings.Instance.GetLockedSensitivity();
        valueText.text = "Locked Camera Sensitivity: " + sliderValue;
        slider.value = sliderValue;
    }
    
    // when the value is changed, the value and text is changed and sent to player settings
    public void OnSliderChanged(float value)
    {
        valueText.text = "Locked Camera Sensitivity: " + value.ToString();
        sliderValue = value;
        
        PlayerSettings.Instance.SetLockedSensitivity(value);
    }
}
