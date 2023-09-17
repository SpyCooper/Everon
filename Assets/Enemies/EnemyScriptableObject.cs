using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    // the SO script for enemies (does not include bosses)

    public string enemyName;
    public GameObject enemyPrefab;
    public int enemyLevel;
    public int xpOnKill;
    public int health;
    public int attack;
    public int defense;
}
