using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUI : MonoBehaviour
{
    // was an attempt at over generalizing skills

    [SerializeField] private TextMeshProUGUI className;

    [SerializeField] private int abilitySlotsUsed;

    [SerializeField] private AbilitySO ability1;
    private GameObject slot1;
    private Button increaseSlot1;
    private TextMeshProUGUI slot1Name;
    private TextMeshProUGUI pointsAllocatedSlot1;

    [SerializeField] private AbilitySO ability2;
    private GameObject slot2;
    private Button increaseSlot2;
    private TextMeshProUGUI slot2Name;
    private TextMeshProUGUI pointsAllocatedSlot2;

    [SerializeField] private AbilitySO ability3;
    private GameObject slot3;
    private Button increaseSlot3;
    private TextMeshProUGUI slot3Name;
    private TextMeshProUGUI pointsAllocatedSlot3;
    
    [SerializeField] private AbilitySO ability4;
    private GameObject slot4;
    private Button increaseSlot4;
    private TextMeshProUGUI slot4Name;
    private TextMeshProUGUI pointsAllocatedSlot4;
    
    [SerializeField] private AbilitySO ability5;
    private GameObject slot5;
    private Button increaseSlot5;
    private TextMeshProUGUI slot5Name;
    private TextMeshProUGUI pointsAllocatedSlot5;

    private void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        slot1 = GameObject.Find("AbilitySlot1");
        slot2 = GameObject.Find("AbilitySlot2");
        slot3 = GameObject.Find("AbilitySlot3");
        slot4 = GameObject.Find("AbilitySlot4");
        slot5 = GameObject.Find("AbilitySlot5");

        switch(abilitySlotsUsed)
        {
            case 1:
                //set up ability 1 only
                RefreshAbility1();
                break;
            case 2:
                //set up ability 1 and 2
                RefreshAbility1();
                break;
            case 3:
                //set up ability 1, 2, and 3
                RefreshAbility1();
                break;
            case 4:
                //set up ability 1, 2, 3, and 4
                RefreshAbility1();
                break;
            case 5:
                //set up ability 1, 2, 3, 4, 5
                RefreshAbility1();
                break;
        }
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void RefreshAbility1()
    {
        slot1Name.text = ability1.abilityName;
        pointsAllocatedSlot1.text = "0"+ "/" + ability1.maxAllocatedPoints;
    }

    private void SetUpAbility1()
    {
        // slot1Name = (TextMeshProUGUI)slot1.Find("Ability Name");
    }
}
