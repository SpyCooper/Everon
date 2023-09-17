using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// the script that handles the UI of the equipment slots in the player's inventory
public class CharacterEquipmentUI : MonoBehaviour
{
    public static CharacterEquipmentUI Instance;

    // on Awake, declares the singleton
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private CharacterEquipmentSlot helmetSlot;
    [SerializeField] private GameObject helmetBackgroundIcon;
    [SerializeField] public CharacterEquipmentSlot chestpieceSlot;
    [SerializeField] private GameObject chestpieceBackgroundIcon;
    [SerializeField] private CharacterEquipmentSlot leggingsSlot;
    [SerializeField] private GameObject leggingsBackgroundIcon;
    [SerializeField] private CharacterEquipmentSlot mainhandSlot;
    [SerializeField] private GameObject mainhandBackgroundIcon;
    [SerializeField] private CharacterEquipmentSlot offhandSlot;
    [SerializeField] private GameObject offhandBackgroundIcon;
    [SerializeField] private GameObject cannotEquipNotification;
    [SerializeField] private Image cannotEquipNotificationImage;
    [SerializeField] private TextMeshProUGUI cannotEquipNotificationText;
    private float notificationFadeMultiplier = 1.75f;
    private IEnumerator notificationCoroutine;
    private bool notificationCoroutineRunning = false;

    // on Start
    private void Start()
    {
        // listens to the events for dropping items into the item slots
        helmetSlot.OnItemDroppedInEquipmentSlot += helmetSlot_ItemDropped;
        chestpieceSlot.OnItemDroppedInEquipmentSlot += chestpieceSlot_ItemDropped;
        leggingsSlot.OnItemDroppedInEquipmentSlot += leggingsSlot_ItemDropped;
        mainhandSlot.OnItemDroppedInEquipmentSlot += mainhandSlot_ItemDropped;
        offhandSlot.OnItemDroppedInEquipmentSlot += offhandSlot_ItemDropped;

        // hides the cannot equip notification
        cannotEquipNotification.SetActive(false);
    }

    // runs when an item cannot be equipped
    private void CannotEquipItem(CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // returns the item to it's previous location
        e.item.parentAfterDrag = e.parentBeforeDrop;

        // displays the cannot equip notification
        CannotEquipNotification();
    }

    // when an item is dropped into the helmet slot
    private void helmetSlot_ItemDropped(object sender, CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // checks to make sure the item is has a type of armor and the armor type is a helmet
        if (e.item.itemSO.type != ItemType.Armor || (e.item.itemSO.type == ItemType.Armor && e.item.itemSO.armorSO.armorType != ArmorType.Head))
        {
            // if the item fails either of those, the player cannot equip the item
            CannotEquipItem(e);
        }
        // checks to make sure the level requirement for the armor is higher than the player's level
        else if (e.item.itemSO.armorSO.armorLevel > PlayerManager.Instance.currentCharacterData.GetPlayerLevel())
        {
            // if the armor level is higher, the player cannot equip the item
            CannotEquipItem(e);
        }
        // if the item matches the helmet type and the player can equip it
        else
        {
            // checks to see if an item is already equipped
            if (e.previousItem != null)
            {
                // if there was already an item equipped, the item is added into the player's inventory
                InventoryManager.Instance.AddItem(e.previousItem.itemSO, e.previousItem.stackAmount);

                // the previous item's gameobject is destroyed
                Destroy(e.previousItem.gameObject);
            }

            // checks to make sure there is an armorSO attached to the item SO (used for coding)
            if(e.item.itemSO.armorSO != null)
            {
                // calls the character data function to equip the armor
                PlayerManager.Instance.currentCharacterData.EquipHelmet(e.item.itemSO.armorSO);

                // sets the background icon of the slot to be hidden
                helmetBackgroundIcon.SetActive(false);
            }
        }
    }

    // when an item is dropped into the chestpiece slot
    private void chestpieceSlot_ItemDropped(object sender, CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // checks to make sure the item is has a type of armor and the armor type is a chestpiece
        if(e.item.itemSO.type != ItemType.Armor || (e.item.itemSO.type == ItemType.Armor && e.item.itemSO.armorSO.armorType != ArmorType.Chest))
        {
            // if the item fails either of those, the player cannot equip the item
            CannotEquipItem(e);
        }
        // checks to make sure the level requirement for the armor is higher than the player's level
        else if(e.item.itemSO.armorSO.armorLevel > PlayerManager.Instance.currentCharacterData.GetPlayerLevel())
        {
            // if the armor level is higher, the player cannot equip the item
            CannotEquipItem(e);
        }
        // if the item matches the chestpiece type and the player can equip it
        else
        {
            // checks to see if an item is already equipped
            if(e.previousItem != null)
            {
                // if there was already an item equipped, the item is added into the player's inventory
                InventoryManager.Instance.AddItem(e.previousItem.itemSO, e.previousItem.stackAmount);

                // the previous item's gameobject is destroyed
                Destroy(e.previousItem.gameObject);
            }
            
            // checks to make sure there is an armorSO attached to the item SO (used for coding)
            if(e.item.itemSO.armorSO != null)
            {
                // calls the character data function to equip the armor
                PlayerManager.Instance.currentCharacterData.EquipChestpiece(e.item.itemSO.armorSO);

                // sets the background icon of the slot to be hidden
                chestpieceBackgroundIcon.SetActive(false);
            }
        }
    }

    // when an item is dropped into the leggings slot
    private void leggingsSlot_ItemDropped(object sender, CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // checks to make sure the item is has a type of armor and the armor type is a leggings
        if(e.item.itemSO.type != ItemType.Armor || (e.item.itemSO.type == ItemType.Armor && e.item.itemSO.armorSO.armorType != ArmorType.Leggings))
        {
            // if the item fails either of those, the player cannot equip the item
            CannotEquipItem(e);
        }
        // checks to make sure the level requirement for the armor is higher than the player's level
        else if(e.item.itemSO.armorSO.armorLevel > PlayerManager.Instance.currentCharacterData.GetPlayerLevel())
        {
            // if the armor level is higher, the player cannot equip the item
            CannotEquipItem(e);
        }
        // if the item matches the leggings type and the player can equip it
        else
        {
            // checks to see if an item is already equipped
            if(e.previousItem != null)
            {
                // if there was already an item equipped, the item is added into the player's inventory
                InventoryManager.Instance.AddItem(e.previousItem.itemSO, e.previousItem.stackAmount);

                // the previous item's gameobject is destroyed
                Destroy(e.previousItem.gameObject);
            }
            
            // checks to make sure there is an armorSO attached to the item SO (used for coding)
            if(e.item.itemSO.armorSO != null)
            {
                // calls the character data function to equip the armor
                PlayerManager.Instance.currentCharacterData.EquipLeggings(e.item.itemSO.armorSO);

                // sets the background icon of the slot to be hidden
                leggingsBackgroundIcon.SetActive(false);
            }
        }
    }

    // when an item is dropped into the main hand weapon slot
    private void mainhandSlot_ItemDropped(object sender, CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // checks to make sure the item is has a type of weapon
        if(e.item.itemSO.type != ItemType.Weapon)
        {
            // if the item fails either of those, the player cannot equip the item
            CannotEquipItem(e);
        }
        // checks to make sure the level requirement for the weapon is higher than the player's level
        else if(e.item.itemSO.weaponSO.weaponLevel > PlayerManager.Instance.currentCharacterData.GetPlayerLevel())
        {
            // if the item fails either of those, the player cannot equip the item
            CannotEquipItem(e);
        }
        // if the item matches the weapon type and the player can equip it
        else
        {
            // checks to see if an item is already equipped
            if (e.previousItem != null)
            {
                // if there was already an item equipped, the item is added into the player's inventory
                InventoryManager.Instance.AddItem(e.previousItem.itemSO, e.previousItem.stackAmount);

                // the previous item's gameobject is destroyed
                Destroy(e.previousItem.gameObject);
            }

            // checks to make sure there is a weaponSO attached to the item SO (used for coding)
            if(e.item.itemSO.weaponSO != null)
            {
                // calls the character data function to equip the weapon
                PlayerManager.Instance.currentCharacterData.EquipWeapon(e.item.itemSO.weaponSO);
                
                // sets the background icon of the slot to be hidden
                mainhandBackgroundIcon.SetActive(false);
            }
        }
    }

    // when an item is dropped into the offhand weapon slot (currently not being used)
    private void offhandSlot_ItemDropped(object sender, CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // item dropped into off-hand slot
        Debug.Log("Equipped " + e.item.itemSO.itemName);
    }

    // when a piece of equipment is removed
    public void EquipmentRemoved(ItemSO itemSO)
    {
        // checks to see if it is a weapon or armor type

        //if it is a weapon
        if(itemSO.type == ItemType.Weapon)
        {
            // calls the remove weapons function from character data 
            PlayerManager.Instance.currentCharacterData.RemoveWeapon();

            // turns the background image for the weapon back on
            mainhandBackgroundIcon.SetActive(true);
        }
        // if the item is an armor piece
        else if(itemSO.type == ItemType.Armor)
        {
            // determines the armor type, removes the item, and turns the background image back on

            if(itemSO.armorSO.armorType == ArmorType.Head)
            {
                PlayerManager.Instance.currentCharacterData.RemoveHelmet();
                helmetBackgroundIcon.SetActive(true);
            }
            else if(itemSO.armorSO.armorType == ArmorType.Chest)
            {
                PlayerManager.Instance.currentCharacterData.RemoveChestpiece();
                chestpieceBackgroundIcon.SetActive(true);
            }
            else if(itemSO.armorSO.armorType == ArmorType.Leggings)
            {
                PlayerManager.Instance.currentCharacterData.RemoveLeggings();
                leggingsBackgroundIcon.SetActive(true);
            }
        }
    }

    // shows the cannot equip notification
    private void CannotEquipNotification()
    {
        // checks to see if a cannot equip notification is already running
        if(notificationCoroutineRunning)
        {
            // if there is one, it stops the coroutine
            StopCoroutine(notificationCoroutine);
        }

        // starts the cannot equip notification
        notificationCoroutine = DisplayNotification();
        StartCoroutine(notificationCoroutine);
    }

    // cannot equip notification coroutine
    private IEnumerator DisplayNotification()
    {
        // sets the coroutine as running
        notificationCoroutineRunning = true;

        // gets the mouse position and shows the notification
        cannotEquipNotification.transform.position = Input.mousePosition;
        cannotEquipNotification.SetActive(true);

        // resets the color of the text and image of the notification
        cannotEquipNotificationImage.color = new Color(cannotEquipNotificationImage.color.r, cannotEquipNotificationImage.color.g, cannotEquipNotificationImage.color.b, 1);
        cannotEquipNotificationText.color = new Color(cannotEquipNotificationText.color.r, cannotEquipNotificationText.color.g, cannotEquipNotificationText.color.b, 1);

        // keeps the notification from fading
        yield return new WaitForSeconds(0.75f);

        // fades the notification out
        for (float i = 1; i >= 0; i -= (Time.deltaTime*notificationFadeMultiplier))
        {
            // fades the notification out as i decreases
            cannotEquipNotificationImage.color = new Color(cannotEquipNotificationImage.color.r, cannotEquipNotificationImage.color.g, cannotEquipNotificationImage.color.b, i);
            cannotEquipNotificationText.color = new Color(cannotEquipNotificationText.color.r, cannotEquipNotificationText.color.g, cannotEquipNotificationText.color.b, i);
            yield return null;
        }
        
        // hides the notification and sets the coroutine as not running
        cannotEquipNotification.SetActive(false);
        notificationCoroutineRunning = false;
    }

}
