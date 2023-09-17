using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Armor")]
public class ArmorSO : ScriptableObject
{
    // the scriptable object for armor

    public string armorName;
    public string description;

    // add the icon texture
    public Sprite icon;

    // add the stats
    public int defense;
    public int armorLevel;
    public ArmorType armorType;

    public bool showDefaultArmor;
    // public ItemSO itemSO;

    // add the game model
    [Header("Helmet")]
    public GameObject helmetPrefab;
    
    [Header("Chestpiece")]
    public GameObject chestPrefab;
    public GameObject rightShoulderPrefab;
    public GameObject leftShoulderPrefab;

    [Header("Leggings")]
    public GameObject waistPrefab;
    public GameObject rightUpperLegPrefab;
    public GameObject rightLowerLegPrefab;
    public GameObject leftUpperLegPrefab;
    public GameObject leftLowerLegPrefab;

}


public enum ArmorType
{
    Head,
    Chest,
    Leggings,
}
