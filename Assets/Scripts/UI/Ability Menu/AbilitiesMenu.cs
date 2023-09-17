using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesMenu : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    private bool isMenuOpen = false;

    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private StatsUI statsUIManager;

    [SerializeField] private Button adventurerAbilitiesTabButton;
    [SerializeField] private AdventurerSkillsUI adventurerSkillsTab;
    private bool adventurerSkillsUIActive;

    // used when classes are introduced
    // [SerializeField] private Button warriorAbilitiesTab;
    // [SerializeField] private Button hunterAbilitiesTab;
    // [SerializeField] private Button mageAbilitiesTab;

    // on Awake
    private void Awake()
    {
        // when the close button is hit, Hide() is run
        closeButton.onClick.AddListener(() => {
            Hide();
        });

        // when the adventurer abilities button is hit, ToggleAdventurerTab() is run
        adventurerAbilitiesTabButton.onClick.AddListener(() => {
            // open the adventurer abilities tab
            ToggleAdventurerTab();
        });
    }

    // on Start
    private void Start()
    {
        // sets up the XP text and refreshes the stats
        SetXPText();
        statsUIManager.RefreshStats();

        // hide skill tabs
        HideAdventurerSkillsTab();

        // listen for a ability menu key press
        starterAssetsInputs.toggleAbilityMenu += starterAssetsInputs_ToggleAbilitiesMenu;

        // listens to the events needed
        PlayerManager.Instance.currentCharacterData.PlayerGainedXP += CharacterData_XPChange;
        PlayerManager.Instance.currentCharacterData.PlayerLeveledUp += CharacterData_XPChange;

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

        // refreshes stats 
        statsUIManager.RefreshStats();
    }

    // sets the XP text based on current and max xp
    private void SetXPText()
    {
        xpText.text = "EXP: " + PlayerManager.Instance.currentCharacterData.GetPlayerXP() + " / " + PlayerManager.Instance.currentCharacterData.GetMaxXP();
    }
    
    // when XPChange is active, sets up the XP text
    private void CharacterData_XPChange(object sender, EventArgs e)
    {
        SetXPText();
    }

    // opens the adventurer's ability tab if not open, and if open, closes the tab
    private void ToggleAdventurerTab()
    {
        if(!adventurerSkillsUIActive)
        {
            ShowAdventurerSkillsTab();
        }
        else
        {
            HideAdventurerSkillsTab();
        }
    }

    // hides the adventurer skills
    private void HideAdventurerSkillsTab()
    {
        adventurerSkillsTab.Hide();
        adventurerSkillsUIActive = false;
    }

    // shows the adventurer skills
    private void ShowAdventurerSkillsTab()
    {
        adventurerSkillsTab.Show();
        adventurerSkillsUIActive = true;
    }

    // when toggleAbilitiesMenu is active, sets up the XP text
    private void starterAssetsInputs_ToggleAbilitiesMenu(object sender, System.EventArgs e)
    {
        // checks to make sure the menu should be available to the player
        if(PlayerManager.Instance.currentCharacterData.GetPlayerLevel() > 0)
        {
            // if the menu is not open, it opens the menu
            if(!isMenuOpen)
            {
                Show();
                // abilityMenuPressed must be set to false because of it's type
                starterAssetsInputs.abilityMenuPressed = false;
            }
            // if the menu is open, it closes the menu
            else
            {
                Hide();
                // abilityMenuPressed must be set to false because of it's type
                starterAssetsInputs.abilityMenuPressed = false;
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
