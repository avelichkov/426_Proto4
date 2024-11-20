using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject pentagonPrefab;  // Regular enemy prefab
    public GameObject trianglePrefab; // Dashing enemy prefab
    public GameObject diamondPrefab;  // Shooting enemy prefab

    public float spawnInterval = 2f;  // Time between spawns
    public float spawnRadius = 10f;  // Spawn radius around the player

    private Transform player;

    // Spawn percentages (must add up to 1 or 100%)
    [Range(0, 1)] public float pentagonSpawnChance = 0.8f; 
    [Range(0, 1)] public float triangleSpawnChance = 0.15f;
    [Range(0, 1)] public float diamondSpawnChance = 0.05f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Wait for the next spawn
            yield return new WaitForSeconds(spawnInterval);

            // Determine enemy type to spawn
            float randomValue = Random.value;
            GameObject enemyToSpawn = null;

            if (randomValue <= pentagonSpawnChance)
            {
                enemyToSpawn = pentagonPrefab;
            }
            else if (randomValue <= pentagonSpawnChance + triangleSpawnChance)
            {
                enemyToSpawn = trianglePrefab;
            }
            else
            {
                enemyToSpawn = diamondPrefab;
            }

            // Calculate random spawn position around the player
            Vector2 spawnPosition = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;

            // Spawn the enemy
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
