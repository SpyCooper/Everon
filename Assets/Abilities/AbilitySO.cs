using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ability")]
public class AbilitySO : ScriptableObject
{
    // name of the ability
    public string abilityName;

    // the ability description
    public string abilityDescription;
    
    // the abilities image
    public Sprite abilityImage;

    // the max points that can be put into the skill
    public int maxAllocatedPoints;
}
