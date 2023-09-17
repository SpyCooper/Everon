using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // hp variables
    public int maxHp;
    public int currentHP;

    // mp variables
    public int maxMP;
    public int currentMP;

    // level variables
    public int currentXP;
    public int maxXPForLevel;
    public int level;

    // camera state
    public int cameraState;
    public float cameraDistance;

    // SEDI Stats
    public int strength;
    public int endurance;
    public int dexterity;
    public int intelligence;
    public int unspentStatPoints;

    // Skill points
    public int unspentSkillPoints;

    // TODO - add current scene

    // current closest spawn point
    public GameObject spawnPoint;

    // TODO - add inventory items and equipment
    // make default weapon fists
    public int playerCurrency;
    public WeaponSO currentWeapon;

    // TODO - used for things that should be compeleted once (mainly quests)
    public SerializableDictionary<string, bool> questsCompleted;

    //the values in the constructor are the default values on a new game/ with not data to load
    public GameData()
    {
        // default HP
        maxHp = 25;
        currentHP = maxHp;

        // default level data
        currentXP = 0;
        maxXPForLevel = 100;
        level = 0;

        // default MP
        maxMP = 0;
        currentMP = 0;

        // default camera data
        cameraState = 1;
        cameraDistance = 4.5f;

        // default stat data
        strength = 0;
        endurance = 0;
        dexterity = 0;
        intelligence = 0;
        unspentStatPoints = 0;

        // no starting skill points
        unspentSkillPoints = 0;
        
        // no quests completed to start with
        questsCompleted = new SerializableDictionary<string, bool>();

        // player starts with no currency
        playerCurrency = 0;
    }
}
