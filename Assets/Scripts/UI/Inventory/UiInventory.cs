using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    // Controls the UI for the inventory
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    private bool isMenuOpen = false;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private RenderTexture playerModelInventory;

    private void Awake()
    {
        // when the close button is hit, Hide() is run
        closeButton.onClick.AddListener(() => {
            Hide();
        });
    }

    // on Start, sets inventory UI
    private void Start()
    {
        SetInventory();
        // listen for a ability menu key press
        starterAssetsInputs.toggleInventoryMenu += starterAssetsInputs_ToggleInventoryMenu;
        PlayerManager.Instance.currentCharacterData.ItemChangeToInventory += Inventory_OnItemListChange;
        
        // hides the menu
        Hide();
    }

    // hides the menu
    public void Hide()
    {
        isMenuOpen = false;
        gameObject.SetActive(isMenuOpen);
    }

    // shows the menu
    public void Show()
    {
        // shows the menu
        isMenuOpen = true;
        gameObject.SetActive(isMenuOpen);

        // fixes the menu on the screen if it it too far off the edge of the screen
        FixMenuOnScreen();
        
        // refreshes the UI
        RefreshInventoryItems();
    }
    
    // when toggleAbilitiesMenu is active, sets up the XP text
    private void starterAssetsInputs_ToggleInventoryMenu(object sender, System.EventArgs e)
    {
        // if the menu is not open, it opens the menu
        if(!isMenuOpen)
        {
            Show();
            // inventoryMenuPressed must be set to false because of it's type
            starterAssetsInputs.inventoryMenuPressed = false;
        }
        // if the menu is open, it closes the menu
        else
        {
            Hide();
            // questMenuPressed must be set to false because of it's type
            starterAssetsInputs.inventoryMenuPressed = false;
        }
    }

    // * use this for any of the windows that need it
    // adjust the menu of it is more than halfway off the screen
    private void FixMenuOnScreen()
    {
        if(!(transform.position.x < Screen.width && transform.position.x > 0 && transform.position.y < Screen.height && transform.position.y > 0))
        {
            transform.position = new Vector3(Screen.width/2, Screen.height/2);
        }
    }

    // returns if the menu is open
    public bool GetIsMenuOpen()
    {
        return isMenuOpen;
    }
    
    // sets the inventory UI
    public void SetInventory()
    {
        // refreshes the UI
        RefreshInventoryItems();
    }

    // private void SetInventory(Inventory inventory)
    // {
    //     this.inventory = inventory;
    // }

    // refreshes the UI
    private void RefreshInventoryItems()
    {
        coinsText.text = PlayerManager.Instance.currentCharacterData.GetPlayerCurrency().ToString();
    }

    // when the item list changes, the UI is updated
    private void Inventory_OnItemListChange(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }
}
