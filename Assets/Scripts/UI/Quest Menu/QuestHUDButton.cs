using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestHUDButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Keybind;
    [SerializeField] private Button questButton;
    [SerializeField] private QuestMenu questMenu;
    [SerializeField] private Image notificationIcon;

    // on Awake
    private void Awake()
    {
        // when the ability button is clicked, it will toggle the ability menu
        questButton.onClick.AddListener(() => {
            if(!questMenu.GetIsMenuOpen())
            {
                questMenu.Show();
            }
            else
            {
                questMenu.Hide();
            }
        });
    }

    // on Start
    private void Start()
    {
        // determines if the notification icon needs to be there
        HideOrShowNotificiation();
    }

    // TODO - listens for a quest added
    // private void CharacterData_LevelUp(object sender, System.EventArgs e)
    // {
    //     // determines if the notification icon needs to be there
    //     HideOrShowNotificiation();
    // }
    
    // determines if the notification icon needs to be there
    private void HideOrShowNotificiation()
    {
        // TODO - sees if a quest is done
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
