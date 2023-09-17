using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Weapon")]
public class WeaponSO : ScriptableObject
{
    // the scriptable object for the weapons

    public string weaponName;
    public string description;

    // add the game model
    public GameObject prefab;

    // add the icon texture
    public Sprite icon;

    // add the stats
    public int attack;
    public int weaponLevel;
}
