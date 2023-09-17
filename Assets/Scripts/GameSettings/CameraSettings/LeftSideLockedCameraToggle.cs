using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftSideLockedCameraToggle : MonoBehaviour
{
    // Toggles the locked camera side

    [SerializeField] private Toggle toggleButton;

    private bool toggleStatus;

    // on Start
    private void Start()
    {
        // sets the toggle status based on the save value in the player prefs
        toggleStatus = PlayerSettings.Instance.GetLockedCameraSideBool();
        toggleButton.isOn = toggleStatus;
    }

    // when the value is changed, the side is changed in the player settings
    public void OnValueChanged(bool toggle)
    {
        toggleStatus = toggle;

        PlayerSettings.Instance.SetLockedCameraSide(toggle);
    }
}
