using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeastiaryHUDButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Keybind;
    [SerializeField] private Button beastiaryButton;
    [SerializeField] private Button LockedBeastiaryButton;
    [SerializeField] private BeastiaryMenu beastiaryMenu;
    [SerializeField] private Image notificationIcon;

    // on Awake
    private void Awake()
    {
        // when the ability button is clicked, it will toggle the ability menu
        beastiaryButton.onClick.AddListener(() => {
            if(!beastiaryMenu.GetIsMenuOpen())
            {
                beastiaryMenu.Show();
            }
            else
            {
                beastiaryMenu.Hide();
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
    }

    // TODO - listens for an achievement to be completed
    // private void CharacterData_LevelUp(object sender, System.EventArgs e)
    // {
    //     // determines if the locked icon should be there or not
    //     LockedButtonTurnOnOrOff();
    //     // determines if the notification icon needs to be there
    //     HideOrShowNotificiation();
    // }

    // determines if the locked icon should be there or not
    private void LockedButtonTurnOnOrOff()
    {
        // TODO - lock beastiary until a certain point
        if(PlayerManager.Instance.currentCharacterData.GetPlayerLevel() > 0)
        {
            LockedBeastiaryButton.gameObject.SetActive(false);
            beastiaryButton.gameObject.SetActive(true);
        }
        else
        {
            LockedBeastiaryButton.gameObject.SetActive(true);
            beastiaryButton.gameObject.SetActive(false);
        }
    }
    
    // determines if the notification icon needs to be there
    private void HideOrShowNotificiation()
    {
        // TODO - sees if a an achievement is completed
        // if(PlayerManager.Instance.currentCharacterData.GetUnspentSkillPoints() > 0 || PlayerManager.Instance.currentCharacterData.GetUnspentStatPoints() > 0)
        // {
        //     notificationIcon.gameObject.SetActive(true);
        // }
        // else
        // {
        //     notificationIcon.gameObject.SetActive(false);
        // }
        notificationIcon.gameObject.SetActive(false);
    }
}
