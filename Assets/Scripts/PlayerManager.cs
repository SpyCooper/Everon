using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Used to store data about the player object and game data

    public static PlayerManager Instance;

    // on Awake, declares the singleton
    private void Awake()
    {
        Instance = this;
    }

    public GameObject player;
    public GameObject playerMeleeHitBox;
    public CharacterData currentCharacterData;
    public GameData currentGameData;
    [SerializeField] private GameObject playerUI;
    private GraphicRaycaster uiRaycaster;

    // Positions of where the parts of the armor are spawned to
    [Header("Equipment Locations")]
    public LayerMask playerLayerMask;
    public GameObject helmetLocation;
    public GameObject torsoLocation;
    public GameObject leftUpperArmLocation;
    public GameObject rightUpperArmLocation;
    public GameObject leftLowerArmLocation;
    public GameObject rightLowerArmLocation;
    public GameObject waistLocation;
    public GameObject leftUpperLegLocation;
    public GameObject leftLowerLegLocation;
    public GameObject rightUpperLegLocation;
    public GameObject rightLowerLegLocation;
    public GameObject mainHandWeaponLocation;
    [Header("Default Armor")]
    [SerializeField] public GameObject defaultHelmet;
    [SerializeField] public GameObject defaultTorso;
    [SerializeField] public GameObject defaultLeftShoulder;
    [SerializeField] public GameObject defaultRightShoulder;
    [SerializeField] public GameObject defaultWaist;
    [SerializeField] public GameObject defaultLeftUpperLeg;
    [SerializeField] public GameObject defaultRightUpperLeg;

    public bool IsCursorOverUI()
    {
        // checks to see if the mouse is clicking on a UI element on the player's UI
        PointerEventData click_data = new PointerEventData(EventSystem.current);
        List<RaycastResult> click_results = new List<RaycastResult>();
        click_data.position = Mouse.current.position.ReadValue();
        uiRaycaster = playerUI.GetComponent<GraphicRaycaster>();
        uiRaycaster.Raycast(click_data, click_results);
        return click_results.Count == 0;

    }

    // damage taken follows the following formula: Attack * (100/(100+defense)
    public int DamageCalculation(int attack, int defense)
    {
        return (int)((float)attack * (100 / (100f + (float)defense)));
    }
}
