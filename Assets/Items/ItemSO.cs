using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Only Gameplay")]
    public ItemType type;
    public GameObject inWorldModel;

    [Header("Only UI")]
    public string itemName;
    public bool stackable = false;
    public Sprite inventorySprite;

    [Header("Gameplay and UI")]
    public int sellValue;
    
    [Header("Weapon Variables")]
    public WeaponSO weaponSO;

    [Header("Armor Variables")]
    public ArmorSO armorSO;
}

public enum ItemType
{
    Weapon,
    Armor,
    HealthPotion,
    ManaPotion,
    Useless,
    Coin
}
