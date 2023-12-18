using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotBarSlot : MonoBehaviour, IDropHandler
{
    // when an item is dropped into an inventory slot
    public void OnDrop(PointerEventData eventData)
    {
        // if there is not a child of the current slot, allows the item to be dropped into it
        if (transform.childCount == 0)
        {
            // gets the item that was dropped
            GameObject dropped = eventData.pointerDrag;
            InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();

            // if the item was removed from a equipment slot
            if (inventoryItem.parentAfterDrag.GetComponent<CharacterEquipmentSlot>() != null)
            {
                // remove that equipment piece
                CharacterEquipmentUI.Instance.EquipmentRemoved(inventoryItem.itemSO);
            }

            // adds the item to the current inventory slot
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
