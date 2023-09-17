using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // This is used to identify an enemy to the hitbox detection, as well as, to get data on the enemy

    //the enemy's SO
    [SerializeField] private EnemyScriptableObject enemySO;

    private int currentHP;
    [SerializeField] private Image currentHPBar;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private GameObject infoBar;
    private float infoBarViewDistance = 25f;
    
    // on Start, sets up the UI of the enemy
    private void Start()
    {
        currentHP = enemySO.health;
        UpdateHPBar();
        nameText.text = enemySO.enemyName;
        levelText.text = "Lvl " + enemySO.enemyLevel;
    }

    // on Update
    private void Update()
    {
        // if the player is out of range the UI disappears
        // UI range is set in the variables section
        if(Physics.CheckSphere(transform.position, infoBarViewDistance, playerLayerMask))
        {
            infoBar.SetActive(true);
        }
        else
        {
            infoBar.SetActive(false);
        }
    }

    // Takes damage based on the damage inputted
    public void TakeDamage(int attack)
    {
        if(currentHP > 0)
        {
            currentHP -= PlayerManager.Instance.DamageCalculation(attack, enemySO.defense);
            UpdateHPBar();
        }
    }

    // Deals damage to the player
    public void Attack()
    {
        PlayerManager.Instance.currentCharacterData.PlayerHit(enemySO.attack);
    }

    // Updates the HP bar text and fill amount
    private void UpdateHPBar()
    {
        currentHPBar.fillAmount = (float) currentHP / enemySO.health;
        hpText.text = "HP:" + currentHP + "/" + enemySO.health;
    }

    // returns the current HP
    public int GetCurrentHP()
    {
        return currentHP;
    }

    // returns the XP recieved on kill
    public int GetXP()
    {
        return enemySO.xpOnKill;
    }
}
