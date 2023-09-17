using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliderController : MonoBehaviour
{
    // this is the basic script for a slider

    [SerializeField] private TextMeshProUGUI valueText;

    private float sliderValue;

    public void OnSliderChanged(float value)
    {
        valueText.text = "Sens: " + value.ToString();
        sliderValue = value;
    }

    public float getValue()
    {
        return sliderValue;
    }
}
