using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    // sets up an item in the world

    // a static function to create an item in the world
    public static ItemWorld SpawnItemWorld(Vector3 position, ItemSO itemSO, int newStackAmount = 1)
    {
        // the offset of the item model from the ground
        float yIncrease = 0.4f;

        // if the item is a weapon or armor, the model is bigger so it needs to be offset more
        if(itemSO.type == ItemType.Weapon || itemSO.type == ItemType.Armor)
        {
            yIncrease += 0.1f;
        }

        // gets the position that the item should spawn and spawns the item model
        Vector3 objectPosition = new Vector3(position.x, position.y + yIncrease, position.z);
        Transform transform = Instantiate(InventoryManager.Instance.itemWorldPrefab, objectPosition, Quaternion.identity);

        // sets the item and data for the item that is spawned in the world
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(itemSO);
        itemWorld.setStackAmount(newStackAmount);

        return itemWorld;
    }

    // drops the item in the world in a random spot around the player
    public static ItemWorld DropItemWorld(Vector3 position, ItemSO itemSO, int newStackAmount = 1)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        if(randomDirection.x < 0.4f)
        {
           randomDirection.x = randomDirection.x*2;
        }
        if(randomDirection.y < 0.4f)
        {
           randomDirection.y = randomDirection.y*2;
        }
        Vector3 randomVector = new Vector3(randomDirection.x * 3, 0f, randomDirection.y*2);
        // sets the item and data for the item that is spawned in the world
        ItemWorld itemWorld = SpawnItemWorld(position + randomVector, itemSO, newStackAmount);

        return itemWorld;
    }

    private ItemSO itemSO;
    private int stackAmount = 1;

    // sets the item
    public void SetItem(ItemSO itemSO)
    {
        this.itemSO = itemSO;
        GameObject itemModel = Instantiate(itemSO.inWorldModel, transform);
    }

    // sets the stack amount for the item in the world
    public void setStackAmount(int newStackAmount)
    {
        stackAmount = newStackAmount;
    }

    // returns the item
    public ItemSO GetItem()
    {
        return itemSO;
    }

    // returns the stackAmount
    public int GetStackAmount()
    {
        return stackAmount;
    }

    // destroys the item
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
