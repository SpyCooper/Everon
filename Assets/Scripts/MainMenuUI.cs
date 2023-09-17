using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    // sets up the main menu

    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    // on Start up
    private void Awake()
    {
        // sets up the buttons to do what they say

        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });
        
        optionsButton.onClick.AddListener(() => {
            // gameObject.SetActive(false);
            OptionsMenu.Instance.OpenMenu();
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit(); // Quits the game
        });
    }

    // on start, the main menu is set to be active
    private void Start()
    {
        gameObject.SetActive(true);
    }
}