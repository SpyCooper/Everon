using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MPBar : MonoBehaviour
{
    [SerializeField] private Image currentMPBar;
    [SerializeField] private TextMeshProUGUI mpText;
    private CharacterData characterData;

    // on Start
    private void Start()
    {
        // sets the character data
        characterData = PlayerManager.Instance.currentCharacterData;

        // updates the MP bar
        UpdateMPBar();
        
        // all of these event point to the same function as they all have to do with MP change
        characterData.PlayerLeveledUp += CharacterData_MPBarChanged;
        characterData.PlayerManaChanged += CharacterData_MPBarChanged;
        characterData.SpentStatOrSkillPoint += CharacterData_MPBarChanged;
    }

    // updates the MP bar based on the current MP and max MP
    public void UpdateMPBar()
    {
        mpText.text = "MP: " + characterData.GetCurrentMP().ToString() + "/" + characterData.GetMaxMP().ToString();

        float mpBarPercentage = (float)characterData.GetCurrentMP() / characterData.GetMaxMP();
        currentMPBar.fillAmount = mpBarPercentage;
    }

    // when called, updates the MP bar
    private void CharacterData_MPBarChanged(object sender, EventArgs e)
    {
        UpdateMPBar();
    }
    
}
