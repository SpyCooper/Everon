using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour
{
    // spawns an item in the world

    public ItemSO itemSO;
    public int stackAmount = 1;

    private void Start()
    {
        // used to spawn something in once on start up
        ItemWorld.SpawnItemWorld(transform.position, itemSO, stackAmount);
        Destroy(gameObject);
    }
}
