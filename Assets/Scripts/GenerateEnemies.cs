using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    // Randomly spawns enemies in the attacked game object
    // ** needs a mesh renderer

    // TODO - add a limit to how many there can be

    [SerializeField] private EnemyScriptableObject enemySO;
    [SerializeField] private float spawnTimerMax;
    private float spawnTimer;
    // private Transform spawnRegion;
    private Vector3 spawnAreaCenter;
    private Vector3 spawnAreaScale;

    // on Awake
    private void Awake()
    {
        // gets the mesh renderer and disables it
        // keeping it on during the editing of the map helps visualize where they spawn
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    // on Start
    void Start()
    {
        // gets the size of the box
        spawnAreaCenter = transform.position;
        spawnAreaScale = transform.localScale/2;
        // sets the spawn timer
        spawnTimer = spawnTimerMax;
    }

    private void Update()
    {
        // decreases the spawn timer
        spawnTimer -= Time.deltaTime;

        // when the spawn timer hits 0, an enemy of type EnemySO is randomly spanws
        if(spawnTimer <= 0)
        {
            float randomX = Random.Range(-spawnAreaScale.x, spawnAreaScale.x);
            float randomZ = Random.Range(-spawnAreaScale.y, spawnAreaScale.y);

            Instantiate(enemySO.enemyPrefab, new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ), Quaternion.identity);
            spawnTimer = spawnTimerMax;
        }

    }

}
