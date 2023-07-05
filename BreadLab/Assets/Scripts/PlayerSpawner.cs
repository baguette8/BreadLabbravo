using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Assign your Player Prefab in Inspector
    public int playerLives = 3; // Number of player lives

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        // Check if player already exists
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            return; // Player already exists, so we don't spawn a new one
        }

        // Check if player has lives left
        if (playerLives <= 0)
        {
            Debug.Log("Game Over");
            return; // No lives left, so we don't spawn a new player
        }

        // As the plane is 100x100 and centered at (0,0,0), the center point will be (0,0,0)
        Vector3 spawnPosition = new Vector3(0, 0.5f, 0); // Assuming the plane's y value is 0 and player's height is 1

        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        // Decrease the number of lives
        playerLives--;
    }
}