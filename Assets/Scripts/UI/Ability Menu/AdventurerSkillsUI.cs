using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerSkillsUI : MonoBehaviour
{
    // this script is somewhat generalized for easy implementation of classes

    [Header("General")]
    // class name
    [SerializeField] private TextMeshProUGUI className;
    // skill points available icon
    [SerializeField] private GameObject pointsUnspentIcon;
    [SerializeField] private TextMeshProUGUI pointsAvailableText;

    // ability 1 slot (recover)
    [Header("Ability 1")]
    [SerializeField] private AbilitySO ability1SO;
    [SerializeField] private Button increaseSlot1;
    [SerializeField] private TextMeshProUGUI ability1Name;
    [SerializeField] private Image ability1Image;
    [SerializeField] private TextMeshProUGUI pointsAllocatedSlot1;

    // ability 2 slot (arcane burst)
    [Header("Ability 2")]
    [SerializeField] private AbilitySO ability2SO;
    [SerializeField] private Button increaseSlot2;
    [SerializeField] private TextMeshProUGUI ability2Name;
    [SerializeField] private Image ability2Image;
    [SerializeField] private TextMeshProUGUI pointsAllocatedSlot2;
    
    // ability 3 slot (multiattack)
    [Header("Ability 3")]
    [SerializeField] private AbilitySO ability3SO;
    [SerializeField] private Button increaseSlot3;
    [SerializeField] private TextMeshProUGUI ability3Name;
    [SerializeField] private Image ability3Image;
    [SerializeField] private TextMeshProUGUI pointsAllocatedSlot3;

    //colors for the buttons
    private Color canBeClickedColor = new Color(1.000f, 0.725f, 0.000f, 1.000f); 
    private Color cannotBeClickedColor = new Color(0.478f, 0.478f, 0.478f, 1.000f);

    // on Awake
    private void Awake()
    {
        // if increase stat 1 slot button is clicked
        increaseSlot1.onClick.AddListener(() => {
            // checks to see if there are unspent skill points
            if(PlayerManager.Instance.currentCharacterData.GetUnspentSkillPoints() > 0)
            {
                // Add a skillpoint into slot 1
                // in this case, it is Recover
                Debug.Log("Added a skillpoint to Recover");

                // used for testing purposes until skills are introduced
                PlayerManager.Instance.currentCharacterData.DecreaseUnspentSkillPoints();

                // refreshes the skill point allocation text, button colors, and unspent skill point notification
                RefreshPointAllocation();
            }
        });

        // if increase stat 2 slot button is clicked
        increaseSlot2.onClick.AddListener(() => {
            // checks to see if there are unspent skill points
            if(PlayerManager.Instance.currentCharacterData.GetUnspentSkillPoints() > 0)
            {
                // Add a skillpoint into slot 2
                // in this case, it is Arcane Burst
                Debug.Log("Added a skillpoint to Arcane Burst");

                // used for testing purposes until skills are introduced
                PlayerManager.Instance.currentCharacterData.DecreaseUnspentSkillPoints();

                // refreshes the skill point allocation text, button colors, and unspent skill point notification
                RefreshPointAllocation();
            }
        });

        // if increase stat 3 slot button is clicked
        increaseSlot3.onClick.AddListener(() => {
            // checks to see if there are unspent skill points
            if(PlayerManager.Instance.currentCharacterData.GetUnspentSkillPoints() > 0)
            {
                // Add a skillpoint into slot 3
                // in this case, it is Multi-Attack
                Debug.Log("Added a skillpoint to Multi-Attack");

                // used for testing purposes until skills are introduced
                PlayerManager.Instance.currentCharacterData.DecreaseUnspentSkillPoints();

                // refreshes the skill point allocation text, button colors, and unspent skill point notification
                RefreshPointAllocation();
            }
        });
    }

    // on Start
    private void Start()
    {
        // listens to the events it needs
        PlayerManager.Instance.currentCharacterData.PlayerLeveledUp += CharacterData_LevelUp;

        // sets up the ability 1 text and sprites
        ability1Name.text = ability1SO.abilityName;
        ability1Image.sprite = ability1SO.abilityImage;

        // sets up the ability 2 text and sprites
        ability2Name.text = ability2SO.abilityName;
        ability2Image.sprite = ability2SO.abilityImage;

        // sets up the ability 3 text and sprites
        ability3Name.text = ability3SO.abilityName;
        ability3Image.sprite = ability3SO.abilityImage;

        // refreshes the skill point allocation text, button colors, and unspent skill point notification
        RefreshPointAllocation();
    }
    
    // shows the skill menu
    public void Show()
    {
        gameObject.SetActive(true);
    }

    // hides the skill menu
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    // refreshes the skill point allocation text, button colors, and unspent skill point notification
    public void RefreshPointAllocation()
    {
        // sets the point allocation text
        pointsAllocatedSlot1.text = "??" + "/" + ability1SO.maxAllocatedPoints;
        pointsAllocatedSlot2.text = "??" + "/" + ability2SO.maxAllocatedPoints;
        pointsAllocatedSlot3.text = "??" + "/" + ability3SO.maxAllocatedPoints;

        // if there are unspent skill points
        if(PlayerManager.Instance.currentCharacterData.GetUnspentSkillPoints() > 0)
        {
            // sets each of the button's color to show that there are unspent skill points
            increaseSlot1.image.color = canBeClickedColor;
            increaseSlot2.image.color = canBeClickedColor;
            increaseSlot3.image.color = canBeClickedColor;

            // shows the unspent skill point notification with the number of unspent skill points
            pointsAvailableText.text = PlayerManager.Instance.currentCharacterData.GetUnspentSkillPoints().ToString();
            pointsUnspentIcon.SetActive(true);
        }
        else
        {
            // sets each of the button's color to show that there are no unspent skill points
            increaseSlot1.image.color = cannotBeClickedColor;
            increaseSlot2.image.color = cannotBeClickedColor;
            increaseSlot3.image.color = cannotBeClickedColor;

            // hides the unspent skill point notification with the number of unspent skill points
            pointsAvailableText.text = PlayerManager.Instance.currentCharacterData.GetUnspentSkillPoints().ToString();
            pointsUnspentIcon.SetActive(false);
        }
    }

    // when the player levels up, it refresh skill point allocation
    private void CharacterData_LevelUp(object sender, System.EventArgs e)
    {
        RefreshPointAllocation();
    }
}
