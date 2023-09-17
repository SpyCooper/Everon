using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHUDButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Keybind;
    [SerializeField] private Button AbilityButton;
    [SerializeField] private Button LockedAbilityButton;
    [SerializeField] private AbilitiesMenu abilitiesMenu;
    [SerializeField] private Image notificationIcon;

    // on Awake
    private void Awake()
    {
        // when the ability button is clicked, it will toggle the ability menu
        AbilityButton.onClick.AddListener(() => {
            if(!abilitiesMenu.GetIsMenuOpen())
            {
                abilitiesMenu.Show();
            }
            else
            {
                abilitiesMenu.Hide();
            }
        });
    }

    // on Start
    private void Start()
    {
        // determines if the locked icon should be there or not
        LockedButtonTurnOnOrOff();

        // determines if the notification icon needs to be there
        HideOrShowNotificiation();

        // listens to the events that it needs to
        PlayerManager.Instance.currentCharacterData.PlayerLeveledUp += CharacterData_LevelUp;
        PlayerManager.Instance.currentCharacterData.SpentStatOrSkillPoint += CharacterData_SpentStatOrSkillPoint;
    }

    // on character level up
    private void CharacterData_LevelUp(object sender, System.EventArgs e)
    {
        // determines if the locked icon should be there or not
        LockedButtonTurnOnOrOff();
        // determines if the notification icon needs to be there
        HideOrShowNotificiation();
    }

    // determines if the locked icon should be there or not
    private void LockedButtonTurnOnOrOff()
    {
        // if the player is above level 0, the locked button is disabled and the normal button is enabled
        if(PlayerManager.Instance.currentCharacterData.GetPlayerLevel() > 0)
        {
            LockedAbilityButton.gameObject.SetActive(false);
            AbilityButton.gameObject.SetActive(true);
        }
        // if the player is level 0, the locked button is enabled and the normal button is disabled
        else
        {
            LockedAbilityButton.gameObject.SetActive(true);
            AbilityButton.gameObject.SetActive(false);
        }
    }
    
    // determines if the notification icon needs to be there
    private void HideOrShowNotificiation()
    {
        // if the character has unspent stat or skill points, the notificiation will be shown
        if(PlayerManager.Instance.currentCharacterData.GetUnspentSkillPoints() > 0 || PlayerManager.Instance.currentCharacterData.GetUnspentStatPoints() > 0)
        {
            notificationIcon.gameObject.SetActive(true);
        }
        else
        {
            notificationIcon.gameObject.SetActive(false);
        }
    }

    // when SpentStatOrSkillPoint is active, it will determine if the notification needs to be shown
    private void CharacterData_SpentStatOrSkillPoint(object sender, System.EventArgs e)
    {
        HideOrShowNotificiation();
    }
}
