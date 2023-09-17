using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : IDataPersistence
{
    // Which camera is currently active
    private const string PLAYER_PREFS_CAMERA_STATE = "CameraState";
    // CameraState is changed in the ToggleCamera script

    // XP and Level Data
    private int playerXP;
    private int maxXPForLevel;
    private int playerLevel;
    public event EventHandler PlayerLeveledUp;
    public event EventHandler PlayerGainedXP;

    // Health Data
    private int maxPlayerHP;
    private int currentPlayerHP;
    
    // MP Data
    private int maxPlayerMP;
    private int currentPlayerMP;
    private int mpRegenerationAmount;
    private float mpRegenerationPercentage = 0.05f; // 5% of max MP
    public EventHandler PlayerManaChanged;

    //Combat
    public event EventHandler PlayerTookDamage;
    public event EventHandler PlayerHealed;
    public WeaponSO currentWeapon;

    // Character SEDI Stats
    private int strength;
    private int endurance;
    private int dexterity;
    private int intelligence;
    private int unspentStatPoints;
    private int statPointsPerLevelUp = 5; // 5 stat points per level
    public event EventHandler SpentStatOrSkillPoint;

    // character skill points
    private int unspentSkillPoints;
    private int skillPointsPerLevelUp = 1; // 1 skill point per level

    // Inventory
    // TODO -  need to add a way to save and load inventory
    private int playerCurrency;
    public EventHandler ItemChangeToInventory;
    private ArmorSO helmetEquipped;
    private ArmorSO chestpieceEquipped;
    private ArmorSO leggingsEquipped;
    private WeaponSO mainHandWeaponEquipped;

    // ----------------------------------------------------------------------------------------------------------------

    // When this class is created, will load in data from Gamedata
    public CharacterData(GameData data)
    {
        Load(data);
    }

    // Old load function
    // I'm not sure why it won't work but the current load system of a constructor and a separate load function works
    public void LoadData(GameData data)
    {
        Load(data);
    }

    // loads in data from the recieved GameData
    private void Load(GameData data)
    {
        // Stats need to be loaded first
        strength = data.strength;
        endurance = data.endurance;
        dexterity = data.dexterity;
        intelligence = data.intelligence;

        // loads in unspent skill and stat points
        unspentSkillPoints = data.unspentSkillPoints;
        unspentStatPoints = data.unspentStatPoints;

        // loads in player XP and level data
        playerXP = data.currentXP;
        maxXPForLevel = data.maxXPForLevel;
        playerLevel = data.level;
        
        // loads in player's max and current HP
        // * might remove the current HP if it is not needed
        maxPlayerHP = data.maxHp;
        currentPlayerHP = data.currentHP;

        // sets the max MP and current MP of the player
        // * might remove the current MP if it is not needed
        maxPlayerMP = data.maxMP;
        currentPlayerMP = data.currentMP;

        // sets MP regen based off of the Max MP
        SetMPRegenerationAmount();

        // TODO - make it actually load and save an inventory
        // sets up inventory
        playerCurrency = data.playerCurrency;
    }
    
    // Saves the data in to the GameData
    // same problem as the LoadData(), it won't work so we have a separate Save()
    public void SaveData(ref GameData data)
    {
        // saves stat data
        data.strength = strength;
        data.endurance = endurance;
        data.dexterity = dexterity;
        data.intelligence = intelligence;
        
        // saves unspent skill and stat point data
        data.unspentStatPoints = unspentStatPoints;
        data.unspentSkillPoints = unspentSkillPoints;

        // saves XP and level data
        data.currentXP = playerXP;
        data.maxXPForLevel = maxXPForLevel;
        data.level = playerLevel;

        // saves current and max HP data
        data.maxHp = maxPlayerHP;
        data.currentHP = currentPlayerHP;

        // saves current and max MP data
        data.maxMP = maxPlayerMP;
        data.currentMP = currentPlayerMP;
        
        // TODO - make it actually load and save an inventory
        // sets up inventory
        data.playerCurrency = playerCurrency;
    }

    // Saves the data to the GameData that is being used by the game and handled in the PlayerManager
    public void Save()
    {
        // saves XP and level data
        PlayerManager.Instance.currentGameData.currentXP = playerXP;
        PlayerManager.Instance.currentGameData.maxXPForLevel = maxXPForLevel;
        PlayerManager.Instance.currentGameData.level = playerLevel;

        // saves stat data
        PlayerManager.Instance.currentGameData.strength = strength;
        PlayerManager.Instance.currentGameData.endurance = endurance;
        PlayerManager.Instance.currentGameData.dexterity = dexterity;
        PlayerManager.Instance.currentGameData.intelligence = intelligence;

        // saves unspent skill and stat point data
        PlayerManager.Instance.currentGameData.unspentStatPoints = unspentStatPoints;
        PlayerManager.Instance.currentGameData.unspentSkillPoints = unspentSkillPoints;

        // saves current and max HP data
        PlayerManager.Instance.currentGameData.maxHp = maxPlayerHP;
        PlayerManager.Instance.currentGameData.currentHP = currentPlayerHP;

        // saves current and max MP data
        PlayerManager.Instance.currentGameData.maxMP = maxPlayerMP;
        PlayerManager.Instance.currentGameData.currentMP = currentPlayerMP;

        // TODO - make it actually load and save an inventory
        PlayerManager.Instance.currentGameData.playerCurrency = playerCurrency;
    }
    
    // Returns the current player HP
    public int GetPlayerXP()
    {
        return playerXP;
    }

    // Adds XP to the player from the parameter
    public void AddPlayerXP(int xpEarned)
    {
        // adds the XP
        playerXP += xpEarned;

        // save data
        Save();

        // sets off the PlayerGainedXP event 
        // (mainly used for the XP bar to change)
        PlayerGainedXP?.Invoke(this, EventArgs.Empty);
    }

    // Returns the max XP to go to the next
    public int GetMaxXP()
    {
        return maxXPForLevel;
    }

    // Returns the current player level
    public int GetPlayerLevel()
    {
        return playerLevel;
    }

    // Increases the player's level by the inputted value, the default number is 1 level up
    public void IncreasePlayerLevel(int levelIncrease = 1)
    {
        // Increases player level
        playerLevel += levelIncrease;
        // subtracts the max XP for the level the player just completed to save the left over XP for the next level
        playerXP -= maxXPForLevel;
        // calculated the amount of XP to hit the next level
        maxXPForLevel = XPToHitNextLevel();

        // Calculates the new HP for the player's level
        maxPlayerHP = GetNewMaxHP();
        // sets the current HP to be the new max (basically heals the player to max on level up)
        currentPlayerHP = maxPlayerHP;

        // Calculates the new max MP for the player
        // the new MP regeneration amount is set in the GetNewMaxMP() function
        maxPlayerMP = GetNewMaxMP();
        // sets the current MP to be max (basically regens the whole mana bar on level up)
        currentPlayerMP = maxPlayerMP;

        // the correct skill points and stat points per level up
        // value is set at the top of the function
        unspentStatPoints += statPointsPerLevelUp;
        unspentSkillPoints += skillPointsPerLevelUp;

        // saves the player's data
        Save();

        // Sets off the PlayerLeveledUp event for the other scripts to know when a level up occurs
        // this is mainly for the UI elements
        PlayerLeveledUp?.Invoke(this, EventArgs.Empty);
    }

    // sets the max XP needed to level up to the next level
    private int XPToHitNextLevel()
    {
        // XP values follow the formula: 100*(1.25^(Level))
        // Ex: level 0 to 1 is 100 XP, level 1 to 2 is 125 XP, level 2 to 3 is 156, etc.
        return Mathf.FloorToInt(100 * Mathf.Pow(1.25f, playerLevel));
    }

    // returns the Max HP of the player
    public int GetMaxHP()
    {
        return maxPlayerHP;
    }

    // returns the current HP of the player
    public int GetCurrentHP()
    {
        return currentPlayerHP;
    }

    // return the new max HP of the player based on level and endurance points
    private int GetNewMaxHP()
    {
        // Max HP follows the formula of 25*(level+1)+(10*END)
        return 25*(playerLevel+1)+(10*endurance);
    }

    // Decreases the player's HP based on the damage calculation
    public void PlayerHit(int attack)
    {
        // if the player's HP is above 0
        // this check is need so HP doesn't go into the negatives during the death animation
        if(currentPlayerHP > 0)
        {
            int defense = 0;
            if(helmetEquipped != null)
            {
                defense += helmetEquipped.defense;
            }
            if(chestpieceEquipped != null)
            {
                defense += chestpieceEquipped.defense;
            }
            if(leggingsEquipped != null)
            {
                defense += leggingsEquipped.defense;
            }

            // subtracts the damage taken based on damage calculation
            currentPlayerHP = currentPlayerHP - PlayerManager.Instance.DamageCalculation(attack, defense);
            
            // sets of the event PlayerTookDamage, so other scripts can update accordingly
            // mainly used for the HP bar
            PlayerTookDamage?.Invoke(this, EventArgs.Empty);
        }

        // saves the game's
        Save();
    }

    // returns the current MP of the player
    public int GetMaxMP()
    {
        return maxPlayerMP;
    }
    
    // returns the current MP of the player
    public int GetCurrentMP()
    {
        return currentPlayerMP;
    }

    // return the new max HP of the player based on level and intelligence points
    private int GetNewMaxMP()
    {
        // Player's MP follows the equation: level*10 + (10*INT)
        int newMaxMP = (playerLevel)*10+(10*intelligence);
        
        // sets a new MP regeneration amount
        SetMPRegenerationAmount();

        return newMaxMP;
    }

    // regenerates MP for the player
    public void RegenMP()
    {
        //checks to see if MP + regeneration is less than or equal to the Max MP
        if(currentPlayerMP + mpRegenerationAmount <= maxPlayerMP)
        {
            // if so, then it just adds the regeneration amount
            currentPlayerMP +=  mpRegenerationAmount;
            // lets the other scripts know that MP has changed
            PlayerManaChanged?.Invoke(this, EventArgs.Empty);
            // saves the game
            Save();
        }
        // if adding the regen amount is greater than max
        else if(currentPlayerMP < maxPlayerMP)
        {
            // current MP is set to max MP
            currentPlayerMP = maxPlayerMP;

            // lets the other scripts know that MP has changed
            PlayerManaChanged?.Invoke(this, EventArgs.Empty);
            // saves the game
            Save();
        }
        // the only case this doesn't check for if current = max, so it does nothing
    }

    // sets the MP regeneration amount based on the max MP
    private void SetMPRegenerationAmount()
    {
        // regeneration amount is equal to Max*mpRegenerationPercentage
        // mpRegenerationPercentage is declares in the variables section, currently is 5% MP every second
        mpRegenerationAmount = Mathf.CeilToInt(maxPlayerMP * mpRegenerationPercentage);

        // if the MP regeneration amound is 0, it will be set to 1
        // I'm not sure how it would hit this since it's a Ceil, but if it ever does, it will 1
        if(mpRegenerationAmount == 0)
        {
            mpRegenerationAmount = 1;
        }
    }

    // returns the currently equipped weaponSO
    public WeaponSO getCurrentWeapon()
    {
        return currentWeapon;
    }

    // resets the player's HP to full, this should only be used when the player dies
    public void RestoreHPToFull()
    {
        // currentHP is set to max HP
        currentPlayerHP = maxPlayerHP;
        // lets the other scripts know the player healed
        PlayerHealed?.Invoke(this, EventArgs.Empty);
        // saves the game data
        Save();
    }

    // restores the player's HP to half
    // this was used when trying to get the player to reset (not respawn) and works the same as restore HP to full
    public void RestoreHPToHalf()
    {
        currentPlayerHP = maxPlayerHP/2;
        PlayerHealed?.Invoke(this, EventArgs.Empty);
        Save();
    }
    
    // restores the player's HP based off the inputted HP
    public void RestoreHP(int hpToRestore)
    {
        // checks to see if HP + regeneration is greater than the Max HP
        if(currentPlayerHP + hpToRestore > maxPlayerHP)
        {
            // if it is, currentHP is set to the Max
            currentPlayerHP = maxPlayerHP;
            // lets the other scripts know that the player's HP went up
        }
        // if HP + regeneration is less than the Max HP
        else if(currentPlayerHP + hpToRestore < maxPlayerHP)
        {
            // HP is restored by the restore amount
            currentPlayerHP += hpToRestore;
            // lets the other scripts know that the player's HP went up
            PlayerHealed?.Invoke(this, EventArgs.Empty);
        }
        // the only case this doesn't check for if current = max, so it does nothing
    }
    
    // regenerates 1% of the player's HP
    public void RegenHP()
    {
        // finds the amount of HP the player should regenerate
        int hpToRestore = Mathf.CeilToInt(maxPlayerHP/100f);
        // sends that amount to RestoreHP() and is saved there
        RestoreHP(hpToRestore);
    }

    // Resets (kills) the player
    public void ResetPlayer()
    {
        // sets the currentHP to 0, where PlayerController will recognize the player is dead
        currentPlayerHP = 0;
        // lets the other scripts know the player has taken damage
        PlayerTookDamage?.Invoke(this, EventArgs.Empty);
    }

    // returns the player's strength stat
    public int GetSTR()
    {
        return strength;
    }
    
    // returns the player's endurance stat
    public int GetEND()
    {
        return endurance;
    }
    
    // returns the player's dexterity stat
    public int GetDEX()
    {
        return dexterity;
    }
    
    // returns the player's intelligence stat
    public int GetINT()
    {
        return intelligence;
    }

    // returns the player's unspent stat points
    public int GetUnspentStatPoints()
    {
        return unspentStatPoints;
    }

    // increases a stat based on the number inputted
    public void SpendStatPoint(int statSlot)
    {
        // statSlot refers to the slot it is in the stats UI menu from top to bottom

        if(statSlot == 1)
        {
            // spent on the 1st stat slot (STR)
            strength++;
            unspentStatPoints--;
        }
        else if(statSlot == 2)
        {
            // spent on the 2nd stat slot (END)
            endurance++;
            unspentStatPoints--;
        }
        else if(statSlot == 3)
        {
            // spent on the 3rd stat slot (DEX)
            dexterity++;
            unspentStatPoints--;
        }
        else if(statSlot == 4)
        {
            // spent on the 4th stat slot (INT)
            intelligence++;
            unspentStatPoints--;
        }

        // recalculates HP and MP in case END or INT were increases
        // Data is saved in SetHPMP()
        SetHPMP();
        
        // sends out the SpentStatOrSkillPoint event
        // mainly used for the Abilities Menu
        SpentStatOrSkillPoint?.Invoke(this, EventArgs.Empty);
    }

    // return the player's unspent skill points
    public int GetUnspentSkillPoints()
    {
        return unspentSkillPoints;
    }

    // TODO: This was used for testing purposes, this should be handled in the individual point allocation
    public void DecreaseUnspentSkillPoints()
    {
        unspentSkillPoints--;
        SpentStatOrSkillPoint?.Invoke(this, EventArgs.Empty);
    }

    // recalculated HP and MP
    private void SetHPMP()
    {
        // calculates and sets player HP
        maxPlayerHP = GetNewMaxHP();

        // calculates and sets player MP
        maxPlayerMP = GetNewMaxMP();

        // saves the character's data
        Save();
    }

    // adds currency to the player's inventory
    public void AddPlayerCurrency(int coinsToAdd)
    {
        // adds the currency to the player's inventory
        playerCurrency += coinsToAdd;

        // lets the inventory script know that the currency amount has changed
        ItemChangeToInventory?.Invoke(this, EventArgs.Empty);

        // saves the data
        Save();
    }

    // returns the player's currency
    public int GetPlayerCurrency()
    {
        return playerCurrency;
    }

    // removes currency from the player's inventory
    public void RemovePlayerCurrency(int coinsToSpend)
    {
        // removes the currency to the player's inventory
        playerCurrency -= coinsToSpend;

        // lets the inventory script know that the currency amount has changed
        ItemChangeToInventory?.Invoke(this, EventArgs.Empty);
        
        // saves the data
        Save();
    }

    // returns the currently equipped helmet armorSO
    public ArmorSO GetEquippedHelmet()
    {
        return helmetEquipped;
    }
    
    // returns the currently equipped chestpiece armorSO
    public ArmorSO GetEquippedChestpiece()
    {
        return chestpieceEquipped;
    }
    
    // returns the currently equipped leggings armorSO
    public ArmorSO GetEquippedLeggings()
    {
        return leggingsEquipped;
    }

    // returns the currently equipped weapon weaponSO
    public WeaponSO GetEquippedWeapon()
    {
        return currentWeapon;
    }

    // equips a helmet on the player
    public void EquipHelmet(ArmorSO armorSO)
    {
        // removes old armor's game model
        if (helmetEquipped != null)
        {
            GameObject.Destroy(PlayerManager.Instance.helmetLocation.transform.GetChild(1).gameObject);
        }

        // shows default clothing, based on armorSO
        if (armorSO.showDefaultArmor)
        {
            ShowDefaultHair(true);
        }
        else
        {
            ShowDefaultHair(false);
        }

        // attaches the helmet's armorSO to the player's helmet data
        helmetEquipped = armorSO;

        // spawns the model on the helmet location
        GameObject.Instantiate(armorSO.helmetPrefab, PlayerManager.Instance.helmetLocation.transform);
    }

    // equips a chestpiece on the player
    public void EquipChestpiece(ArmorSO armorSO)
    {
        // removes old armor's game models
        if(chestpieceEquipped != null)
        {
            GameObject.Destroy(PlayerManager.Instance.torsoLocation.transform.GetChild(1).gameObject);
            GameObject.Destroy(PlayerManager.Instance.leftUpperArmLocation.transform.GetChild(1).gameObject);
            GameObject.Destroy(PlayerManager.Instance.rightUpperArmLocation.transform.GetChild(1).gameObject);
        }

        // shows default clothing, based on armorSO
        if(armorSO.showDefaultArmor)
        {
            ShowDefaultShirt(true);
        }
        else
        {
            ShowDefaultShirt(false);
        }

        // attaches the chestpiece's armorSO to the player's chestpiece data
        chestpieceEquipped = armorSO;
        
        // spawns the model of the chestpiece parts on their respective locations
        GameObject.Instantiate(armorSO.chestPrefab, PlayerManager.Instance.torsoLocation.transform);
        GameObject.Instantiate(armorSO.leftShoulderPrefab, PlayerManager.Instance.leftUpperArmLocation.transform);
        GameObject.Instantiate(armorSO.rightShoulderPrefab, PlayerManager.Instance.rightUpperArmLocation.transform);
    }

    // shows the default shirt of the player based on the state
    private void ShowDefaultShirt(bool state)
    {
        PlayerManager.Instance.defaultTorso.SetActive(state);
        PlayerManager.Instance.defaultRightShoulder.SetActive(state);
        PlayerManager.Instance.defaultLeftShoulder.SetActive(state);
    }

    // shows the default hair of the player based on the state
    private void ShowDefaultHair(bool state)
    {
        PlayerManager.Instance.defaultHelmet.SetActive(state);
    }
    
    // shows the default pants of the player based on the state
    private void ShowDefaultPants(bool state)
    {
        PlayerManager.Instance.defaultWaist.SetActive(state);
        PlayerManager.Instance.defaultRightUpperLeg.SetActive(state);
        PlayerManager.Instance.defaultLeftUpperLeg.SetActive(state);
    }

    // equips a leggings on the player
    public void EquipLeggings(ArmorSO armorSO)
    {
        // removes old armor's game models
        if(leggingsEquipped != null)
        {
            GameObject.Destroy(PlayerManager.Instance.waistLocation.transform.GetChild(1).gameObject);
            GameObject.Destroy(PlayerManager.Instance.leftUpperLegLocation.transform.GetChild(1).gameObject);
            GameObject.Destroy(PlayerManager.Instance.leftLowerLegLocation.transform.GetChild(0).gameObject);
            GameObject.Destroy(PlayerManager.Instance.rightUpperLegLocation.transform.GetChild(1).gameObject);
            GameObject.Destroy(PlayerManager.Instance.rightLowerLegLocation.transform.GetChild(0).gameObject);
        }

        // shows default clothing, based on armorSO
        if(armorSO.showDefaultArmor)
        {
            ShowDefaultPants(true);
        }
        else
        {
            ShowDefaultPants(false);
        }

        // attaches the leggings's armorSO to the player's leggings data
        leggingsEquipped = armorSO;
        
        // spawns the model of the leggings parts on their respective locations
        GameObject.Instantiate(armorSO.waistPrefab, PlayerManager.Instance.waistLocation.transform);
        GameObject.Instantiate(armorSO.rightUpperLegPrefab, PlayerManager.Instance.rightUpperLegLocation.transform);
        GameObject.Instantiate(armorSO.rightLowerLegPrefab, PlayerManager.Instance.rightLowerLegLocation.transform);
        GameObject.Instantiate(armorSO.leftUpperLegPrefab, PlayerManager.Instance.leftUpperLegLocation.transform);
        GameObject.Instantiate(armorSO.leftLowerLegPrefab, PlayerManager.Instance.leftLowerLegLocation.transform);
    }

    // equips a weapon on the player
    public void EquipWeapon(WeaponSO weaponSO)
    {
        // removes old weapons's game model
        if (currentWeapon != null)
        {
            GameObject.Destroy(PlayerManager.Instance.mainHandWeaponLocation.transform.GetChild(1).gameObject);
        }

        // attaches the weapons's weaponSO to the current weapon data
        currentWeapon = weaponSO;
        GameObject.Instantiate(weaponSO.prefab, PlayerManager.Instance.mainHandWeaponLocation.transform);
    }

    // removes the player's current weapon
    public void RemoveWeapon()
    {
        // removes old weapon model
        if (currentWeapon != null)
        {
            // removes the old weapon's model
            GameObject.Destroy(PlayerManager.Instance.mainHandWeaponLocation.transform.GetChild(1).gameObject);
            
            // TODO: change to an armorSO of 0 or check for null on damage calculation and creating a game
            currentWeapon = null;
        }
    }

    // removes the player's current helmet
    public void RemoveHelmet()
    {
        // remove old armor
        if (helmetEquipped != null)
        {
            // removes the old helmet's model
            GameObject.Destroy(PlayerManager.Instance.helmetLocation.transform.GetChild(1).gameObject);

            // TODO: change to an armorSO of 0 or check for null on damage calculation and creating a game
            helmetEquipped = null;

            // shows the default hair
            ShowDefaultHair(true);
        }
    }
    
    // removes the player's current chestpiece
    public void RemoveChestpiece()
    {
        // remove old armor
        if(chestpieceEquipped != null)
        {
            // removes the old chestpiece's models
            GameObject.Destroy(PlayerManager.Instance.torsoLocation.transform.GetChild(1).gameObject);
            GameObject.Destroy(PlayerManager.Instance.leftUpperArmLocation.transform.GetChild(1).gameObject);
            GameObject.Destroy(PlayerManager.Instance.rightUpperArmLocation.transform.GetChild(1).gameObject);

            // TODO: change to an armorSO of 0 or check for null on damage calculation and creating a game
            chestpieceEquipped = null;

            // shows the default shirt
            ShowDefaultShirt(true);
        }
    }

    // removes the player's current leggings
    public void RemoveLeggings()
    {
        // remove old armor
        if(leggingsEquipped != null)
        {
            // removes the old legging's models
            GameObject.Destroy(PlayerManager.Instance.waistLocation.transform.GetChild(1).gameObject);
            GameObject.Destroy(PlayerManager.Instance.leftUpperLegLocation.transform.GetChild(1).gameObject);
            GameObject.Destroy(PlayerManager.Instance.leftLowerLegLocation.transform.GetChild(0).gameObject);
            GameObject.Destroy(PlayerManager.Instance.rightUpperLegLocation.transform.GetChild(1).gameObject);
            GameObject.Destroy(PlayerManager.Instance.rightLowerLegLocation.transform.GetChild(0).gameObject);
            
            // TODO: change to an armorSO of 0 or check for null on damage calculation and creating a game
            leggingsEquipped = null;

            // shows the default pants
            ShowDefaultPants(true);
        }
    }
}
