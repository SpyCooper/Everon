using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Image currentHPBar;
    [SerializeField] private TextMeshProUGUI hpText;
    private CharacterData characterData;

    // private float timer = 1;

    // on Start
    private void Start()
    {
        // sets the character data
        characterData = PlayerManager.Instance.currentCharacterData;

        // updates the HP bar
        UpdateHPBar();

        // all of these event point to the same function as they all have to do with HP change
        characterData.PlayerLeveledUp += characterData_HPChange;
        characterData.PlayerTookDamage += characterData_HPChange;
        characterData.PlayerHealed += characterData_HPChange;
        characterData.SpentStatOrSkillPoint += characterData_HPChange;
    }

    // updates the HP bar based on the current HP and max HP
    private void UpdateHPBar()
    {
        hpText.text = "HP: " + characterData.GetCurrentHP().ToString() + "/" + characterData.GetMaxHP().ToString();

        float hpBarPercentage = (float)characterData.GetCurrentHP() / characterData.GetMaxHP();
        currentHPBar.fillAmount = hpBarPercentage;
    }
    
    // when an HP change occurs, the health bar updates
    private void characterData_HPChange(object sender, EventArgs e)
    {
        UpdateHPBar();
    }
}
