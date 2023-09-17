using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    // handles the options/settings menu in game
    // most likely is going to change

    public static OptionsMenu Instance {get; private set;}

    private const string MAIN_MENU_SCENE_NAME = "MainMenuScene";

    [SerializeField] private Button closeButton;
    [SerializeField] private Button mainMenuOptionsButton;

    [SerializeField] private Button generalSettingsButton;
    [SerializeField] private Button soundSettingButton;
    [SerializeField] private Button bindingsButton;
    [SerializeField] private GameObject generalSettingsTab;
    [SerializeField] private GameObject soundSettingTab;
    [SerializeField] private GameObject bindingsTab;

    // sets the current menu tab that is open
    private enum CurrentMenu 
    {
        GeneralSettings,
        SoundSettings,
        KeyBindings,
    };

    private CurrentMenu currentMenu;

    // on Awake
    private void Awake()
    {
        // set the singleton
        Instance = this;

        // when the close button is clicked, then menu closes and it will select the main menu options button if it is on the main menu
        closeButton.onClick.AddListener(() => {
            string sceneName = SceneManager.GetActiveScene().name;
            CloseMenu();
            if(sceneName == MAIN_MENU_SCENE_NAME)
            {
                mainMenuOptionsButton.Select();
            }
        });

        // when the general settings tab is clicked, closes the other tabs and opens the general settings tab
        generalSettingsButton.onClick.AddListener(() => {
            generalSettingsTab.gameObject.SetActive(true);

            if(currentMenu == CurrentMenu.SoundSettings)
            {
                CloseSoundSetting();
            }
            else if(currentMenu == CurrentMenu.KeyBindings)
            {
                CloseBindings();
            }

            currentMenu = CurrentMenu.GeneralSettings;
        });
        
        // when the sound settings tab is clicked, closes the other tabs and opens the sound settings tab
        soundSettingButton.onClick.AddListener(() => {
            soundSettingTab.gameObject.SetActive(true);

            if(currentMenu == CurrentMenu.GeneralSettings)
            {
                CloseGeneralSetting();
            }
            else if(currentMenu == CurrentMenu.KeyBindings)
            {
                CloseBindings();
            }

            currentMenu = CurrentMenu.SoundSettings;
        });
        
        // when the bindings settings tab is clicked, closes the other tabs and opens the bindings settings tab
        bindingsButton.onClick.AddListener(() => {
            bindingsTab.gameObject.SetActive(true);

            if(currentMenu == CurrentMenu.GeneralSettings)
            {
                CloseGeneralSetting();
            }
            else if(currentMenu == CurrentMenu.SoundSettings)
            {
                CloseSoundSetting();
            }

            currentMenu = CurrentMenu.KeyBindings;
        });
    }

    // on Start, closes the menu
    // the menu has to be open in the editor for some reason
    private void Start()
    {
        CloseMenu();
    }

    // opens the menu and sets the tab to the general settings menu
    public void OpenMenu()
    {
        gameObject.SetActive(true);
        currentMenu = CurrentMenu.GeneralSettings;
        generalSettingsTab.gameObject.SetActive(true);
        soundSettingTab.gameObject.SetActive(false);
    }

    // closes the menu
    private void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    // closes the general settings tab
    private void CloseGeneralSetting()
    {
        generalSettingsTab.gameObject.SetActive(false);
    }
    
    // closes the sound settings tab
    private void CloseSoundSetting()
    {
        soundSettingTab.gameObject.SetActive(false);
    }

    // closes the bindings tab
    private void CloseBindings()
    {
        bindingsTab.gameObject.SetActive(false);
    }
}