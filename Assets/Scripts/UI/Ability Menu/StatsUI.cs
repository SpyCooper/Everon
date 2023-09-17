using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    // stats
    [SerializeField] private TextMeshProUGUI strText;
    [SerializeField] private Button increaseSTR;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private Button increaseEND;
    [SerializeField] private TextMeshProUGUI dexText;
    [SerializeField] private Button increaseDEX;
    [SerializeField] private TextMeshProUGUI intText;
    [SerializeField] private Button increaseINT;

    // colors for the buttons
    private Color canBeClickedColor = new Color(1.000f, 0.725f, 0.000f, 1.000f); 
    private Color cannotBeClickedColor = new Color(0.478f, 0.478f, 0.478f, 1.000f);

    // on Awake
    private void Awake()
    {
        // sets up all the increase stat buttons

        increaseSTR.onClick.AddListener(() => {
            if(PlayerManager.Instance.currentCharacterData.GetUnspentStatPoints() > 0)
            {
                // increase STR and decrease unspent stat points 
                // (number corresponds to the position in the list)
                PlayerManager.Instance.currentCharacterData.SpendStatPoint(1);
                // refresh stats
                RefreshStats();
            }
        });
        increaseEND.onClick.AddListener(() => {
            if(PlayerManager.Instance.currentCharacterData.GetUnspentStatPoints() > 0)
            {
                // increase END and decrease unspent stat points 
                // (number corresponds to the position in the list)
                PlayerManager.Instance.currentCharacterData.SpendStatPoint(2);
                // refresh stats
                RefreshStats();
            }
        });
        increaseDEX.onClick.AddListener(() => {
            if(PlayerManager.Instance.currentCharacterData.GetUnspentStatPoints() > 0)
            {
                // increase DEX and decrease unspent stat points 
                // (number corresponds to the position in the list)
                PlayerManager.Instance.currentCharacterData.SpendStatPoint(3);
                // refresh stats
                RefreshStats();
            }
        });
        increaseINT.onClick.AddListener(() => {
            if(PlayerManager.Instance.currentCharacterData.GetUnspentStatPoints() > 0)
            {
                // increase INT and decrease unspent stat points 
                // (number corresponds to the position in the list)
                PlayerManager.Instance.currentCharacterData.SpendStatPoint(4);
                // refresh stats
                RefreshStats();
            }
        });
    }

    // on Start
    private void Start()
    {
        // listens to the events it needs to
        PlayerManager.Instance.currentCharacterData.PlayerLeveledUp += CharacterData_LevelUp;
    }
    
    // refreshes all the stats
    public void RefreshStats()
    {
        // Sets up the stats text
        strText.text = "STR " + PlayerManager.Instance.currentCharacterData.GetSTR();
        endText.text = "END " + PlayerManager.Instance.currentCharacterData.GetEND();
        dexText.text = "DEX " + PlayerManager.Instance.currentCharacterData.GetDEX();
        intText.text = "INT " + PlayerManager.Instance.currentCharacterData.GetINT();

        // if there are unspent stat points, the buttons are yellow
        if(PlayerManager.Instance.currentCharacterData.GetUnspentStatPoints() > 0)
        {
            increaseSTR.image.color = canBeClickedColor;
            increaseEND.image.color = canBeClickedColor;
            increaseDEX.image.color = canBeClickedColor;
            increaseINT.image.color = canBeClickedColor;
        }
        // if there are no spent stat points, the buttons are grey
        else
        {
            increaseSTR.image.color = cannotBeClickedColor;
            increaseEND.image.color = cannotBeClickedColor;
            increaseDEX.image.color = cannotBeClickedColor;
            increaseINT.image.color = cannotBeClickedColor;
        }
    }

    // on level up, refresh stats
    private void CharacterData_LevelUp(object sender, System.EventArgs e)
    {
        RefreshStats();
    }
}
