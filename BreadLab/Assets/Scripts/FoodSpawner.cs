using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public float spawnRate = 1.0f; // The rate at which food will spawn, in seconds
    private Vector3 spawnArea = new Vector3(100, 0, 100);
    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + 1/spawnRate;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnFood();
            nextSpawnTime = Time.time + 1/spawnRate;
        }
    }

    void SpawnFood()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnArea.x / 2, spawnArea.x / 2), 0, Random.Range(-spawnArea.z / 2, spawnArea.z / 2));
        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }
}