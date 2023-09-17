using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public Transform itemWorldPrefab;

    // sets the inventory manager as a singleton
    private void Awake()
    {
        Instance = this;
    }

    // adds an item into the inventory
    public bool AddItem(ItemSO itemSO, int stackAmount)
    {
        // searches for the same item already in the inventory to stack with (if applicable)
        foreach(InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null && itemInSlot.itemSO == itemSO && itemSO.stackable)
            {
                itemInSlot.stackAmount += stackAmount;
                itemInSlot.RefreshStackAmount();
                return true;
            }
        }

        // find the first open slot and adds a new inventory item
        foreach(InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null)
            {
                SpawnNewItemInInventory(itemSO, slot, stackAmount);
                return true;
            }
        }

        // returns false if the item is not added
        return false;
    }

    // spawns a new item in the player's inventory
    private void SpawnNewItemInInventory(ItemSO itemSO, InventorySlot inventorySlot, int stackAmount)
    {
        GameObject newItemGameObject = Instantiate(inventoryItemPrefab, inventorySlot.transform);
        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(itemSO, stackAmount);
    }
}
