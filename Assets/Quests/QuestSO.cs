using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest")]
public class QuestSO : ScriptableObject
{
    // the SO for quests

    public string questName;
    public string questDescription;
    public string objective;
    public int numberOfRewards;
    public Sprite[] rewards;
}
