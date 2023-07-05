using UnityEngine;

public class PredatorSpawner : MonoBehaviour
{
    public GameObject[] predatorPrefabs = new GameObject[5];
    public int predatorCount = 10;
    public float planeSize = 100f;

    void Start()
    {
        for (int i = 0; i < predatorCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-planeSize / 2, planeSize / 2), 0, Random.Range(-planeSize / 2, planeSize / 2));
            int randomIndex = Random.Range(0, predatorPrefabs.Length);
            Instantiate(predatorPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        }
    }
}