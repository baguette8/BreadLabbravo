using UnityEngine;

public class SpawnCritters : MonoBehaviour
{
    public GameObject[] critterPrefabs = new GameObject[5];
    public int critterCount = 50;
    public float planeSize = 100f;

    void Start()
    {
        for (int i = 0; i < critterCount; i++)
        {
            float randomX = Random.Range(-planeSize / 2, planeSize / 2);
            float randomZ = Random.Range(-planeSize / 2, planeSize / 2);
            Vector3 spawnPosition = new Vector3(randomX, 0, randomZ);

            // Select a random critter prefab from the array
            GameObject critterPrefab = critterPrefabs[Random.Range(0, critterPrefabs.Length)];

            Instantiate(critterPrefab, spawnPosition, Quaternion.identity);
        }
    }
}