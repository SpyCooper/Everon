using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler
{
    [HideInInspector] public ItemSO itemSO;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public int stackAmount = 1;
    public Image image;
    public TextMeshProUGUI stackText;

    private void Awake()
    {
        // PlayerManager.Instance.player.GetComponent<StarterAssetsInputs>().DoubleClicked += StarterAssetsInputs_DoubleClicked;
    }

    // creates a new inventory item
    public void InitialiseItem(ItemSO newItemSO, int newStackAmount)
    {
        itemSO = newItemSO;
        image.sprite = newItemSO.inventorySprite;
        stackAmount = newStackAmount;
        RefreshStackAmount();
    }

    // refreshes the item's stack amount
    public void RefreshStackAmount()
    {
        // if the stack is greater than 1, it shows the amount in the stack
        if(stackAmount == 1)
        {
            stackText.text = "";
        }
        else
        {
            stackText.text = stackAmount.ToString();
        }
    }

    // on begin drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        // saves the parent before the drag
        parentAfterDrag = transform.parent;

        // moves the image in front of the inventory screen
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // makes the image unable to be hit by a raycast
        image.raycastTarget = false;
    }

    // on drag
    public void OnDrag(PointerEventData eventData)
    {
        // follows the mouse position
        transform.position = Input.mousePosition;
    }

    // on end drag
    public void OnEndDrag(PointerEventData eventData)
    {
        // runs if there is a ui element that is underneath the cursor
        if(!PlayerManager.Instance.IsCursorOverUI())
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
        // if there isn't a UI element under the cursor, the player will drop the item
        else
        {
            // drop item
            ItemWorld.DropItemWorld(PlayerManager.Instance.player.transform.position, itemSO, stackAmount);
            // remove item
            Destroy(transform.gameObject);
        }
        
    }

    // doesn't work properly but could be good to add
    
    // private void StarterAssetsInputs_DoubleClicked(object sender, System.EventArgs e)
    // {
    //     Debug.Log("Double clicked an item");
    //     if(itemSO.type == ItemType.Armor || itemSO.type == ItemType.Weapon)
    //     {
    //         CharacterEquipmentUI.Instance.chestpieceSlot.EquipArmor(this);
    //     }
    // }
}
