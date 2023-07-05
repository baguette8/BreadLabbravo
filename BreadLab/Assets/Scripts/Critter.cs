using UnityEngine;

public class Critter : MonoBehaviour
{
    // Enum to represent the state of the critter
    public enum CritterState
    {
        Forage,  // Searching for food
        Alert,   // Aware of a predator
        Retreat  // Running away from a predator
    }

    public float speed = 1.0f;  // Movement speed
    public float health = 100.0f;  // Health points
    public float energy = 100.0f;  // Energy points
    public float runSpeedMultiplier = 2.0f;  // Speed multiplier when running
    public GameObject critterPrefab;  // Prefab to instantiate when reproducing
    public float fieldOfView = 351.0f; // Field of view for detecting predators (360 degrees - 9 degrees blind spot)
    private Vector3 targetDirection;  // Direction the critter is moving towards
    private bool isRunning = false;  // Flag to check if the critter is running
    private int foodCount = 0;  // Count of food eaten
    private CritterState state = CritterState.Forage;  // Initial state is Forage

    void Start()
    {
        // Set initial direction to a random direction
        targetDirection = GetRandomDirection();
    }

    void Update()
    {
        // Determine current speed based on whether the critter is running or not
        float currentSpeed = isRunning ? speed * runSpeedMultiplier : speed;
        // Move and rotate the critter in the target direction
        MoveAndRotate(targetDirection, currentSpeed);

        // Decrease energy based on movement
        energy -= currentSpeed * Time.deltaTime;
        // If energy is depleted, destroy the critter
        if (energy <= 0)
        {
            Destroy(gameObject);
        }

        // If health is depleted, destroy the critter
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        // Randomly change direction with a 2% chance each frame
        if (Random.Range(0, 100) < 2)
        {
            targetDirection = GetRandomDirection();
        }

        // Check for predators within field of view
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, fieldOfView);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Predator"))
            {
                // If a predator is detected, change state to Alert
                state = CritterState.Alert;
                break;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If the critter collides with food
        if (collision.gameObject.CompareTag("Food"))
        {
            // Increase energy and destroy the food
            energy += 50.0f;
            Destroy(collision.gameObject);
            foodCount++;
            // If the critter has eaten 5 food items, reproduce
            if (foodCount >= 5)
            {
                Instantiate(critterPrefab, transform.position, Quaternion.identity);
                foodCount = 0;
            }
        }
        // If the critter collides with a predator
        else if (collision.gameObject.CompareTag("Predator"))
        {
            // Decrease health
            health -= 50.0f;
            // If health is depleted, destroy the critter
            if (health <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                // If the critter survives, start running away from the predator
                isRunning = true;
                targetDirection = -collision.contacts[0].normal; // Run in the opposite direction of the predator
                state = CritterState.Retreat;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // If the critter stops colliding with a predator, stop running and return to foraging
        if (collision.gameObject.CompareTag("Predator"))
        {
            isRunning = false;
            state = CritterState.Forage;
        }
    }

    // Method to get a random direction
    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    // Method to move and rotate the critter in a given direction
    private void MoveAndRotate(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
    }
}