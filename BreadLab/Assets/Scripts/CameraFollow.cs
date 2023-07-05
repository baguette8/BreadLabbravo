using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // Assign your Player's Transform in Inspector
    public float height = 10.0f; // Adjust this value as needed in Inspector for the height of the camera above the player

    private void LateUpdate()
    {
        // Follow the Player
        if(playerTransform != null)
        {
            Vector3 newPosition = new Vector3(playerTransform.position.x, playerTransform.position.y + height, playerTransform.position.z);
            transform.position = newPosition;

            // Always look at the player
            transform.LookAt(playerTransform);
        }
    }
}