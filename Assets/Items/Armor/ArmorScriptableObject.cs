using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Weapon")]
public class ArmorScriptableObject : ScriptableObject
{
    // the scriptable object for the Armor

    public string armorName;

    // add the game model
    public GameObject prefab;

    // add the icon texture
    public Sprite icon;

    // add the stats
    public int defense;
    public int armorLevel;
}
