using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeastiaryMenu : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    private bool isMenuOpen = false;

    [SerializeField] private Button closeButton;

    private void Awake()
    {
        // when the close button is hit, Hide() is run
        closeButton.onClick.AddListener(() => {
            Hide();
        });
    }

    // Start is called before the first frame update
    private void Start()
    {
        // listen for a ability menu key press
        starterAssetsInputs.toggleBeastiaryMenu += starterAssetsInputs_ToggleBeastiaryMenu;
        
        // hides the menu
        Hide();
    }
    
    // hides the menu
    public void Hide()
    {
        isMenuOpen = false;
        gameObject.SetActive(isMenuOpen);
    }

    // shows the menu
    public void Show()
    {
        // shows the menu
        isMenuOpen = true;
        gameObject.SetActive(isMenuOpen);

        // fixes the menu on the screen if it it too far off the edge of the screen
        FixMenuOnScreen();
    }

    
    // when toggleAbilitiesMenu is active, sets up the XP text
    private void starterAssetsInputs_ToggleBeastiaryMenu(object sender, System.EventArgs e)
    {
        // checks to make sure the menu should be available to the player
        if(PlayerManager.Instance.currentCharacterData.GetPlayerLevel() > 0)
        {
            // if the menu is not open, it opens the menu
            if(!isMenuOpen)
            {
                Show();
                // beastiaryMenuPressed must be set to false because of it's type
                starterAssetsInputs.beastiaryMenuPressed = false;
            }
            // if the menu is open, it closes the menu
            else
            {
                Hide();
                // beastiaryMenuPressed must be set to false because of it's type
                starterAssetsInputs.beastiaryMenuPressed = false;
            }
        }
    }

    // * use this for any of the windows that need it
    // adjust the menu of it is more than halfway off the screen
    private void FixMenuOnScreen()
    {
        if(!(transform.position.x < Screen.width && transform.position.x > 0 && transform.position.y < Screen.height && transform.position.y > 0))
        {
            transform.position = new Vector3(Screen.width/2, Screen.height/2);
        }
    }

    // returns if the menu is open
    public bool GetIsMenuOpen()
    {
        return isMenuOpen;
    }
}
