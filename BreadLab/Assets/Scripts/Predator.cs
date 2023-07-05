using UnityEngine;
using System.Collections;

public class Predator : MonoBehaviour
{
    // Public variables
    public GameObject player;
    public GameObject[] critters;
    public GameObject predatorPrefab; // Prefab for predator reproduction
    public float health = 200f;
    public float energy = 10f;
    public float speed = 10f;
    public float awarenessRange = 10f; // The range at which the predator becomes aware of the player
    public float damage = 20f; // Damage dealt to critters on contact
    public float scanInterval = 2f; // Time interval to scan for player
    public float energyGainFromCritter = 5f; // Energy gained from eating a critter
    public float energyGainFromPlayer = 10f; // Energy gained from eating a player
    public float energyLossRate = 1f; // Energy lost per second
    public float wanderTime = 3f; // Time to wander in one direction (reduced for more frequent turns)
    public float attackSpeed = 20f; // Speed during attack
    public float attackDuration = 0.5f; // Duration of attack
    public int crittersEaten = 0; // Number of critters eaten
    public int crittersToReproduce = 15; // Number of critters to eat before reproduction

    // Private variables
    private GameObject target;
    private float wanderTimer = 0f;
    private float attackTimer = 0f;
    private enum State { Prowl, Chase, Attack }
    private State currentState = State.Prowl;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScanForTarget());
    }

    // Coroutine to scan for target
    IEnumerator ScanForTarget()
    {
        while (true)
        {
            // Find the nearest critter
            float minDistance = float.MaxValue;
            foreach (GameObject critter in critters)
            {
                float distance = Vector3.Distance(transform.position, critter.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = critter;
                }
            }

            // If the player is within the awareness range and closer than the nearest critter, target the player
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            if (playerDistance <= awarenessRange && playerDistance < minDistance)
            {
                target = player;
            }

            // Determine state based on distance to target
            if (target != null)
            {
                float targetDistance = Vector3.Distance(transform.position, target.transform.position);
                Vector3 targetDirection = (target.transform.position - transform.position).normalized;
                float targetAngle = Vector3.Angle(transform.forward, targetDirection);

                if (targetDistance <= 1f && targetAngle <= 28f / 2f)
                {
                    currentState = State.Attack;
                    attackTimer = attackDuration;
                }
                else if (targetDistance <= awarenessRange && targetAngle <= 28f / 2f)
                {
                    currentState = State.Chase;
                }
                else
                {
                    currentState = State.Prowl;
                }
            }
            else
            {
                currentState = State.Prowl;
            }

            yield return new WaitForSeconds(scanInterval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Lose energy over time
        energy -= energyLossRate * Time.deltaTime;
        if (energy <= 0)
        {
            // If out of energy, die
            Destroy(gameObject);
            return;
        }

        // Move based on current state
        switch (currentState)
        {
            case State.Prowl:
                Wander();
                break;
            case State.Chase:
                ChaseTarget();
                break;
            case State.Attack:
                AttackTarget();
                break;
        }
    }

    // Wander around when no target is found
    private void Wander()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            // Turn towards a random direction
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            wanderTimer = wanderTime;
        }
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // Chase the target
    private void ChaseTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction.y = 0; // Prevents the predator from flying into the air
            transform.position += direction * speed * Time.deltaTime;

            // Orient the predator to face the direction of movement
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
        }
    }

    // Attack the target
    private void AttackTarget()
    {
        if (target != null && attackTimer > 0)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction.y = 0; // Prevents the predator from flying into the air
            transform.position += direction * attackSpeed * Time.deltaTime;
            attackTimer -= Time.deltaTime;

            // Orient the predator to face the direction of movement
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, attackSpeed * Time.deltaTime);
        }
        else
        {
            currentState = State.Prowl;
        }
    }

    // Handle collision with other objects
    private void OnCollisionEnter(Collision collision)
    {
        // If the predator collides with a critter or the player, deal damage and gain energy
        if (collision.gameObject == target)
        {
            // The 'Health' class is not defined in this script or imported from another namespace.
            // DEBUG and add health here if we want predators to fight each other
            // Health targetHealth = target.GetComponent<Health>();
            // if (targetHealth != null)
            // {
            //     targetHealth.TakeDamage(damage);
            // }

            if (collision.gameObject == player)
            {
                energy += energyGainFromPlayer;
            }
            else
            {
                energy += energyGainFromCritter;
                crittersEaten++;
                if (crittersEaten >= crittersToReproduce)
                {
                    Instantiate(predatorPrefab, transform.position, Quaternion.identity);
                    crittersEaten = 0;
                }
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else if (collision.gameObject.CompareTag("Barrier")) // If hit a barrier that is not a player, critter, or other predator, reverse course
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
        }
    }
}