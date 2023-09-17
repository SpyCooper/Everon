using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// the class that handles each individual equipment slot
public class CharacterEquipmentSlot : MonoBehaviour, IDropHandler
{
    public event EventHandler<OnItemDroppedEventArgs> OnItemDroppedInEquipmentSlot;

    // EventArgs required for the UI script
    public class OnItemDroppedEventArgs : EventArgs
    {
        public InventoryItem item;
        public Transform parentBeforeDrop;
        public InventoryItem previousItem;
    }
    
    // when an item is dropped into the slot
    public void OnDrop(PointerEventData eventData)
    {
        // checks and grabs the previous item in the slot
        InventoryItem previousItemInSlot = null;
        if(transform.childCount != 0)
        {
            previousItemInSlot = transform.GetChild(0).GetComponent<InventoryItem>();
        }

        // gets the data of the item that has been dropped
        GameObject dropped = eventData.pointerDrag;
        InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();
        Transform previousSlot = inventoryItem.parentAfterDrag;
        inventoryItem.parentAfterDrag = transform;

        // sets off the event that an item has been dropped into the equipment slot
        OnItemDroppedInEquipmentSlot?.Invoke(this, new OnItemDroppedEventArgs {item = inventoryItem, parentBeforeDrop = previousSlot, previousItem = previousItemInSlot});
    }

    // public void EquipArmor(InventoryItem inventoryItem)
    // {
    //     InventoryItem previousItemInSlot = null;
    //     if(transform.childCount != 0)
    //     {
    //         previousItemInSlot = transform.GetChild(0).GetComponent<InventoryItem>();
    //     }
    //     Transform previousSlot = inventoryItem.parentAfterDrag;
    //     inventoryItem.parentAfterDrag = transform;
    //     OnItemDroppedInEquipmentSlot?.Invoke(this, new OnItemDroppedEventArgs {item = inventoryItem, parentBeforeDrop = previousSlot, previousItem = previousItemInSlot});
    // }
}
