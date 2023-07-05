using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private CharacterController controller;

    [Tooltip("The factor by which the speed is multiplied when sprinting.")]
    public float sprintMultiplier = 4.0f;

    void Start()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // If player is dead, don't process the input
        if (player.state == PlayerState.Dead)
            return;
        
        float moveHorizontal = -Input.GetAxis("Horizontal");
        float moveVertical = -Input.GetAxis("Vertical");

        // Ensure the player does not move if there is no input
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            Vector3 moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
            float speed = player.speed;

            // Check if the shift key is held down for sprinting
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                // Use extra energy for sprinting
                if (player.energy > 0)
                {
                    speed *= sprintMultiplier;
                    player.energy -= Time.deltaTime; // Decrease energy over time
                }
            }

            moveDirection *= speed;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}