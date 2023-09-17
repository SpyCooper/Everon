using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField] private Image xpEarnedBar;
    private CharacterData characterData;

    private void Start()
    {
        // sets the character data
        characterData = PlayerManager.Instance.currentCharacterData;
        
        // updates the XP bar
        UpdateXPBar();

        // all of these event point to the same function as they all have to do with XP change
        characterData.PlayerLeveledUp += CharacterData_XPChange;
        characterData.PlayerGainedXP += CharacterData_XPChange;
    }

    // updates the XP bar based on the current XP and max XP
    public void UpdateXPBar()
    {
        if(characterData.GetPlayerXP() >= characterData.GetMaxXP())
        {
            characterData.IncreasePlayerLevel();
        }
        float xpBarPercentage = (float)characterData.GetPlayerXP() / characterData.GetMaxXP();
        xpEarnedBar.fillAmount = xpBarPercentage;
    }

    // when called, updates the XP bar
    private void CharacterData_XPChange(object sender, EventArgs e)
    {
        UpdateXPBar();
    }
}
