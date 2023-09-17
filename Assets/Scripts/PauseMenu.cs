using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private GameObject resetClarificationWindow;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private Button mainMenuButton;

    public event EventHandler closePauseMenu;

    // on Awake
    private void Awake()
    {
        // closes the clarification window for the reset button
        resetClarificationWindow.SetActive(false);

        // sets up all the buttons on the pause menu
        resumeButton.onClick.AddListener(()=> {
            Hide();
            closePauseMenu?.Invoke(this, EventArgs.Empty);
        });
        optionsButton.onClick.AddListener(()=> {
            // Hide();
            OptionsMenu.Instance.OpenMenu();
        });
        resetButton.onClick.AddListener(()=> {
            // opens the clarification window
            resetClarificationWindow.SetActive(true);
        });
        mainMenuButton.onClick.AddListener(()=> {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        // buttons for the clarification window
        yesButton.onClick.AddListener(()=> {
            resetClarificationWindow.SetActive(false);
            Reset();
            Hide();
            closePauseMenu?.Invoke(this, EventArgs.Empty);
        });
        noButton.onClick.AddListener(()=> {
            resetClarificationWindow.SetActive(false);
        });
    }

    // on Start, closes pause menu
    private void Start()
    {
        gameObject.SetActive(false);
    }

    // opens the pause menu
    public void Show()
    {
        gameObject.SetActive(true);
        // resumeButton.Select();
    }
    
    // closes the pause menu
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // calls the reset function to reset the player
    private void Reset()
    {
        PlayerManager.Instance.currentCharacterData.ResetPlayer();
    }
}
